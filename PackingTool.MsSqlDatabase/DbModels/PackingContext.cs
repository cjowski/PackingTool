﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PackingTool.MsSqlDatabase.DbModels
{
    public partial class PackingContext : DbContext
    {
        public PackingContext()
        {
        }

        public PackingContext(DbContextOptions<PackingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PackingList> PackingList { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPackingList> UserPackingList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackingList>(entity =>
            {
                entity.Property(e => e.PackingListId).HasColumnName("PackingListID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId).HasColumnName("CreatedUserID");

                entity.Property(e => e.Json)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("JSON");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId).HasColumnName("ModifiedUserID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId).HasColumnName("CreatedUserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId).HasColumnName("ModifiedUserID");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserPackingList>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.PackingListId }, "UK_UserPackingList")
                    .IsUnique();

                entity.Property(e => e.UserPackingListId).HasColumnName("UserPackingListID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId).HasColumnName("CreatedUserID");

                entity.Property(e => e.PackingListId).HasColumnName("PackingListID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.PackingList)
                    .WithMany(p => p.UserPackingList)
                    .HasForeignKey(d => d.PackingListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPackingList_PackingList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPackingList)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPackingList_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}