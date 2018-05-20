using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;
using FoodShop.Manager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositories _userRepositories;

        public UserService(IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
        }

        public UserDto AddUser(UserParameter userParameter, int userId)
        {
            var newUser = new User()
            {
                UserName = userParameter.UserName,
                Password = userParameter.Password,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };
            _userRepositories.AddUser(newUser);
            return UserDto.ToUserDto(newUser);
        }

        public UserDto GetUser(int Id)
        {
            var food = _userRepositories.GetUser(Id);
            return UserDto.ToUserDto(food);
        }

        public bool RemoveUser(int Id, int userId)
        {
            var user = _userRepositories.GetUser(Id);
            if (user != null)
            {
                user.IsDeleted = true;
                user.UpdatedBy = userId;
                user.UpdatedAt = DateTime.Now;
                _userRepositories.UpdateUser(user);
                return true;
            }

            return false;
        }

        public IList<UserDto> SearchUser(string keywords)
        {
            var users = _userRepositories.SearchUser(keywords);
            return users.Select(UserDto.ToUserDto).ToList();
        }

        public UserDto UpdateUser(UserParameter foodParameter, int userId)
        {
            var user = _userRepositories.GetUser(foodParameter.Id);
            if (user != null)
            {
                user.UserName = foodParameter.UserName;
                user.Password = foodParameter.Password;
                user.UpdatedBy = userId;
                user.UpdatedAt = DateTime.Now;
                return UserDto.ToUserDto(_userRepositories.UpdateUser(user));
            }

            return null;
        }
    }
}
