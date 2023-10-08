using AcademicBlog.BussinessObject;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPostRepository  _postRepository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }
        // [BindProperty]
        public IEnumerable<Post> Posts { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }



        public async Task<IActionResult> OnGet()
        {
            //get post
            Posts = await _postRepository.GetAll(9);
            return Page();
        }
        
        
    }
}