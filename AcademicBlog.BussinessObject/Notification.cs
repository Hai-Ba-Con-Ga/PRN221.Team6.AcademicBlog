using System;
using System.Collections.Generic;

namespace AcademicBlog.BussinessObject;

public partial class Notification
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int ReceiverId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int PostId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual Account Receiver { get; set; } = null!;
}
