using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;
using FoodShop.Manager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepositories _foodRepositories;

        public FoodService(IFoodRepositories foodRepositories)
        {
            _foodRepositories = foodRepositories;
        }

        public FoodDto AddFood(FoodParameter foodParameter, int userId)
        {
            var newFood = new Food()
            {
                FoodName = foodParameter.FoodName,
                Remark = foodParameter.Remark,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };
            _foodRepositories.AddFood(newFood);
            return FoodDto.ToFoodDto(newFood);
        }

        public FoodDto GetFood(int Id)
        {
            var food = _foodRepositories.GetFood(Id);
            return FoodDto.ToFoodDto(food);
        }

        public bool RemoveFood(int Id, int userId)
        {
            var food = _foodRepositories.GetFood(Id);
            if (food != null)
            {
                food.IsDeleted = true;
                food.UpdatedBy = userId;
                food.UpdatedAt = DateTime.Now;
                _foodRepositories.UpdateFood(food);
                return true;
            }

            return false;
        }

        public IList<FoodDto> SearchFood(string keywords)
        {
            var foods = _foodRepositories.SearchFood(keywords);
            return foods.Select(FoodDto.ToFoodDto).ToList();
        }

        public FoodDto UpdateFood(FoodParameter foodParameter, int userId)
        {
            var food = _foodRepositories.GetFood(foodParameter.Id);
            if (food != null)
            {
                food.FoodName = foodParameter.FoodName;
                food.Remark = foodParameter.Remark;
                food.UpdatedBy = userId;
                food.UpdatedAt = DateTime.Now;
                return FoodDto.ToFoodDto(_foodRepositories.UpdateFood(food));
            }

            return null;
        }
    }
}
