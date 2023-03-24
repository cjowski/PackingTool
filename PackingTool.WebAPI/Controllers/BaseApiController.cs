using Microsoft.AspNetCore.Mvc;

namespace PackingTool.WebAPI.Controllers
{
    [ApiController]
    [Attributes.UserTokenAuthorize]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase {

        private Core.Service.User.ITokenService _tokenService { get; }

        public BaseApiController(
            Core.Service.User.ITokenService tokenService
        )
        {
            _tokenService = tokenService;
        }

        protected int GetRequestedUserID()
        {
            var authorizationHeader = Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var userID = _tokenService.ValidateToken(authorizationHeader!);

            if (userID == 0)
            {
                throw new HttpRequestException(
                    $"Unable to get requested userID"
                );
            }

            return userID;
        }
    }
}
