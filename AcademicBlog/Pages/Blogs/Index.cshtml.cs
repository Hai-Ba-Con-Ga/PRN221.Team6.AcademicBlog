using AcademicBlog.Pages.Blogs.Component;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages.Blogs
{
   
    public class IndexModel : PageModel
    {
        public List<TabItem> Tabs { get; set; }

        [BindProperty]
        public string SearchKeyword { get;set; }
        public void OnGet()
        {
            Tabs = new List<TabItem>
        {
            new TabItem { Text = "Lastest", Key = "lastest"},
            new TabItem { Text = "Trending", Key = "trending" },
            new TabItem { Text = "Following", Key = "following" },
            new TabItem { Text = "Bookmark", Key = "bookmark" }
        };
        }
    }
}
