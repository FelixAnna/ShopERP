using System;

namespace FoodShop.Manager.Entities.FoodShop
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }

        public int PrimaryFoodId { get; set; }

        /// <summary>
        /// 配菜+打包+配送等费用
        /// </summary>
        public string AdditioalFoodIds { get; set; }

        public string OrderName { get; set; }

        /// <summary>
        /// 优惠券
        /// </summary>
        public float Reduced { get; set; }

        public float Price { get; set; }
    }
}
