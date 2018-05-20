using FoodShop.Manager.Entities.FoodShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodShop.Manager.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(ShopDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{UserName="Admin",Password="147147"},
                new User{UserName="Lily",Password="123456"},
                new User{UserName="Lucy",Password="345678"}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
