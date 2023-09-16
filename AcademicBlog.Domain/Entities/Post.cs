using AcademicBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicBlog.Domain.Entities
{
    [Table("Post")]
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ThumbnailUrl { get; set; }
        public Guid CreatorId { get; set; }
        public virtual Account Creator { get; set; }

        public ICollection<Hit> Hits { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }  
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
