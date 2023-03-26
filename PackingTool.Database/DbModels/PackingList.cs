using System;
using System.Collections.Generic;

namespace PackingTool.Database.DbModels
{
    public partial class PackingList
    {
        public PackingList()
        {
            UserPackingLists = new HashSet<UserPackingList>();
        }

        public int PackingListId { get; set; }
        public string ListName { get; set; } = null!;
        public string ListContent { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }

        public virtual ICollection<UserPackingList> UserPackingLists { get; set; }
    }
}
