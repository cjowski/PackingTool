using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Input
{
    public class SetTemporaryPassword
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string Password { get; set; }

        public SetTemporaryPassword(
            int userID,
            string password
        )
        {
            UserID = userID;
            Password = password;
        }
    }
}
