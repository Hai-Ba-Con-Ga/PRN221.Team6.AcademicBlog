using System;
using System.Collections.Generic;

namespace AcademicBlog.Infrastructure.Entities;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
