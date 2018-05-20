using System;

namespace FoodShop.Manager.Entities.FoodShop
{
    public class BaseEntity
    {
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
