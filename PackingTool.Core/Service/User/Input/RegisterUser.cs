using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Input
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public RegisterUser()
        {
            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
        }
    }
}
