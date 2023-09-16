using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AcademicBlog.Domain.Common.Enums;

namespace AcademicBlog.Domain.Entities
{
    [Table("Notification")]
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ReceiverId { get; set; }
        public virtual Account Receiver { get; set; }

    }
}
