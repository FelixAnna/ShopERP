using System.Collections.Generic;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;

namespace FoodShop.Manager.Services
{
    public interface IOrderService
    {
        OrderDto AddOrder(OrderParameter orderParameter, int userId);
        OrderDto GetOrder(int Id);
        bool RemoveOrder(int Id, int userId);
        IList<OrderDto> SearchOrder(int foodId);
        OrderDto UpdateOrder(OrderParameter orderParameter, int userId);
    }
}