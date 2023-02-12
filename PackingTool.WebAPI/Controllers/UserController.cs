using Microsoft.AspNetCore.Mvc;
using CoreService = PackingTool.Core.Service.User;

namespace PackingTool.WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private CoreService.IUserService _userService { get; }

        public UserController(
            CoreService.IUserService userService
        )
        {
            _userService = userService;
        }

        [HttpPost("AddUser")]
        public async Task AddUser(
            [FromBody] CoreService.Input.User user
        )
        {
            await _userService.AddUser(user);
        }
    }
}
