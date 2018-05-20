using AutoMapper;
using FoodShop.Manager.Entities.FoodShop;
using FoodShop.Manager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodShop.Manager.DataAccess.Services
{
    public class OrderRepositories : IOrderRepositories
    {
        private IUnitOfWork<ShopDBContext> _unitOfWork;
        public OrderRepositories(IUnitOfWork<ShopDBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Order AddOrder(Order order)
        {
            _unitOfWork.DbContext.Orders.Add(order);
            _unitOfWork.DbContext.SaveChanges();
            return order;
        }

        public Order UpdateOrder(Order order)
        {
            var old = _unitOfWork.DbContext.Orders.First(x => x.Id == order.Id);
            Mapper.Map(order, old);
            _unitOfWork.DbContext.SaveChanges();
            return order;
        }

        public bool RemoveOrder(int Id)
        {
            var order = _unitOfWork.DbContext.Orders.FirstOrDefault(x => x.Id == Id);
            if (order == null)
            {
                return false;
            }

            order.IsDeleted = true;
            _unitOfWork.DbContext.SaveChanges();
            return true;
        }

        public Order GetOrder(int Id)
        {
            var order = _unitOfWork.DbContext.Orders.FirstOrDefault(x => x.Id == Id);
            return order;
        }

        public IList<Order> SearchOrder(int foodId)
        {
            var orders = _unitOfWork.DbContext.Orders
                .Where(x => x.PrimaryFoodId == foodId)
                .Where(x => x.IsDeleted == false);
            return orders.ToArray();
        }
    }
}
