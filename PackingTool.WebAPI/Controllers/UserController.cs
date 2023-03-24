using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService = PackingTool.Core.Service.User;

namespace PackingTool.WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private UserService.IUserService _userService { get; }

        public UserController(
            UserService.ITokenService tokenService,
            UserService.IUserService userService
        ) : base(tokenService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("userID")]
        public async Task<int> GetUserID(
            [FromQuery] string userName
        )
        {
            return await _userService.GetUserID(userName);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<UserService.Output.AuthenticateResponse> Authenticate(
            [FromBody] UserService.Input.AuthenticateUser user
        )
        {
            return await _userService.Authenticate(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<UserService.Output.UserResponse> Register(
            [FromBody] UserService.Input.RegisterUser user
        )
        {
            return await _userService.Register(user, GetRequestedUserID());
        }
    }
}
