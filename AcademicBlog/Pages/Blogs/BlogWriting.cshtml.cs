using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITagRepository tagRepository;
        private readonly IPostTagRepository postTagRepository;
        public List<Category> Categories { get; set; } = new List<Category>();
        public BlogWritingModel(ICategoryRepository categoryRepository, IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository)
        {
            this.categoryRepository = categoryRepository;
            this.postRepository = postRepository;
            this.tagRepository = tagRepository;
            this.postRepository = postRepository;
            this.postTagRepository = postTagRepository;
            InitializeAsync().GetAwaiter().GetResult();

        }
        public async Task InitializeAsync()
        {
            Categories = (await categoryRepository.GetAll()).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine(BlogPostRequest);
            if (User is not null)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var tags = new List<Tag>();
                var remainTags = new List<Tag>();
                BlogPostRequest.Tag.AsParallel().ToList().ForEach(async tag =>
                {
                    var persistedTag = await tagRepository.FindByName(tag);
                    if (persistedTag is null)
                    {
                        remainTags.Add(new() { Name=tag});
                    }else
                    {
                        tags.Add(persistedTag);
                    }
                });

                tags.AddRange(await tagRepository.AddRange(remainTags));
                var newPost = new Post()
                {
                    CategoryId = BlogPostRequest.Category,
                    Title = BlogPostRequest?.Title ?? "Untitled",
                    Content = BlogPostRequest.Content,
                    ThumbnailUrl = BlogPostRequest?.Thumbnail?? "https://source.unplash.com/random",
                    CreatorId = int.Parse(userId)
                };
                var persistedPost = await postRepository.Add(newPost);
                if(persistedPost is not null)
                {
                    tags.ForEach(tag => postTagRepository.Add(new() { PostId = persistedPost.Id, TagId = tag.Id }));
                }
            } else
            {
                return Redirect("/Auth/Login?returnUrl=/Blogs/BlogWriting");
            }
            return Page();
        }
    }
}
