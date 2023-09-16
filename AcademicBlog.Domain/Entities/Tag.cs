using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Tag")]
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
