using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Bookmark")]
    public class Bookmark : BaseEntity
    {
        public Guid CreatorId { get; set; }
        public Account Creator { get; set; }
        public Guid PostId { get; set; }

        public Post Post { get; set; }
    }
}
