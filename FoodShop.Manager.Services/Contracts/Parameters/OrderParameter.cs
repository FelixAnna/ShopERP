using System.ComponentModel.DataAnnotations;

namespace FoodShop.Manager.Services.Contracts.Parameters
{
    public class OrderParameter
    {
        public int Id { get; set; }

        [Required]
        public int PrimaryFoodId { get; set; }

        /// <summary>
        /// 配菜+打包+配送等费用
        /// </summary>
        public string AdditioalFoodIds { get; set; }

        /// <summary>
        /// 优惠券
        /// </summary>
        [Required]
        public float Reduced { get; set; }
    }
}
