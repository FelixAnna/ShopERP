using AutoMapper;
using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.DataAccess.Services
{
    public class FoodRepositories : IFoodRepositories
    {
        private IUnitOfWork<ShopDBContext> _unitOfWork;
        public FoodRepositories(IUnitOfWork<ShopDBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Food AddFood(Food food)
        {
            _unitOfWork.DbContext.Foods.Add(food);
            _unitOfWork.DbContext.SaveChanges();
            return food;
        }

        public Food UpdateFood(Food food)
        {
            var old = _unitOfWork.DbContext.Foods.First(x => x.Id == food.Id);
            old = Mapper.Map(food, old);
            _unitOfWork.DbContext.SaveChanges();
            return food;
        }

        public bool RemoveFood(int Id)
        {
            var food = _unitOfWork.DbContext.Foods.FirstOrDefault(x => x.Id == Id);
            if (food == null)
            {
                return false;
            }

            food.IsDeleted = true;
            _unitOfWork.DbContext.SaveChanges();
            return true;
        }

        public Food GetFood(int Id)
        {
            var food = _unitOfWork.DbContext.Foods.FirstOrDefault(x => x.Id == Id);
            return food;
        }

        public IList<Food> GetFood(IEnumerable<int> Ids)
        {
            var foods = _unitOfWork.DbContext.Foods.Where(x => Ids.Contains(x.Id));
            return foods.ToList();
        }

        public IList<Food> SearchFood(string keywords)
        {
            var foods = _unitOfWork.DbContext.Foods
                .Where(x => x.FoodName.ToUpperInvariant().Contains(keywords.ToUpper()) || x.Remark.ToUpperInvariant().Contains(keywords.ToUpper()))
                .Where(x => x.IsDeleted == false);
            return foods.ToArray();
        }
    }
}
