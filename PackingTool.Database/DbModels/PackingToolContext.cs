using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PackingTool.Database.DbModels;

public partial class PackingToolContext : DbContext
{
    public PackingToolContext(DbContextOptions<PackingToolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PackingList> PackingLists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAuthorization> UserAuthorizations { get; set; }

    public virtual DbSet<UserPackingList> UserPackingLists { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackingList>(entity =>
        {
            entity.HasKey(e => e.PackingListId).HasName("packing_list_pkey");

            entity.ToTable("packing_list", "packing");

            entity.Property(e => e.PackingListId).HasColumnName("packing_list_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.ListContent)
                .HasColumnType("json")
                .HasColumnName("list_content");
            entity.Property(e => e.ListName)
                .HasMaxLength(50)
                .HasColumnName("list_name");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role", "users");

            entity.HasIndex(e => e.RoleName, "role_role_name_key").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user", "users");

            entity.HasIndex(e => e.UserName, "user_user_name_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<UserAuthorization>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_authorization_pkey");

            entity.ToTable("user_authorization", "users");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Authorized).HasColumnName("authorized");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.LastLoginAttemptDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login_attempt_date");
            entity.Property(e => e.LastLoginDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login_date");
            entity.Property(e => e.LoginAttemptsLeft)
                .HasDefaultValueSql("10")
                .HasColumnName("login_attempts_left");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .HasColumnName("password_hash");
            entity.Property(e => e.RequiredNewPassword).HasColumnName("required_new_password");

            entity.HasOne(d => d.User).WithOne(p => p.UserAuthorization)
                .HasForeignKey<UserAuthorization>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_authorization_user_id_fkey");
        });

        modelBuilder.Entity<UserPackingList>(entity =>
        {
            entity.HasKey(e => e.UserPackingListId).HasName("user_packing_list_pkey");

            entity.ToTable("user_packing_list", "packing");

            entity.HasIndex(e => new { e.UserId, e.PackingListId }, "user_packing_list_user_id_packing_list_id_key").IsUnique();

            entity.Property(e => e.UserPackingListId).HasColumnName("user_packing_list_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");
            entity.Property(e => e.PackingListId).HasColumnName("packing_list_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PackingList).WithMany(p => p.UserPackingLists)
                .HasForeignKey(d => d.PackingListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_packing_list_packing_list_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserPackingLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_packing_list_user_id_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("user_role_pkey");

            entity.ToTable("user_role", "users");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "user_role_user_id_role_id_key").IsUnique();

            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
