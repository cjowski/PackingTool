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
        public bool RequiredNewPassword { get; }
        [Required]
        public string Message { get; }

        private AuthenticateResponse(
            bool success,
            int userID,
            string token,
            UserRole[] roles,
            bool requiredNewPassword,
            string message
        )
        {
            Success = success;
            UserID = userID;
            Token = token;
            Roles = roles;
            RequiredNewPassword = requiredNewPassword;
            Message = message;
        }

        public static AuthenticateResponse Succeed(
            int userID,
            UserRole[] roles,
            string token,
            bool requiredNewPassword
        )
        {
            return new AuthenticateResponse(
                success: true,
                userID: userID,
                token: token,
                roles: roles,
                requiredNewPassword: requiredNewPassword,
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
                requiredNewPassword: false,
                message: message
            );
        }
    }
}
