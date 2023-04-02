using System;
using System.Collections.Generic;

namespace PackingTool.Database.DbModels;

public partial class UserAuthorization
{
    public int UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool Authorized { get; set; }

    public bool RequiredNewPassword { get; set; }

    public DateTime LastLoginDate { get; set; }

    public int LoginAttemptsLeft { get; set; }

    public DateTime LastLoginAttemptDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int ModifiedUserId { get; set; }

    public virtual User User { get; set; } = null!;
}
