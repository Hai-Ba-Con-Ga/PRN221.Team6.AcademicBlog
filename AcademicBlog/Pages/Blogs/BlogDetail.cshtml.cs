using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.DTO;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace AcademicBlog.Pages.Blogs
{
    public class BlogDetailModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commnentRepository;
        private readonly IFollowingRepository _followingRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IBookmarkRepository _bookmarkRepository;

        public int AccountId { get; set; } = 0;

        public BlogDetailModel(IPostRepository postRepository, ICommentRepository commnentRepository,IFollowingRepository followingRepository, IFavoriteRepository favoriteRepository, IBookmarkRepository bookmarkRepository)
        {
            _postRepository = postRepository;
            _commnentRepository = commnentRepository;
            _followingRepository = followingRepository;
            _favoriteRepository = favoriteRepository;
            _bookmarkRepository = bookmarkRepository;
            AccountId = Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Post Post { get; set; }
        public bool IsDisplayFollowingButton { get; set; } = true;
        public bool IsFollowed { get; set; } = true;
        
        public bool IsBookmark { get; set; } = false;
        public bool IsFavorite { get; set; } = false;

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
            // Follow 
            AccountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            
            var followingConnection = (await _followingRepository.GetList(new()
            {
                PageIndex = 1,
                PageSize = 1,
                Filter = new()
                {
                    Logic = "and",
                    Filters = new List<Filter>()
                        {
                            new()
                            {
                                Field = "FollowerId",
                                Operator = "eq",
                                Value = AccountId
                            },
                            new()
                            {
                                  Field = "FollowingId",
                                Operator = "eq",
                                Value = Post.CreatorId
                            }
                        }
                }

            })).FirstOrDefault();
            if(AccountId < 0 || Post.CreatorId == AccountId)
            {
                IsDisplayFollowingButton = false;
            }
            if(followingConnection is null) { IsFollowed = false; }
            var bookmarkConnection = (await _bookmarkRepository.GetList(new()
            {
                PageIndex = 1,
                PageSize = 1,
                Filter = new()
                {
                    Logic = "and",
                    Filters = new List<Filter>()
                        {
                            new()
                            {
                                Field = "CreatorId",
                                Operator = "eq",
                                Value = AccountId
                            },
                            new()
                            {
                                  Field = "PostId",
                                Operator = "eq",
                                Value = Id
                            }
                        }
                }

            })).FirstOrDefault();
            var favoriteConnection = (await _favoriteRepository.GetList(new()
            {
                PageIndex = 1,
                PageSize = 1,
                Filter = new()
                {
                    Logic = "and",
                    Filters = new List<Filter>()
                        {
                            new()
                            {
                                Field = "CreatorId",
                                Operator = "eq",
                                Value = AccountId
                            },
                            new()
                            {
                                  Field = "PostId",
                                Operator = "eq",
                                Value = Id
                            }
                        }
                }

            })).FirstOrDefault();
            if(bookmarkConnection is not null)
            {
                IsBookmark = true;
            }
            if(favoriteConnection is not null)
            {
                IsFavorite = true;
            }
            return Page();

        }
        [BindProperty]
        public string CommentContent { get; set; }

        [Authorize]
        public async Task<IActionResult> OnPostCommentAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value??"-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
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
        [Authorize]

        public async Task<IActionResult> OnPostCommentReplyAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
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

            return await OnGet();
        }
        [BindProperty]

        public string ActionFollow { get; set; } //follow / unfollow
        [BindProperty]

        public int CreatorId { get; set; } 
        [Authorize]

        public async Task<IActionResult> OnPostFollowAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                var followingConnection = (await _followingRepository.GetList(new()
                {
                    PageIndex = 1,
                    PageSize = 1,
                    Filter = new()
                    {
                        Logic = "and",
                        Filters = new List<Filter>()
                        {
                            new()
                            {
                                Field = "FollowerId",
                                Operator = "eq",
                                Value = accountId
                            },
                            new()
                            {
                                  Field = "FollowingId",
                                Operator = "eq",
                                Value = CreatorId
                            }
                        }
                    }

                })).FirstOrDefault();
                switch (ActionFollow)
                {
                    case "follow":
                        {
                            if(followingConnection == null)
                            {
                                await _followingRepository.Add(new()
                                {
                                    FollowerId = accountId,
                                    FollowingId = CreatorId,
                                });
                            }
                            break;
                        }
                    case "unfollow":
                        {

                            if (followingConnection != null)
                            {
                                await _followingRepository.Delete(followingConnection);
                            }
                            break;
                        }
                }
                
            }
            return await OnGet();
        }

        [BindProperty]

        public string ActionFavorite { get; set; } //favorite / unfavorite
       
        [Authorize]
        public async Task<IActionResult> OnPostFavoriteAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                var favoriteConnection = (await _favoriteRepository.GetList(new()
                {
                    PageIndex = 1,
                    PageSize = 1,
                    Filter = new()
                    {
                        Logic = "and",
                        Filters = new List<Filter>()
                        {
                            new()
                            {
                                Field = "CreatorId",
                                Operator = "eq",
                                Value = accountId
                            },
                            new()
                            {
                                  Field = "PostId",
                                Operator = "eq",
                                Value = Id
                            }
                        }
                    }

                })).FirstOrDefault();
                switch (ActionFavorite)
                {
                    case "favorite":
                        {
                            if (favoriteConnection == null)
                            {
                                await _favoriteRepository.Add(new()
                                {
                                    CreatorId = accountId,
                                    PostId = Id,
                                });
                            }
                            break;
                        }
                    case "unfavorite":
                        {

                            if (favoriteConnection != null)
                            {
                                await _favoriteRepository.Delete(favoriteConnection);
                            }
                            break;
                        }
                }

            }
            return await OnGet();
        }

        [BindProperty]

        public string ActionBookmark { get; set; } //bookmark/unbookmark

        [Authorize]
        public async Task<IActionResult> OnPostBookmarkAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                var bookmarkConnection = (await _bookmarkRepository.GetList(new()
                {
                    PageIndex = 1,
                    PageSize = 1,
                    Filter = new()
                    {
                        Logic = "and",
                        Filters = new List<Filter>()
                        {
                            new()
                            {
                                Field = "CreatorId",
                                Operator = "eq",
                                Value = accountId
                            },
                            new()
                            {
                                  Field = "PostId",
                                Operator = "eq",
                                Value = Id
                            }
                        }
                    }

                })).FirstOrDefault();
                switch (ActionBookmark)
                {
                    case "bookmark":
                        {
                            if (bookmarkConnection == null)
                            {
                                await _bookmarkRepository.Add(new()
                                {
                                    CreatorId = accountId,
                                    PostId = Id,
                                });
                            }
                            break;
                        }
                    case "unbookmark":
                        {

                            if (bookmarkConnection != null)
                            {
                                await _bookmarkRepository.Delete(bookmarkConnection);
                            }
                            break;
                        }
                }

            }
            return await OnGet();
        }

    }
}
