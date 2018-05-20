using System.Collections.Generic;
using FoodShop.Manager.Entities.FoodShop;

namespace FoodShop.Manager.Services.Interfaces
{
    public interface IOrderRepositories
    {
        Order AddOrder(Order order);
        Order GetOrder(int Id);
        bool RemoveOrder(int Id);
        IList<Order> SearchOrder(int foodId);
        Order UpdateOrder(Order order);
    }
}