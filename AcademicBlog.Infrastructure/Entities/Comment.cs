using System;
using System.Collections.Generic;

namespace AcademicBlog.Infrastructure.Entities;

public partial class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public int PostId { get; set; }

    public int CreatorId { get; set; }

    public int ParentId { get; set; }

    public string Path { get; set; } = null!;

    public virtual Account Creator { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
