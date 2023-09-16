using AcademicBlog.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicBlog.Domain.Entities
{
    [Table("Account")]
    public class Account : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
