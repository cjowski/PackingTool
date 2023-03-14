using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Output
{
    public class AuthenticateResponse
    {
        [Required]
        public bool Success { get; }
        [Required]
        public int UserID { get; }
        [Required]
        public string Token { get; }
        [Required]
        public string Message { get; }

        private AuthenticateResponse(
            bool success,
            int userID,
            string token,
            string message
        )
        {
            Success = success;
            UserID = userID;
            Token = token;
            Message = message;
        }

        public static AuthenticateResponse Succeed(
            int userID,
            string token    
        )
        {
            return new AuthenticateResponse(
                success: true,
                userID: userID,
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
                userID: -1,
                token: string.Empty,
                message: message
            );
        }
    }
}
