using System;
using System.Collections.Generic;

namespace PackingTool.PostgreSqlDatabase.DbModels
{
    public partial class User
    {
        public User()
        {
            UserLists = new HashSet<UserList>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Authorized { get; set; }
        public bool Deleted { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }

        public virtual ICollection<UserList> UserLists { get; set; }
    }
}
