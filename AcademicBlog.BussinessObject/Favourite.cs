using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Favourite
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int CreatorId { get; set; }

    public virtual Post Post { get; set; } = null!;
}
