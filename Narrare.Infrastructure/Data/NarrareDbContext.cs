using Microsoft.EntityFrameworkCore;
using Narrare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
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
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<PageContent> PageContents { get; set; }
    public DbSet<SiteStyle> SiteStyles { get; set; }
    public DbSet<UploadedImage> UploadedImages => Set<UploadedImage>();
    public DbSet<ImageUsage> ImageUsages => Set<ImageUsage>();
    public DbSet<PrivateMessage> PrivateMessages => Set<PrivateMessage>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants
    => Set<ConversationParticipant>();
    public DbSet<GroupMessageReadStatus> GroupMessageReadStatuses
    => Set<GroupMessageReadStatus>();
    public DbSet<ContentReport> ContentReports { get; set; }
    public DbSet<Like> Likes { get; set; }



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

            entity.HasData(
                new Category
                {
                    Id = 3,
                    Name = "Kök",
                    Description = "Diskussioner om kök, matlagning och köksinredning."
                },
                new Category
                {
                    Id = 4,
                    Name = "Sovrum",
                    Description = "Diskussioner om sovrum, sängar och förvaring."
                },
                new Category
                {
                    Id = 5,
                    Name = "Badrum",
                    Description = "Diskussioner om badrum, renovering och inredning."
                },
                new Category
                {
                    Id = 6,
                    Name = "Trädgård",
                    Description = "Diskussioner om trädgård, uteplatser och växter."
                }
            );
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

            entity.HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Hem",
                    Url = "/",
                    SortOrder = 1
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Kategorier",
                    Url = "/categories",
                    SortOrder = 2
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Om Narrare",
                    Url = "/pages/about",
                    SortOrder = 3
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Logga in",
                    Url = "/login",
                    SortOrder = 4
                }
            );
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

        modelBuilder.Entity<PrivateMessage>(entity =>
        {
            entity.HasKey(message => message.Id);

            entity.Property(message => message.Content)
                .IsRequired()
                .HasMaxLength(5000);

            entity.Property(message => message.SentAt)
                .IsRequired();

            entity.Property(message => message.IsRead)
                .IsRequired();

            entity.HasOne(message => message.Sender)
                .WithMany()
                .HasForeignKey(message => message.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(message => message.Receiver)
                .WithMany()
                .HasForeignKey(message => message.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(message => message.SenderId);
            entity.HasIndex(message => message.ReceiverId);

            entity.HasOne(message => message.Conversation)
           .WithMany(conversation => conversation.Messages)
               .HasForeignKey(message => message.ConversationId)
               .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(message => message.ConversationId);
        });
        
        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(page => page.Id);

            entity.Property(page => page.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(page => page.Slug)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(page => page.Content)
                .IsRequired();

            entity.HasIndex(page => page.Slug)
                .IsUnique();
        });
        modelBuilder.Entity<UploadedImage>(entity =>
        {
            entity.HasKey(image => image.Id);

            entity.Property(image => image.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(image => image.StoragePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(image => image.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(image => image.FileSize)
                .IsRequired();

            entity.Property(image => image.UploadedAt)
                .IsRequired();

            entity.HasOne(image => image.UploadedByUser)
                .WithMany()
                .HasForeignKey(image => image.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ImageUsage>(entity =>
        {
            entity.HasKey(usage => usage.Id);

            entity.HasOne(usage => usage.UploadedImage)
                .WithMany()
                .HasForeignKey(usage => usage.UploadedImageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(usage => usage.Post)
                .WithMany()
                .HasForeignKey(usage => usage.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(usage => usage.Comment)
                .WithMany()
                .HasForeignKey(usage => usage.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(usage => usage.IsInGallery)
                .IsRequired();

            entity.Property(usage => usage.SortOrder)
                .IsRequired();

            
    // En bildkoppling måste gälla antingen ett inlägg,
    // en kommentar eller enbart galleriet.
    entity.ToTable(table => table.HasCheckConstraint(
                "CK_ImageUsage_OneTarget",
                """
        (
            [PostId] IS NOT NULL AND [CommentId] IS NULL AND [IsInGallery] = 0
        )
        OR
        (
            [PostId] IS NULL AND [CommentId] IS NOT NULL AND [IsInGallery] = 0
        )
        OR
        (
            [PostId] IS NULL AND [CommentId] IS NULL AND [IsInGallery] = 1
        )
        """));
        });
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(conversation => conversation.Id);

            entity.Property(conversation => conversation.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(conversation => conversation.CreatedAt)
                .IsRequired();
        });
        
        modelBuilder.Entity<ConversationParticipant>(entity =>
        {
            entity.HasKey(participant => participant.Id);

            entity.Property(participant => participant.JoinedAt)
                .IsRequired();

            entity.HasOne(participant => participant.Conversation)
              .WithMany(conversation => conversation.Participants)
              .HasForeignKey(participant => participant.ConversationId)
              .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(participant => participant.User)
                .WithMany()
                .HasForeignKey(participant => participant.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(participant => new
            {
                participant.ConversationId,
                participant.UserId
            }).IsUnique();
        });
        modelBuilder.Entity<GroupMessageReadStatus>(entity =>
        {
            entity.HasKey(status => status.Id);

            entity.HasOne(status => status.Message)
                .WithMany()
                .HasForeignKey(status => status.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(status => status.User)
                .WithMany()
                .HasForeignKey(status => status.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // En användare ska bara ha en lässtatus
            // per meddelande.
            entity.HasIndex(status => new
            {
                status.MessageId,
                status.UserId
            }).IsUnique();
        });
        modelBuilder.Entity<ContentReport>(entity =>
        {
            entity.Property(report => report.Reason)
                .IsRequired()
                .HasMaxLength(1000);
            
            entity.Property(report => report.CreatedAt)
                .IsRequired();

            // En anmälan ska gälla antingen ett inlägg
            // eller en kommentar, aldrig båda eller inget av dem.
            entity.ToTable(table => table.HasCheckConstraint(
                "CK_ContentReports_PostOrComment",
                """
        ([PostId] IS NOT NULL AND [CommentId] IS NULL)
        OR
        ([PostId] IS NULL AND [CommentId] IS NOT NULL)
        """));

            entity.HasOne(report => report.ReporterUser)
                .WithMany()
                .HasForeignKey(report => report.ReporterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(report => report.Post)
                .WithMany()
                .HasForeignKey(report => report.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(report => report.Comment)
                .WithMany()
                .HasForeignKey(report => report.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(like => like.Id);

            
            // En användare kan bara gilla samma inlägg en gång.
            entity.HasIndex(like => new
            {
                like.UserId,
                like.PostId
            })
            .IsUnique()
            .HasFilter("[PostId] IS NOT NULL");
            
            // En användare kan bara gilla samma kommentar en gång.
            entity.HasIndex(like => new
            {
                like.UserId,
                like.CommentId
            })
            .IsUnique()
            .HasFilter("[CommentId] IS NOT NULL");
            
            entity.HasOne(like => like.User)
                .WithMany()
                .HasForeignKey(like => like.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(like => like.Post)
             .WithMany()
             .HasForeignKey(like => like.PostId)
               .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(like => like.Comment)
                .WithMany()
                .HasForeignKey(like => like.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // En gilla-markering måste tillhöra antingen
            // ett inlägg eller en kommentar.
            entity.ToTable(table => table.HasCheckConstraint(
                "CK_Likes_OneTarget",
                """
        ([PostId] IS NOT NULL AND [CommentId] IS NULL)
        OR
        ([PostId] IS NULL AND [CommentId] IS NOT NULL)
        """));
        });
    }
    


} 