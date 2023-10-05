﻿using System;
using System.Collections.Generic;

namespace AcademicBlog.Infrastructure.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
