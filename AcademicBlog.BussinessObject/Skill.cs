using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Skill
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
