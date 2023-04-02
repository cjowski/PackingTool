using System;
using System.Collections.Generic;

namespace PackingTool.Database.DbModels;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Deleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int ModifiedUserId { get; set; }

    public virtual UserAuthorization? UserAuthorization { get; set; }

    public virtual ICollection<UserPackingList> UserPackingLists { get; } = new List<UserPackingList>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
