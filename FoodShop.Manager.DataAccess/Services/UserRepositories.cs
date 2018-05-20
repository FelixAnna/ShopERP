using AutoMapper;
using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.DataAccess.Services
{
    public class UserRepositories : IUserRepositories
    {
        private IUnitOfWork<ShopDBContext> _unitOfWork;
        public UserRepositories(IUnitOfWork<ShopDBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User AddUser(User user)
        {
            _unitOfWork.DbContext.Users.Add(user);
            _unitOfWork.DbContext.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            var old = _unitOfWork.DbContext.Users.First(x => x.Id == user.Id);
            Mapper.Map(user, old);
            _unitOfWork.DbContext.SaveChanges();
            return user;
        }

        public bool RemoveUser(int Id)
        {
            var user = _unitOfWork.DbContext.Users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;
            _unitOfWork.DbContext.SaveChanges();
            return true;
        }

        public User GetUser(int Id)
        {
            var user = _unitOfWork.DbContext.Users.FirstOrDefault(x => x.Id == Id);
            return user;
        }

        public IList<User> SearchUser(string keywords)
        {
            var users = _unitOfWork.DbContext.Users
                .Where(x => x.UserName.ToUpperInvariant().Contains(keywords.ToUpper()))
                .Where(x => x.IsDeleted == false);
            return users.ToArray();
        }
    }
}
