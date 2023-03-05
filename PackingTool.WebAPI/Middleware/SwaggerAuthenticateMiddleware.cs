using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace PackingTool.WebAPI.Middleware
{
    internal class SwaggerAuthenticateMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerAuthenticateMiddleware(
            RequestDelegate next
        )
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next.Invoke(context).ConfigureAwait(false);
                return;
            }

            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            var tokenService = context.RequestServices
                .GetRequiredService<Core.Service.User.ITokenService>();

            var userID = tokenService.ValidateToken(token!);

            if (userID == 0)
            {
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
        }
    }
}