using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;
using FoodShop.Manager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.Services
{
    public class FoodPriceService : IFoodPriceService
    {
        private readonly IFoodPriceRepositories _foodPriceRepositories;

        public FoodPriceService(IFoodPriceRepositories foodPriceRepositories)
        {
            _foodPriceRepositories = foodPriceRepositories;
        }

        public FoodPriceDto AddFoodPrice(FoodPriceParameter foodPriceParameter, int userId)
        {
            var newFoodPrice = new FoodPrice()
            {
                FoodId = foodPriceParameter.FoodId,
                Price = foodPriceParameter.Price,
                StartDate = foodPriceParameter.StartDate ?? DateTime.Now,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };
            _foodPriceRepositories.AddFoodPrice(newFoodPrice);
            return FoodPriceDto.ToFoodPriceDto(newFoodPrice);
        }

        public FoodPriceDto GetFoodPrice(int Id)
        {
            var foodPrice = _foodPriceRepositories.GetFoodPrice(Id);
            return FoodPriceDto.ToFoodPriceDto(foodPrice);
        }

        public bool RemoveFoodPrice(int Id, int userId)
        {
            var foodPrice = _foodPriceRepositories.GetFoodPrice(Id);
            if (foodPrice != null)
            {
                foodPrice.IsDeleted = true;
                foodPrice.UpdatedBy = userId;
                foodPrice.UpdatedAt = DateTime.Now;
                _foodPriceRepositories.UpdateFoodPrice(foodPrice);
                return true;
            }

            return false;
        }

        public FoodPriceDto GetCurrentFoodPrice(int foodId)
        {
            var foodPrice = _foodPriceRepositories.GetCurrentFoodPrice(foodId);
            return FoodPriceDto.ToFoodPriceDto(foodPrice);
        }

        public IList<FoodPriceDto> SearchFoodPrice(int foodId)
        {
            var foodPrices = _foodPriceRepositories.SearchFoodPrice(foodId);
            return foodPrices.Select(FoodPriceDto.ToFoodPriceDto).ToList();
        }

        public FoodPriceDto UpdateFoodPrice(FoodPriceParameter foodPriceParameter, int userId)
        {
            var foodPrice = _foodPriceRepositories.GetFoodPrice(foodPriceParameter.Id);
            if (foodPrice != null)
            {
                foodPrice.FoodId = foodPriceParameter.FoodId;
                foodPrice.Price = foodPriceParameter.Price;
                foodPrice.StartDate = foodPriceParameter.StartDate ?? DateTime.Now;
                foodPrice.UpdatedBy = userId;
                foodPrice.UpdatedAt = DateTime.Now;
                return FoodPriceDto.ToFoodPriceDto(_foodPriceRepositories.UpdateFoodPrice(foodPrice));
            }

            return null;
        }
    }
}
