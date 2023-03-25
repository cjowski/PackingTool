using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PackingTool.PostgreSqlDatabase.DbModels
{
    public partial class PackingToolContext : DbContext
    {
        public PackingToolContext()
        {
        }

        public PackingToolContext(DbContextOptions<PackingToolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<List> Lists { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserList> UserLists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<List>(entity =>
            {
                entity.ToTable("list", "packing");

                entity.Property(e => e.ListId).HasColumnName("list_id");

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "users");

                entity.HasIndex(e => e.UserName, "user_user_name_key")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Authorized).HasColumnName("authorized");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_date");

                entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("last_login_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modified_date");

                entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(200)
                    .HasColumnName("password_hash");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<UserList>(entity =>
            {
                entity.ToTable("user_list", "packing");

                entity.HasIndex(e => new { e.UserId, e.ListId }, "user_list_user_id_list_id_key")
                    .IsUnique();

                entity.Property(e => e.UserListId).HasColumnName("user_list_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_date");

                entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modified_date");

                entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.UserLists)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_list_list_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_list_user_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
