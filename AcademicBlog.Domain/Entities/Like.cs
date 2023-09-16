using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Like")]
    public class Like : BaseEntity
    {
        public Guid CreatorId { get; set; }
        public virtual Account Creator { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
