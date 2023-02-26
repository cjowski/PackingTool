using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Input
{
    public class AuthenticateUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public AuthenticateUser()
        {
            UserName = string.Empty;
            Password = string.Empty;
        }
    }
}
