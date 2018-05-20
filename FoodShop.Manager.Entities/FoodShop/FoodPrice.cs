using System;

namespace FoodShop.Manager.Entities.FoodShop
{
    public class FoodPrice : BaseEntity
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        public float Price { get; set; }

        public DateTime StartDate { get; set; }

        public Food Food { get; set; }
    }
}
