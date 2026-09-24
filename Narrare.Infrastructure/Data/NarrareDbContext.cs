using Microsoft.EntityFrameworkCore;
using Narrare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Narrare.Infrastructure.Data;

public class NarrareDbContext : DbContext
{
    public NarrareDbContext(DbContextOptions<NarrareDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(user => user.PasswordHash)
                .IsRequired();

            entity.Property(user => user.Location)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(user => user.Occupation)
                .HasMaxLength(100);

            entity.Property(user => user.ProfileImageUrl)
                .HasMaxLength(500);

            entity.HasIndex(user => user.Username)
                .IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(category => category.Id);

            entity.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(category => category.Description)
                .HasMaxLength(500);
        });
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(menuItem => menuItem.Id);

            entity.Property(menuItem => menuItem.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(menuItem => menuItem.Url)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(menuItem => menuItem.SortOrder)
                .IsRequired();
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(post => post.Id);

            entity.Property(post => post.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(post => post.Content)
                .IsRequired();

            entity.HasOne(post => post.User)
                .WithMany(user => user.Posts)
                .HasForeignKey(post => post.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(post => post.Category)
                .WithMany(category => category.Posts)
                .HasForeignKey(post => post.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(comment => comment.Id);

            entity.Property(comment => comment.Content)
                .IsRequired();

            entity.HasOne(comment => comment.User)
                .WithMany()
                .HasForeignKey(comment => comment.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(comment => comment.Post)
                .WithMany(post => post.Comments)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}