using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Output
{
    public class UserDetails
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public bool Authorized { get; set; }

        public UserDetails(
            int userID,
            string userName,
            bool authorized
        )
        {
            UserID = userID;
            UserName = userName;
            Authorized = authorized;
        }
    }
}
