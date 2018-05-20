using FoodShop.Manager.Entities.FoodShop;
using System;

namespace FoodShop.Manager.Services.Contracts.Dtos
{
    public class UserDto : BaseDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public static UserDto ToUserDto(User model)
        {
            if (model == null)
            {
                return null;
            }

            return new UserDto()
            {
                Id = model.Id,
                UserName = model.UserName,
                Password = model.Password,
                IsDeleted=model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedAt = model.CreatedAt,
                UpdatedBy = model.UpdatedBy,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
}
