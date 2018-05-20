using FoodShop.Manager.Common;
using FoodShop.Manager.Services;
using FoodShop.Manager.Services.Contracts.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodShop.Manager.Api.Controllers
{
    [Route("api/foods")]
    public class FoodsController : Controller
    {
        private IFoodService _foodService;

        public FoodsController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet("{id}")]
        public ActionResult GetFood([FromRoute]int id)
        {
            var food = _foodService.GetFood(id);
            if (food != null)
            {
                return Ok(food);
            }

            return NotFound();
        }

        [HttpPost("")]
        public ActionResult AddFood([FromBody]FoodParameter foodParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var food = _foodService.AddFood(foodParameter, userId);
            if (food != null)
            {
                return Ok(food);
            }

            return StatusCode(500);
        }

        [HttpDelete("{Id}")]
        public ActionResult RemoveFood(int Id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            if (_foodService.RemoveFood(Id, userId))
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("search")]
        public ActionResult SearchFood(string keywords)
        {
            var foods = _foodService.SearchFood(keywords);
            if (foods.Any())
            {
                return Ok(foods);
            }

            return NotFound();
        }

        [HttpPost("update")]
        public ActionResult UpdateFood([FromBody]FoodParameter foodParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var food = _foodService.UpdateFood(foodParameter, userId);
            if (food != null)
            {
                return Ok(food);
            }

            return StatusCode(500);
        }
    }
}
