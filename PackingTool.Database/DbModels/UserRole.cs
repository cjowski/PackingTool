using System;
using System.Collections.Generic;

namespace PackingTool.Database.DbModels;

public partial class UserRole
{
    public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
