using FoodShop.Manager.Entities.FoodShop;
using System;

namespace FoodShop.Manager.Services.Contracts.Dtos
{
    public class OrderDto : BaseDto
    {
        public int Id { get; set; }

        public int PrimaryFoodId { get; set; }

        /// <summary>
        /// 配菜+打包+配送等费用
        /// </summary>
        public string AdditioalFoodIds { get; set; }

        public string OrderName { get; set; }

        /// <summary>
        /// 优惠券
        /// </summary>
        public float Reduced { get; set; }

        public float Price { get; set; }

        public static OrderDto ToOrderDto(Order model)
        {
            if (model == null)
            {
                return null;
            }

            return new OrderDto()
            {
                Id = model.Id,
                PrimaryFoodId = model.PrimaryFoodId,
                AdditioalFoodIds = model.AdditioalFoodIds,
                OrderName = model.OrderName,
                Reduced = model.Reduced,
                Price = model.Price,
                IsDeleted = model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedAt = model.CreatedAt,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
}
