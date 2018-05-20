using FoodShop.Manager.Entities.FoodShop;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.Manager.DataAccess
{
    public class ShopDBContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodPrice> FoodPrices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        public ShopDBContext(DbContextOptions<ShopDBContext> options) : base(options)
        { }
    }
}
