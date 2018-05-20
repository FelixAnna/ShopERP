
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodShop.Manager.Services.Contracts.Dtos
{
    public class BaseDto
    {
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
