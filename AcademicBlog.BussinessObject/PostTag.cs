using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcademicBlog.BussinessObject;

public partial class PostTag
{
    [Key]
    public int PostId { get; set; }

    [Key]
    public int TagId { get; set; }

    
    public virtual Post Post { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
