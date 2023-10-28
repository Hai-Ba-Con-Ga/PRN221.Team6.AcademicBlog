using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
