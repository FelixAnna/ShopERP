using System.Collections.Generic;
using FoodShop.Manager.Entities.FoodShop;

namespace FoodShop.Manager.Services.Interfaces
{
    public interface IUserRepositories
    {
        User AddUser(User user);
        User GetUser(int Id);
        bool RemoveUser(int Id);
        IList<User> SearchUser(string keywords);
        User UpdateUser(User user);
    }
}