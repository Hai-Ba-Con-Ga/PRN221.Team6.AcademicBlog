using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string ThumbnailUrl { get; set; } = null!;

    public bool IsPublic { get; set; }

    public int CreatorId { get; set; }

    public int CategoryId { get; set; }

    public int ApproverId { get; set; }

    public DateTime ApproveDate { get; set; }

    public virtual Account Approver { get; set; } = null!;

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Account Creator { get; set; } = null!;

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Hit> Hits { get; set; } = new List<Hit>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
