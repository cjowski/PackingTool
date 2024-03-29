﻿using Microsoft.AspNetCore.Authorization;
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
            return await _userService.Register(user);
        }

        [HttpPost("changePassword")]
        public async Task<UserService.Output.UserResponse> ChangePassword(
            [FromBody] UserService.Input.ChangePassword changePassword
        )
        {
            var requestedUserID = GetRequestedUserID();
            return await _userService.ChangePassword(changePassword, requestedUserID, requestedUserID);
        }

        //TODO admin role!
        [HttpGet("searchUsers")]
        public async Task<UserService.Output.UserDetails[]> SearchUsers(
            string searchingPhrase
        )
        {
            return await _userService.SearchUsers(searchingPhrase);
        }

        //TODO admin role!
        [HttpPost("authorizeUser")]
        public async Task AuthorizeUser(
            int userID,
            bool authorized
        )
        {
            await _userService.AuthorizeUser(userID, authorized, GetRequestedUserID());
        }

        //TODO admin role!
        [HttpPost("setTemporaryPassword")]
        public async Task SetTemporaryPassword(
            [FromBody] UserService.Input.SetTemporaryPassword setTemporaryPassword
        )
        {
            await _userService.SetTemporaryPassword(setTemporaryPassword, GetRequestedUserID());
        }
    }
}
