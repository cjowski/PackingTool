using System;
using System.Collections.Generic;

namespace PackingTool.PostgreSqlDatabase.DbModels
{
    public partial class List
    {
        public List()
        {
            UserLists = new HashSet<UserList>();
        }

        public int ListId { get; set; }
        public string ListName { get; set; } = null!;
        public string ListContent { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }

        public virtual ICollection<UserList> UserLists { get; set; }
    }
}
