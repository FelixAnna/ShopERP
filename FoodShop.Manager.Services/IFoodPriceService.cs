using System.Collections.Generic;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;

namespace FoodShop.Manager.Services
{
    public interface IFoodPriceService
    {
        FoodPriceDto AddFoodPrice(FoodPriceParameter foodPriceParameter, int userId);
        FoodPriceDto GetFoodPrice(int Id);
        bool RemoveFoodPrice(int Id, int userId);
        FoodPriceDto GetCurrentFoodPrice(int foodId);
        IList<FoodPriceDto> SearchFoodPrice(int foodId);
        FoodPriceDto UpdateFoodPrice(FoodPriceParameter foodPriceParameter, int userId);
    }
}