using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.DTO;
using AcademicBlog.Repository.Interface;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

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

        public BlogDetailModel(IPostRepository postRepository, ICommentRepository commnentRepository, IFollowingRepository followingRepository, IFavoriteRepository favoriteRepository, IBookmarkRepository bookmarkRepository)
        {
            _postRepository = postRepository;
            _commnentRepository = commnentRepository;
            _followingRepository = followingRepository;
            _favoriteRepository = favoriteRepository;
            _bookmarkRepository = bookmarkRepository;

        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Post Post { get; set; }
        public IEnumerable<Post> relatedPosts { get; set; } = new List<Post>();
        public bool IsDisplayFollowingButton { get; set; } = true;
        public bool IsDisplayApproveButton { get; set; } = false;
        public bool IsFollowed { get; set; } = true;

        public bool IsBookmark { get; set; } = false;
        public bool IsFavorite { get; set; } = false;
        public bool IsApprove { get; set; } = false;
        public bool IsReject { get; set; } = false;
        public bool IsOwner { get; set; } = false;

        public IDictionary<int, CommentObject> Comments { get; set; }
        public ICollection<CommentObject> PrimaryComments { get; set; } = new List<CommentObject>();



        [TempData]
        public string ErrorMessage { get; set; }
        public async Task<IActionResult> OnGet()
        {
            AccountId = Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            Console.WriteLine(AccountId);
            Post = await _postRepository.GetById(Id);
            if(AccountId == Post.CreatorId)
            {
                IsOwner = true;
            }
            if (Post == null)
            {
                ErrorMessage = "Post not found";
                return RedirectToPage("/Index");
            }
            //check post is public or not
            if (!Post.IsPublic && AccountId < 0)
            {
                ErrorMessage = "Post not found";
                return RedirectToPage("/Index");
            }
            
            if (!Post.IsPublic && !User.IsInRole("Mod"))
            {
                if (Post.CreatorId != AccountId)
                {
                    ErrorMessage = "Post not found";
                    return RedirectToPage("/Index");
                }
            }
            var filter = new ProfanityFilter.ProfanityFilter();
            var childrenGroup = Post.Comments.GroupBy(p => p.ParentId).ToDictionary(p => p.Key, p => p.Select(p => p.Id).ToList());
            if (Post.Comments is not null)
            {
                Comments = Post.Comments.Select(p =>
                {
                    var cmtObj = new CommentObject()
                    {
                        Id = p.Id,
                        Content = filter.CensorString(p.Content),
                        CreatedDate = p.CreatedDate,
                        ModifiedDate = p.ModifiedDate,
                        CreatorId = p.CreatorId,
                        Creator = p.Creator,
                        Path = p.Path,
                        ParentId = p.ParentId,
                        PostId = p.PostId,
                        ChildrenId = childrenGroup.ContainsKey(p.Id) ? childrenGroup[p.Id] : null
                    };
                    if (p.ParentId == 0)
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
            if (AccountId < 0 || Post.CreatorId == AccountId)
            {
                IsDisplayFollowingButton = false;
            }
            if (followingConnection is null) { IsFollowed = false; }
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
            if (bookmarkConnection is not null)
            {
                IsBookmark = true;
            }
            if (favoriteConnection is not null)
            {
                IsFavorite = true;
            }

            if (AccountId > 0 && User.IsInRole("Mod") && Post.Status == 0)
            {
                IsDisplayApproveButton = true;
                if (Post.Status == 1)
                {
                    IsApprove = true;
                }
                if (Post.Status == 2)
                {
                    IsReject = true;
                }
            }
            relatedPosts = (await _postRepository.GetAllPost(new()
            {
                PageIndex = 1,
                PageSize = 4,
                Sort= new List<Sort>()
                {
                    new(){Field = "CreatedDate",Dir = "DESC"}
                },
                Filter = new()
                {
                    Field = "CategoryId",
                    Operator= "eq",
                    Value = Post.CategoryId
                }
            })).ToList();
            return Page();

        }
        [BindProperty]
        public string CommentContent { get; set; }

        [Authorize]
        public async Task<IActionResult> OnPostCommentAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                await _commnentRepository.Add(new()
                {
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

        public int ParentId { get; set; }

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
                            if (followingConnection == null)
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

        public async Task<IActionResult> OnPostApproveAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                var post = await _postRepository.GetById(Id);
                if (post is not null)
                {
                    post.Status = 1;
                    post.IsPublic = true;
                    post.ApproverId = accountId;
                    post.ApproveDate = DateTime.Now;
                    await _postRepository.Update(post);
                }
                using (var client = new HttpClient())
                {
                    string emailTemplate = Utils.Utils.PostApprovalEmail(true, post);
                    var body = JsonSerializer.Serialize(new
                    {
                        to = post?.Creator?.Email?? "phonglethanh2@gmail.com", 
                        content = emailTemplate
                    });
                    var requestContent = new StringContent(body, Encoding.UTF8, "application/json");
                    _ = await client.PostAsync("http://notification.wyvernpserver.tech/mail/send", requestContent);
                }
                Pagable pagable = new()
                {
                    PageIndex = 1,
                    PageSize = 20,
                    Filter = new Filter()
                    {
                        Logic = "and",
                        Filters = new List<Filter>()
                                {
                                    new Filter()
                                    {
                                        Field = "FollowingId",
                                        Operator = "eq",
                                        Value = Id
                                    }
                                }
                    }
                };
                using (var client = new HttpClient())
                {
                    var follower = await _followingRepository.GetList(pagable, f => f.Include(f => f.Follower));
                    var followers = follower.Select(f => f.Follower);
                    string emailNoticeTemplate = Utils.Utils.NewPostNotificationEmail(post);

                    // Create tasks for sending notifications to followers
                    var notificationTasks = followers?.Select(async follower =>
                    {
                        var bodyNotice = JsonSerializer.Serialize(new
                        {
                            to = follower.Email ?? "phonglethanh2@gmail.com",
                            content = emailNoticeTemplate
                        });
                        var requestNoticeContent = new StringContent(bodyNotice, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://notification.wyvernpserver.tech/mail/send", requestNoticeContent);
                        // Optionally, you can check the response here
                    });

                    // Await all notification tasks
                    await Task.WhenAll(notificationTasks);
                }
            }
            return await OnGet();
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                var post = await _postRepository.GetById(Id);
                if (post is not null)
                {
                    post.Status = 2;
                    post.IsPublic = false;  
                    post.ApproverId = accountId;
                    post.ApproveDate = DateTime.Now;
                    await _postRepository.Update(post);
                }
                string emailTemplate = Utils.Utils.PostApprovalEmail(false, post);
                var body = JsonSerializer.Serialize(new
                {
                    to = post?.Creator?.Email ?? "phonglethanh2@gmail.com",
                    content = emailTemplate
                });
                using (var client = new HttpClient())
                {
                    var requestContent = new StringContent(body, Encoding.UTF8, "application/json");
                    _ = client.PostAsync("http://notification.wyvernpserver.tech/mail/send", requestContent);
                }
            }
            return await OnGet();
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Blogs/BlogDetail/{Id}");
            }
            if (accountId >= 0)
            {
                var post = await _postRepository.GetById(Id);
                 await _postRepository.Delete(post);
            }

            return Redirect("/blogs");
        }
    }
}
