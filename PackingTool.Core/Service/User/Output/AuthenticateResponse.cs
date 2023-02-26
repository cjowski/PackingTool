namespace PackingTool.Core.Service.User.Output
{
    public class AuthenticateResponse
    {
        public bool Success { get; }
        public string Token { get; }
        public string Message { get; }

        private AuthenticateResponse(
            bool success,
            string token,
            string message
        )
        {
            Success = success;
            Token = token;
            Message = message;
        }

        public static AuthenticateResponse Succeed(
            string token    
        )
        {
            return new AuthenticateResponse(
                success: true,
                token: token,
                message: string.Empty
            );
        }

        public static AuthenticateResponse Failed(
            string message
        )
        {
            return new AuthenticateResponse(
                success: false,
                token: string.Empty,
                message: message
            );
        }
    }
}
