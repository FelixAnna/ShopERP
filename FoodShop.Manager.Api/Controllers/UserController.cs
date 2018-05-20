using FoodShop.Manager.Common;
using FoodShop.Manager.Services;
using FoodShop.Manager.Services.Contracts.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodShop.Manager.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult GetUser([FromRoute]int id)
        {
            var user = _userService.GetUser(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost("")]
        public ActionResult AddUser([FromBody]UserParameter userParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var user = _userService.AddUser(userParameter, userId);
            if (user != null)
            {
                return Ok(user);
            }

            return StatusCode(500);
        }

        [HttpDelete("{Id}")]
        public ActionResult RemoveUser(int Id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            if (_userService.RemoveUser(Id, userId))
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("search")]
        public ActionResult SearchUser(string keywords)
        {
            var users = _userService.SearchUser(keywords);
            if (users.Any())
            {
                return Ok(users);
            }

            return NotFound();
        }

        [HttpPost("update")]
        public ActionResult UpdateUser([FromBody]UserParameter foodParameter)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == WsConstants.UserIdClaim)?.Value ?? "-1");
            var user = _userService.UpdateUser(foodParameter, userId);
            if (user != null)
            {
                return Ok(user);
            }

            return StatusCode(500);
        }
    }
}
