using FoodShop.Manager.Common;
using FoodShop.Manager.Services;
using FoodShop.Manager.Services.Contracts.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodShop.Manager.Api.Controllers
{
    [Route("api/food-prices")]
    public class FoodPricesController : Controller
    {
        private IFoodPriceService _foodPriceService;

        public FoodPricesController(IFoodPriceService foodPriceService)
        {
            _foodPriceService = foodPriceService;
        }

        [HttpGet("{id}")]
        public ActionResult GetFoodPrice([FromRoute]int id)
        {
            var foodPrice = _foodPriceService.GetFoodPrice(id);
            if (foodPrice != null)
            {
                return Ok(foodPrice);
            }

            return NotFound();
        }

        [HttpPost("")]
        public ActionResult AddFoodPrice([FromBody]FoodPriceParameter foodPriceParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var foodPrice = _foodPriceService.AddFoodPrice(foodPriceParameter, userId);
            if (foodPrice != null)
            {
                return Ok(foodPrice);
            }

            return StatusCode(500);
        }

        [HttpDelete("{Id}")]
        public ActionResult RemoveFoodPrice(int Id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            if (_foodPriceService.RemoveFoodPrice(Id, userId))
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("food/{foodId}")]
        public ActionResult GetCurrentFoodPrice(int foodId)
        {
            var foodPrice = _foodPriceService.GetCurrentFoodPrice(foodId);
            if (foodPrice != null)
            {
                return Ok(foodPrice);
            }

            return NotFound();
        }

        [HttpGet("search")]
        public ActionResult SearchFoodPrice(int foodId)
        {
            var foodPrices = _foodPriceService.SearchFoodPrice(foodId);
            if (foodPrices.Any())
            {
                return Ok(foodPrices);
            }

            return NotFound();
        }

        [HttpPost("update")]
        public ActionResult UpdateFoodPrice([FromBody]FoodPriceParameter foodPriceParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var foodPrice = _foodPriceService.UpdateFoodPrice(foodPriceParameter, userId);
            if (foodPrice != null)
            {
                return Ok(foodPrice);
            }

            return StatusCode(500);
        }
    }
}
