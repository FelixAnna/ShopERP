using System.Collections.Generic;
using FoodShop.Manager.Entities.FoodShop;

namespace FoodShop.Manager.Services.Interfaces
{
    public interface IFoodPriceRepositories
    {
        FoodPrice AddFoodPrice(FoodPrice foodPrice);
        FoodPrice GetFoodPrice(int Id);
        bool RemoveFoodPrice(int Id);
        FoodPrice GetCurrentFoodPrice(int foodId);
        IList<FoodPrice> SearchFoodPrice(int foodId);
        FoodPrice UpdateFoodPrice(FoodPrice foodPrice);
    }
}