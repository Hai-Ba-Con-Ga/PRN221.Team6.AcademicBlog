using System;
using System.Collections.Generic;

namespace AcademicBlog.Domain.Entities;

public partial class Hit
{
    public int Id { get; set; }

    public string SessionId { get; set; } = null!;

    public int PostId { get; set; }

    public virtual Post Post { get; set; } = null!;
}
