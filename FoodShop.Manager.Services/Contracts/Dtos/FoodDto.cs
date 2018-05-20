using FoodShop.Manager.Entities.FoodShop;
using System;

namespace FoodShop.Manager.Services.Contracts.Dtos
{
    public class FoodDto : BaseDto
    {
        public int Id { get; set; }

        public string FoodName { get; set; }

        public string Remark { get; set; }

        public static FoodDto ToFoodDto(Food model)
        {
            if (model == null)
            {
                return null;
            }

            return new FoodDto()
            {
                Id = model.Id,
                FoodName = model.FoodName,
                Remark = model.Remark,
                IsDeleted=model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedAt = model.CreatedAt,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
}
