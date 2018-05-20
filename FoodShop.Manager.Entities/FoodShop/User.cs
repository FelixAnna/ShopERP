using FoodShop.Manager.Entities.FoodShop;

namespace FoodShop.Manager.Entities.FoodShop
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
