using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Packaging;
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
        private readonly ISkillRepository skillRepository;
        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<int> InputSkills { get; set; } = new List<int>();

        [FromQuery(Name = "mode")]
        public string Mode { get; set; } = "draft"; //prototype draft/edit

        [FromQuery(Name = "id")]
        public int PostId{ get; set; }
        public bool IsEdit { get; set; } = false;
        public Post EditPost { get; set; }
        public string PageTitle { get; set; } = "Write a new blog";
        public BlogWritingModel(ICategoryRepository categoryRepository, IPostRepository postRepository, ITagRepository tagRepository, IPostTagRepository postTagRepository, ISkillRepository skillRepository)
        {
            this.categoryRepository = categoryRepository;
            this.postRepository = postRepository;
            this.tagRepository = tagRepository;
            this.postTagRepository = postTagRepository;
            this.skillRepository = skillRepository;
        }
        

        public async Task InitializeAsync()
        {
            Categories = (await categoryRepository.GetAll()).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = (await categoryRepository.GetAll()).ToList();
            Skills = (await skillRepository.GetAll()).ToList();
            if(Mode.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                IsEdit = true;
                int userId = int.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value?? "-1");
                var post  = (await postRepository.GetById(PostId));
                if (post is null) return Redirect("/404");
                if (post.CreatorId != userId) return Redirect("/403");
                EditPost = post;
                PageTitle = "Update blog";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Mode.Equals("edit", StringComparison.OrdinalIgnoreCase))
            {
                IsEdit = true;
                int userId = int.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
                var post = (await postRepository.GetById(PostId));
                if (post is null) return Redirect("/404");
                if (post.CreatorId != userId) return Redirect("/403");
                EditPost = post;
            }
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
                var skills = await skillRepository.GetAll();

                if (IsEdit)
                {
                    EditPost.CategoryId = BlogPostRequest.Category;
                    EditPost.Content = BlogPostRequest?.Content ?? EditPost.Content;
                    EditPost.Title = BlogPostRequest?.Title ?? EditPost.Title;
                    EditPost.ThumbnailUrl = BlogPostRequest?.Thumbnail ?? EditPost.ThumbnailUrl;
                    EditPost.IsPublic = false;
                    EditPost.ApproveDate = null;
                    EditPost.Approver = null;
                    await postRepository.Update(EditPost);
                }
                else
                {
                    var newPost = new Post()
                    {
                        CategoryId = BlogPostRequest.Category,
                        Title = BlogPostRequest?.Title ?? "Untitled",
                        Content = BlogPostRequest?.Content ?? "",
                        ThumbnailUrl = BlogPostRequest?.Thumbnail?? "https://source.unplash.com/random",
                        CreatorId = int.Parse(userId)
                    };
                    var persistedPost = await postRepository.Add(newPost);
                        persistedPost.Tags = tags;
                        persistedPost.Skills = skills.ToList().FindAll(skill => this.BlogPostRequest.Skill.Contains(skill.Id));
                        await postRepository.Update(persistedPost);


                }
                return Redirect("/blogs?tab=mypending");
           
            } else
            {
                return Redirect("/Auth/Login?returnUrl=/Blogs/BlogWriting");
            }
        }
    }
}
