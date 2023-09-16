using AcademicBlog.Domain.Config;
using AcademicBlog.Domain.Entities;

namespace AcademicBlog.Infrastructure.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    #region DbSet
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Hit> Hits { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasMany(e => e.Accounts)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Account>()
            .HasMany(e => e.Bookmarks)
            .WithOne(e => e.Creator)
            .HasForeignKey(e => e.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Account>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.Creator)
            .HasForeignKey(e => e.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Account>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Creator)
            .HasForeignKey(e => e.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Account>()
            .HasMany(e => e.Notifications)
            .WithOne(e => e.Receiver)
            .HasForeignKey(e => e.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Post>()
            .HasMany(e => e.Hits)
            .WithOne(e => e.Post)
            .HasForeignKey(e => e.PostId);
        modelBuilder.Entity<Post>()
            .HasMany(e => e.Likes)
            .WithOne(e => e.Post)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Post>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.Post)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Post>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Posts);
        modelBuilder.Entity<Post>()
            .HasMany(e => e.Bookmarks)
            .WithOne(e => e.Post)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.NoAction);
    }


}


