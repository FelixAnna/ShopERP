using System.Collections.Generic;

namespace FoodShop.Manager.Entities.FoodShop
{
    public class Food : BaseEntity
    {
        public int Id { get; set; }

        public string FoodName { get; set; }

        public string Remark { get; set; }

        public List<FoodPrice> FoodPrices { get; set; }
    }
}
