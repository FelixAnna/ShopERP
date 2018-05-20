using System.ComponentModel.DataAnnotations;

namespace FoodShop.Manager.Services.Contracts.Parameters
{
    public class FoodParameter
    {
        public int Id { get; set; }

        [Required]
        public string FoodName { get; set; }

        public string Remark { get; set; }
    }
}
