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
        public UserRole[] Roles { get; }
        [Required]
        public string Message { get; }

        private AuthenticateResponse(
            bool success,
            int userID,
            string token,
            UserRole[] roles,
            string message
        )
        {
            Success = success;
            UserID = userID;
            Token = token;
            Roles = roles;
            Message = message;
        }

        public static AuthenticateResponse Succeed(
            int userID,
            UserRole[] roles,
            string token    
        )
        {
            return new AuthenticateResponse(
                success: true,
                userID: userID,
                token: token,
                roles: roles,
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
                roles: Array.Empty<UserRole>(),
                message: message
            );
        }
    }
}
