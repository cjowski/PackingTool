using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PackingTool.WebAPI.Attributes
{
    public class UserTokenAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var token = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var tokenService = context.HttpContext.RequestServices
                .GetRequiredService<Core.Service.User.ITokenService>();

            var userID = tokenService.ValidateToken(token!);

            if (userID == 0)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
