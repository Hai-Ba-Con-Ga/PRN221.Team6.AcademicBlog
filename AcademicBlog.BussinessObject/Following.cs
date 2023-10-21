using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Following
{
    public int Id { get; set; }

    public int FollowerId { get; set; }

    public int FollowingId { get; set; }

    public virtual Account Follower { get; set; } = null!;

    public virtual Account FollowingNavigation { get; set; } = null!;
}
