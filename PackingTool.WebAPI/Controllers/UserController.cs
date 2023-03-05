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

        [HttpPost("authenticate")]
        public async Task<CoreService.Output.AuthenticateResponse> Authenticate(
            [FromBody] CoreService.Input.AuthenticateUser user
        )
        {
            return await _userService.Authenticate(user);
        }

        [HttpPost("register")]
        public async Task<CoreService.Output.UserResponse> Register(
            [FromBody] CoreService.Input.RegisterUser user
        )
        {
            return await _userService.Register(user);
        }
    }
}
