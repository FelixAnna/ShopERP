using FoodShop.Manager.Entities.FoodShop;
using System;

namespace FoodShop.Manager.Services.Contracts.Dtos
{
    public class FoodPriceDto : BaseDto
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        public float Price { get; set; }

        public DateTime StartDate { get; set; }

        public static FoodPriceDto ToFoodPriceDto(FoodPrice model)
        {
            if (model == null)
            {
                return null;
            }

            return new FoodPriceDto()
            {
                Id = model.Id,
                FoodId = model.FoodId,
                Price = model.Price,
                StartDate = model.StartDate,
                IsDeleted = model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedAt = model.CreatedAt,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt
            };
        }
    }
}
