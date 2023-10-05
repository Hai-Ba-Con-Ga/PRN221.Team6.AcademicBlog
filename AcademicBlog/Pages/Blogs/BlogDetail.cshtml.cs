using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages.Blogs
{
    public class BlogDetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public void OnGet()
        {
        }
    }
}
