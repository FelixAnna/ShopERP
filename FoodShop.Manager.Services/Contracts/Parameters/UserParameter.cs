using System.ComponentModel.DataAnnotations;

namespace FoodShop.Manager.Services.Contracts.Parameters
{
    public class UserParameter
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
