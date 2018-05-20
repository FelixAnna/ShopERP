using System.Collections.Generic;
using FoodShop.Manager.Services.Contracts.Dtos;
using FoodShop.Manager.Services.Contracts.Parameters;

namespace FoodShop.Manager.Services
{
    public interface IUserService
    {
        UserDto AddUser(UserParameter userParameter, int userId);
        UserDto GetUser(int Id);
        bool RemoveUser(int Id, int userId);
        IList<UserDto> SearchUser(string keywords);
        UserDto UpdateUser(UserParameter foodParameter, int userId);
    }
}