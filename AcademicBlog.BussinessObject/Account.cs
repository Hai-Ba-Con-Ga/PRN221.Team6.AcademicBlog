using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Account
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Post> PostApprovers { get; set; } = new List<Post>();

    public virtual ICollection<Post> PostCreators { get; set; } = new List<Post>();

    public virtual Role Role { get; set; } = null!;
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public virtual ICollection<Following> FollowingFollowers { get; set; } = new List<Following>();

    public virtual ICollection<Following> FollowingFollowingNavigations { get; set; } = new List<Following>();
}
