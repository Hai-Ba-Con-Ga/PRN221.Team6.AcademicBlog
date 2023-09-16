using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        public Guid? CreatorId { get; set; }
        public virtual Account Creator { get; set; }
    }
}
