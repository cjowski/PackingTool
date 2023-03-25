using System;
using System.Collections.Generic;

namespace PackingTool.PostgreSqlDatabase.DbModels
{
    public partial class UserList
    {
        public int UserListId { get; set; }
        public int UserId { get; set; }
        public int ListId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }

        public virtual List List { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
