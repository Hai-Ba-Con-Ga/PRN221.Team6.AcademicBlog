﻿using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Bookmark
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int CreatorId { get; set; }

    public virtual Account Creator { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
