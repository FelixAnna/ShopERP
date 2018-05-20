using System.Collections.Generic;
using FoodShop.Manager.Entities.FoodShop;

namespace FoodShop.Manager.Services.Interfaces
{
    public interface IFoodRepositories
    {
        Food AddFood(Food food);
        Food GetFood(int Id);
        IList<Food> GetFood(IEnumerable<int> Ids);
        bool RemoveFood(int Id);
        IList<Food> SearchFood(string keywords);
        Food UpdateFood(Food food);
    }
}