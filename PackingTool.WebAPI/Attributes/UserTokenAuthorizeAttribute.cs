using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PackingTool.WebAPI.Attributes
{
    public class UserTokenAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                {
                    return;
                }
            
                if (ValidateToken(context))
                {
                    return;
                }

                //TODO think over better solution
                if (!ValidateUser(context))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private static bool ValidateToken(
            AuthorizationFilterContext context
        )
        {
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var tokenService = context.HttpContext.RequestServices
                .GetRequiredService<Core.Service.User.ITokenService>();

            var userID = tokenService.ValidateToken(authorizationHeader!);
            return userID != 0;
        }

        private static bool ValidateUser(
            AuthorizationFilterContext context
        )
        {
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var credentials = System.Text.Encoding.UTF8
                .GetString(Convert.FromBase64String(authorizationHeader!))
                .Split(':');

            var userService = context.HttpContext.RequestServices
                .GetRequiredService<Core.Service.User.IUserService>();

            var response = userService.Authenticate(
                new Core.Service.User.Input.AuthenticateUser(
                    userName: credentials[0],
                    password: credentials[1]
                )
            );

            response.Wait();
            return response.Result.Success;
        }
    }
}
