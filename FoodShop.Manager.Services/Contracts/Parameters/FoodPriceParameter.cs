using System;
using System.ComponentModel.DataAnnotations;

namespace FoodShop.Manager.Services.Contracts.Parameters
{
    public class FoodPriceParameter
    {
        public int Id { get; set; }

        [Required]
        public int FoodId { get; set; }

        [Required]
        public float Price { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
