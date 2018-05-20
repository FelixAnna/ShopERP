using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;
using FoodShop.Manager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderShop.Manager.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepositories _orderRepositories;
        private readonly IFoodRepositories _foodRepositories;
        private readonly IFoodPriceRepositories _foodPriceRepositories;

        public OrderService(IOrderRepositories orderRepositories, IFoodRepositories foodRepositories, IFoodPriceRepositories foodPriceRepositories)
        {
            _orderRepositories = orderRepositories;
            _foodRepositories = foodRepositories;
            _foodPriceRepositories = foodPriceRepositories;
        }

        public OrderDto AddOrder(OrderParameter orderParameter, int userId)
        {
            var additionalIds = orderParameter.AdditioalFoodIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)) ?? new List<int>();

            var newOrder = new Order()
            {
                PrimaryFoodId = orderParameter.PrimaryFoodId,
                AdditioalFoodIds = orderParameter.AdditioalFoodIds,
                OrderName = this.GetFoodName(orderParameter.PrimaryFoodId, additionalIds),
                Reduced = orderParameter.Reduced,
                Price = this.GetTotalPrice(orderParameter.PrimaryFoodId, additionalIds) - orderParameter.Reduced,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };
            _orderRepositories.AddOrder(newOrder);
            return OrderDto.ToOrderDto(newOrder);
        }

        public OrderDto GetOrder(int Id)
        {
            var order = _orderRepositories.GetOrder(Id);
            return OrderDto.ToOrderDto(order);
        }

        public bool RemoveOrder(int Id, int userId)
        {
            var order = _orderRepositories.GetOrder(Id);
            if (order != null)
            {
                order.IsDeleted = true;
                order.UpdatedBy = userId;
                order.UpdatedAt = DateTime.Now;
                _orderRepositories.UpdateOrder(order);
                return true;
            }

            return false;
        }

        public IList<OrderDto> SearchOrder(int foodId)
        {
            var orders = _orderRepositories.SearchOrder(foodId);
            return orders.Select(OrderDto.ToOrderDto).ToList();
        }

        public OrderDto UpdateOrder(OrderParameter orderParameter, int userId)
        {
            var additionalIds = orderParameter.AdditioalFoodIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)) ?? new List<int>();

            var order = _orderRepositories.GetOrder(orderParameter.Id);
            if (order != null)
            {
                order.PrimaryFoodId = orderParameter.PrimaryFoodId;
                order.AdditioalFoodIds = orderParameter.AdditioalFoodIds;
                order.OrderName = this.GetFoodName(orderParameter.PrimaryFoodId, additionalIds);
                order.Reduced = orderParameter.Reduced;
                order.Price = this.GetTotalPrice(orderParameter.PrimaryFoodId, additionalIds) - orderParameter.Reduced;
                order.UpdatedBy = userId;
                order.UpdatedAt = DateTime.Now;
                return OrderDto.ToOrderDto(_orderRepositories.UpdateOrder(order));
            }

            return null;
        }

        private string GetFoodName(int primaryId, IEnumerable<int> additionalIds)
        {
            var primaryFood = _foodRepositories.GetFood(primaryId);
            var additionalFoods = _foodRepositories.GetFood(additionalIds);

            if (additionalFoods.Any())
            {
                var foods = additionalIds
                    .Select(x => additionalFoods.FirstOrDefault(y => y.Id == x))
                    .GroupBy(x => x.FoodName)
                    .Select(g =>
                    {
                        if (g.Count() > 1)
                            return $"{g.Key}*{g.Count()}";
                        return g.Key;
                    })
                    ;
                additionalFoods.Select(x => x.FoodName);
                return $"{primaryFood.FoodName}({string.Join(',', foods)})";
            }
            else
            {
                return primaryFood.FoodName;
            }
        }

        private float GetTotalPrice(int primaryId, IEnumerable<int> additionalIds)
        {
            var primaryFoodPrice = _foodPriceRepositories.GetCurrentFoodPrice(primaryId);
            var additionalFoodPrice = additionalIds.Select(x => _foodPriceRepositories.GetCurrentFoodPrice(x));
            if (additionalFoodPrice.Any())
            {
                return primaryFoodPrice.Price + additionalFoodPrice.Sum(x => x.Price);
            }
            else
            {
                return primaryFoodPrice.Price;
            }
        }
    }
}
