namespace AcademicBlog.Pages.Blogs.Component
{
    public class BlogPostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public int Category { get; set; } 
        public List<string> Tag { get; set; }
        public IFormFile File {  get; set; }

    }
}