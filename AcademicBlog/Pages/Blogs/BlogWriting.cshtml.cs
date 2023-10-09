using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AcademicBlog.Pages.Blogs
{
    public class BlogWritingModel : PageModel
    {
        [BindProperty]
        public BlogPostRequest BlogPostRequest { get; set; }
        private readonly ICategoryRepository categoryRepository;
        private readonly IPostRepository postRepository;
        public List<Category> Categories { get; set; } = new List<Category>();
        public  BlogWritingModel(ICategoryRepository categoryRepository, IPostRepository postRepository)
        {
            this.categoryRepository = categoryRepository;
            this.postRepository = postRepository;
            InitializeAsync().GetAwaiter().GetResult();
            this.postRepository = postRepository;
        }
        public async Task InitializeAsync()
        {
            Categories = (await categoryRepository.GetAll()).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            
            return Page();
        }
      
        public async Task<IActionResult> OnPostAsync() {
            Console.WriteLine(BlogPostRequest);
            if (User is not null) {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var newPost = new Post()
            {
                CategoryId =  BlogPostRequest.Category,
                Title = BlogPostRequest.Title,
                Content = BlogPostRequest.Content,
                ThumbnailUrl = BlogPostRequest.Thumbnail,
                CreatorId = int.Parse(userId),

                };
                var persistedPost = await postRepository.Add(newPost);
                if(persistedPost != null)
                {
                    //TODO persist tags
                }
            }
            return Page();
        }
    }
}
