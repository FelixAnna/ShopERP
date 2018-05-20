using System.Collections.Generic;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;

namespace FoodShop.Manager.Services
{
    public interface IFoodService
    {
        FoodDto AddFood(FoodParameter foodParameter, int userId);
        FoodDto GetFood(int Id);
        bool RemoveFood(int Id, int userId);
        IList<FoodDto> SearchFood(string keywords);
        FoodDto UpdateFood(FoodParameter foodParameter, int userId);
    }
}