using AutoMapper;
using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.DataAccess.Services
{
    public class FoodPriceRepositories : IFoodPriceRepositories
    {
        private IUnitOfWork<ShopDBContext> _unitOfWork;
        public FoodPriceRepositories(IUnitOfWork<ShopDBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FoodPrice AddFoodPrice(FoodPrice foodPrice)
        {
            _unitOfWork.DbContext.FoodPrices.Add(foodPrice);
            _unitOfWork.DbContext.SaveChanges();
            return foodPrice;
        }

        public FoodPrice UpdateFoodPrice(FoodPrice foodPrice)
        {
            var old = _unitOfWork.DbContext.FoodPrices.First(x => x.Id == foodPrice.Id);
            Mapper.Map(foodPrice, old);
            _unitOfWork.DbContext.SaveChanges();
            return foodPrice;
        }

        public bool RemoveFoodPrice(int Id)
        {
            var foodPrice = _unitOfWork.DbContext.FoodPrices.FirstOrDefault(x => x.Id == Id);
            if (foodPrice == null)
            {
                return false;
            }

            foodPrice.IsDeleted = true;
            _unitOfWork.DbContext.SaveChanges();
            return true;
        }

        public FoodPrice GetFoodPrice(int Id)
        {
            var foodPrice = _unitOfWork.DbContext.FoodPrices.FirstOrDefault(x => x.Id == Id);
            return foodPrice;
        }

        public FoodPrice GetCurrentFoodPrice(int foodId)
        {
            var foodPrice = _unitOfWork.DbContext.FoodPrices
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault(x => x.FoodId == foodId && x.IsDeleted == false);
            return foodPrice;
        }

        public IList<FoodPrice> SearchFoodPrice(int foodId)
        {
            var foodPrices = _unitOfWork.DbContext.FoodPrices
                .Where(x => x.FoodId == foodId)
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedAt);
            return foodPrices.ToArray();
        }
    }
}
