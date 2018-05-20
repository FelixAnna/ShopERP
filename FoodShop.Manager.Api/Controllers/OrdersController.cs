using FoodShop.Manager.Common;
using FoodShop.Manager.Services;
using FoodShop.Manager.Services.Contracts.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodShop.Manager.Api.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public ActionResult GetOrder([FromRoute]int id)
        {
            var order = _orderService.GetOrder(id);
            if (order != null)
            {
                return Ok(order);
            }

            return NotFound();
        }

        [HttpPost("")]
        public ActionResult AddOrder([FromBody]OrderParameter orderParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var order = _orderService.AddOrder(orderParameter, userId);
            if (order != null)
            {
                return Ok(order);
            }

            return StatusCode(500);
        }

        [HttpDelete("{Id}")]
        public ActionResult RemoveOrder(int Id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            if (_orderService.RemoveOrder(Id, userId))
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("search")]
        public ActionResult SearchOrder(int foodId)
        {
            var orders = _orderService.SearchOrder(foodId);
            if (orders.Any())
            {
                return Ok(orders);
            }

            return NotFound();
        }

        [HttpPost("update")]
        public ActionResult UpdateOrder([FromBody]OrderParameter orderParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var order = _orderService.UpdateOrder(orderParameter, userId);
            if (order != null)
            {
                return Ok(order);
            }

            return StatusCode(500);
        }
    }
}
