using AcademicBlog.BussinessObject;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages.Blogs
{
    public class BlogDetailModel : PageModel
    {
        private readonly IPostRepository _postRepository;

        public BlogDetailModel(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Post Post { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Post = await _postRepository.GetById(Id);
            if (Post == null)
            {
                ErrorMessage = "Post not found";
                return RedirectToPage("/Index");
            }
            return Page();
        }

    }
}
