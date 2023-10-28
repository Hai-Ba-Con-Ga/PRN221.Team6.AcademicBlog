using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AcademicBlog.BussinessObject;

public partial class AcademicBlogDbContext : DbContext
{
    public AcademicBlogDbContext()
    {
    }

    public AcademicBlogDbContext(DbContextOptions<AcademicBlogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bookmark> Bookmarks { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    public virtual DbSet<Hit> Hits { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostTag> PostTags { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<Following> Followings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        optionsBuilder
           .UseLoggerFactory(loggerFactory) // Attach the logger factory
                .EnableSensitiveDataLogging()   // Include sensitive data in logs
            .UseSqlServer("server =wyvernpserver.tech; database = AcademicBlogDB;uid=sa;pwd=ThanhPhong2506;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC277C099CDF");

            entity.ToTable("Account");

            entity.HasIndex(e => e.RoleId, "IX_Account_RoleID");

            entity.HasIndex(e => new { e.Username, e.Email }, "UC_Username_Email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fullname).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
            entity.HasMany(d => d.Skills).WithMany(p => p.Accounts)
               .UsingEntity<Dictionary<string, object>>(
                   "AccountSkill",
                   r => r.HasOne<Skill>().WithMany()
                       .HasForeignKey("SkillId")
                       .OnDelete(DeleteBehavior.ClientSetNull)
                       .HasConstraintName("FK_AccSkill_Skill"),
                   l => l.HasOne<Account>().WithMany()
                       .HasForeignKey("AccountId")
                       .OnDelete(DeleteBehavior.ClientSetNull)
                       .HasConstraintName("FK_AccSkill_Acc"),
                   j =>
                   {
                       j.HasKey("AccountId", "SkillId").HasName("PK_AccSkill");
                       j.ToTable("AccountSkill");
                       j.IndexerProperty<int>("AccountId").HasColumnName("accountId");
                       j.IndexerProperty<int>("SkillId").HasColumnName("skillId");
                   });
        });

        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookmark__3214EC27B06F2739");

            entity.ToTable("Bookmark");

            entity.HasIndex(e => e.CreatorId, "IX_Bookmark_CreatorID");

            entity.HasIndex(e => e.PostId, "IX_Bookmark_PostID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatorId).HasColumnName("CreatorID");
            entity.Property(e => e.PostId).HasColumnName("PostID");

            entity.HasOne(d => d.Creator).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookmark_Account");

            entity.HasOne(d => d.Post).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookmark_Post");
        });
        modelBuilder.Entity<Following>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Followin__3214EC27BEFF90D9");
            entity.ToTable("Following");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FollowerId).HasColumnName("FollowerID");
            entity.Property(e => e.FollowingId).HasColumnName("FollowingID");
            entity.HasOne(d => d.Follower).WithMany(p => p.FollowingFollowers)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Following_Follower");
            entity.HasOne(d => d.FollowingNavigation).WithMany(p => p.FollowingFollowingNavigations)
                .HasForeignKey(d => d.FollowingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Following_Following");
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC27489DE8C0");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC27EBE64D02");

            entity.ToTable("Comment");

            entity.HasIndex(e => e.CreatorId, "IX_Comment_CreatorID");

            entity.HasIndex(e => e.PostId, "IX_Comment_PostID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorId).HasColumnName("CreatorID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Path).HasMaxLength(255);
            entity.Property(e => e.PostId).HasColumnName("PostID");

            entity.HasOne(d => d.Creator).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Account");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Post");
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favourit__3214EC27C7CF2A6D");

            entity.ToTable("Favourite");

