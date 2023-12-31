﻿using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    public string ThumbnailUrl { get; set; } = null!;

    public bool IsPublic { get; set; } = false;

    public int CreatorId { get; set; }

    public int CategoryId { get; set; }

    public int? ApproverId { get; set; }

    public DateTime? ApproveDate { get; set; }

    public int? Status { get; set; } = 0;

    public virtual Account? Approver { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Account Creator { get; set; } = null!;

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Hit> Hits { get; set; } = new List<Hit>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
