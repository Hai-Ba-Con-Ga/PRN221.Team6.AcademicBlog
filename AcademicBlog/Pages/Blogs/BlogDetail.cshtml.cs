using AcademicBlog.BussinessObject;
using AcademicBlog.Repository.DTO;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AcademicBlog.Pages.Blogs
{
    public class BlogDetailModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commnentRepository;

        public BlogDetailModel(IPostRepository postRepository, ICommentRepository commnentRepository)
        {
            _postRepository = postRepository;
            _commnentRepository = commnentRepository;
        }
        

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Post Post { get; set; }
        public bool IsDisplayFollowingButton { get; set; } = false;
        public bool IsFollowed { get; set; } = false;
        public IDictionary<int, CommentObject> Comments { get; set; }
        public ICollection<CommentObject> PrimaryComments { get; set; } = new List<CommentObject>();

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
            var childrenGroup = Post.Comments.GroupBy(p => p.ParentId).ToDictionary(p => p.Key, p => p.Select(p => p.Id).ToList());
            if (Post.Comments is not null)
            {
                Comments = Post.Comments.Select(p =>
                {
                    var cmtObj =  new CommentObject()
                    {
                        Id = p.Id,
                        Content = p.Content,
                        CreatedDate = p.CreatedDate,
                        ModifiedDate = p.ModifiedDate,
                        CreatorId = p.CreatorId,
                        Creator = p.Creator,
                        Path = p.Path,
                        ParentId = p.ParentId,
                        PostId = p.PostId,
                        ChildrenId = childrenGroup.ContainsKey(p.Id) ? childrenGroup[p.Id] : null
                    };
                    if(p.ParentId == 0)
                    {
                        PrimaryComments.Add(cmtObj);
                    }
                    return cmtObj;
                }).ToDictionary(post => post.Id);
            }
         
            return Page();
        }
        [BindProperty]
        public string CommentContent { get; set; }
        public async Task<IActionResult> OnPostCommentAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value??"-1");
            if(accountId >= 0) {
            await _commnentRepository.Add(new() { 
                Content = CommentContent,
                Path = "/",
                CreatorId = accountId,
                CreatedDate = DateTime.Now,
                PostId = Id,
                ModifiedDate = DateTime.Now,
            });
            }
            await Console.Out.WriteLineAsync(CommentContent);
            return await OnGet();
        }

        [BindProperty]

        public int ParentId { get;set; }

        [BindProperty]

        public string CurrentPath { get; set; }

        public async Task<IActionResult> OnPostCommentReplyAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId >= 0)
            {
                await _commnentRepository.Add(new()
                {
                    Content = CommentContent,
                    Path = $"{CurrentPath}{ParentId}/",
                    CreatorId = accountId,
                    CreatedDate = DateTime.Now,
                    PostId = Id,
                    ModifiedDate = DateTime.Now,
                    ParentId = ParentId
                });
            }
            await Console.Out.WriteLineAsync(CommentContent);
            return await OnGet();
        }

    }
}
