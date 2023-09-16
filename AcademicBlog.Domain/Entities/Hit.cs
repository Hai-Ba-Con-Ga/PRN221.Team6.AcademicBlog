using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Hit")]
    public class Hit : BaseEntity
    {
        public string SessionId { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

    }
}
