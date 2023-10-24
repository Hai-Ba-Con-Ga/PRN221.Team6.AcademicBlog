using AcademicBlog.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Repository.DTO
{
    public class CommentObject
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int PostId { get; set; }

        public int CreatorId { get; set; }

        public int ParentId { get; set; }

        public string Path { get; set; } = null!;
        public IEnumerable<int> ChildrenId { get; set; }

        public virtual Account Creator { get; set; } = null!;

        public virtual Post Post { get; set; } = null!;
    }
}
