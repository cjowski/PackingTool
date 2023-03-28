using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Input
{
    public class ChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public ChangePassword(
            string currentPassword,
            string newPassword
        )
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}
