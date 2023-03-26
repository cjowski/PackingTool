using System;
using System.Collections.Generic;

namespace PackingTool.Database.DbModels
{
    public partial class UserPackingList
    {
        public int UserPackingListId { get; set; }
        public int UserId { get; set; }
        public int PackingListId { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUserId { get; set; }

        public virtual PackingList PackingList { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
