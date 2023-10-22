using AcademicBlog.Repository.DTO;

namespace AcademicBlog.Pages.Blogs.Component.UI
{
    public class CommentViewModel
    {
        public CommentObject Comment { get; set; }
        public IDictionary<int, CommentObject> CommentList { get; set; }
    }
}
