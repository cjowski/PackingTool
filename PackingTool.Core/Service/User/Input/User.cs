using System.ComponentModel.DataAnnotations;

namespace PackingTool.Core.Service.User.Input
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