            entity.HasIndex(e => e.PostId, "IX_Favourite_PostID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatorId).HasColumnName("CreatorID");
            entity.Property(e => e.PostId).HasColumnName("PostID");

            entity.HasOne(d => d.Post).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favourite_Post");
        });

        modelBuilder.Entity<Hit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Hit__3214EC270302DCA0");

            entity.ToTable("Hit");

            entity.HasIndex(e => e.PostId, "IX_Hit_PostID");

            entity.HasIndex(e => e.SessionId, "UC_SessionID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.SessionId)
                .HasMaxLength(50)
                .HasColumnName("SessionID");

            entity.HasOne(d => d.Post).WithMany(p => p.Hits)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hit_Post");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC27DD018ABC");

            entity.ToTable("Notification");

            entity.HasIndex(e => e.PostId, "IX_Notification_PostID");

            entity.HasIndex(e => e.ReceiverId, "IX_Notification_ReceiverID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.Type).HasMaxLength(10);

            entity.HasOne(d => d.Post).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Post");

            entity.HasOne(d => d.Receiver).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Account");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC27773A454B");

            entity.ToTable("Post");

            entity.HasIndex(e => e.ApproverId, "IX_Post_ApproverID");

            entity.HasIndex(e => e.CategoryId, "IX_Post_CategoryID");

            entity.HasIndex(e => e.CreatorId, "IX_Post_CreatorId");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApproveDate).HasColumnType("datetime");
            entity.Property(e => e.ApproverId).HasColumnName("ApproverID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Approver).WithMany(p => p.PostApprovers)
                .HasForeignKey(d => d.ApproverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Approver");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Category");

            entity.HasOne(d => d.Creator).WithMany(p => p.PostCreators)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Account");



            entity.HasMany(d => d.Skills).WithMany(p => p.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PostSkill_Skill"),
                    l => l.HasOne<Post>().WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PostSkill_Post"),
                    j =>
                    {
                        j.HasKey("PostId", "SkillId");
                        j.ToTable("PostSkill");
                        j.IndexerProperty<int>("PostId").HasColumnName("postId");
                        j.IndexerProperty<int>("SkillId").HasColumnName("skillId");
                    });
            //entity.HasMany(d => d.Tags).WithMany(p => p.Posts)
            //  .UsingEntity<Dictionary<string, object>>(
            //      "PostTag",
            //      r => r.HasOne<Tag>().WithMany()
            //          .HasForeignKey("TagId")
            //          .OnDelete(DeleteBehavior.ClientSetNull)
            //          .HasConstraintName("FK_PostTag_Tag"),
            //      l => l.HasOne<Post>().WithMany()
            //          .HasForeignKey("PostId")
            //          .OnDelete(DeleteBehavior.ClientSetNull)
            //          .HasConstraintName("FK_PostTag_Post"),
            //      j =>
            //      {
            //          j.HasKey("PostId", "TagId").HasName("PK__PostTag__7C45AF9C10732B7F");
            //          j.ToTable("PostTag");
            //          j.HasIndex(new[] { "PostId" }, "IX_PostTag_PostID");
            //          j.HasIndex(new[] { "TagId" }, "IX_PostTag_TagID");
            //          j.IndexerProperty<int>("PostId").HasColumnName("PostID");
            //          j.IndexerProperty<int>("TagId").HasColumnName("TagID");
            //      });
        });
        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__skill__3213E83F6DBB2C02");
            entity.ToTable("skill");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });
        modelBuilder.Entity<PostTag>(entity =>
    {
        entity
            .HasKey(e => new { e.TagId, e.PostId });
        //.ToTable("PostTag");
        entity.ToTable("PostTag");

        entity.HasIndex(e => e.PostId, "IX_PostTag_PostID");

        entity.HasIndex(e => e.TagId, "IX_PostTag_TagID");

        entity.Property(e => e.PostId).HasColumnName("PostID");
        entity.Property(e => e.TagId).HasColumnName("TagID");

        entity.HasOne(d => d.Post).WithMany()
            .HasForeignKey(d => d.PostId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PostTag_Post");

        entity.HasOne(d => d.Tag).WithMany()
            .HasForeignKey(d => d.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PostTag_Tag");
    });

        modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Role__3214EC27E8245A8B");

                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasMaxLength(10);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC27A0DFC654");

                entity.ToTable("Tag");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasMaxLength(20);
            });


        OnModelCreatingPartial(modelBuilder);
        }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
