using System;
using System.Collections.Generic;

namespace PackingTool.Database.DbModels;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int ModifiedUserId { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
