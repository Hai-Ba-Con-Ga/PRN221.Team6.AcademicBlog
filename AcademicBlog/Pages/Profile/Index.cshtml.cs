using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository.Interface;
using AcademicBlog.Repository;
using AcademicBlog.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Security.Claims;
using AcademicBlog.BussinessObject.PagingObject;
using Microsoft.EntityFrameworkCore;

namespace AcademicBlog.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IFollowingRepository _followingRepository;
        private readonly IAccountRepository _accountRepository;


        public List<TabItem> Tabs { get; set; }
        [FromQuery(Name = "tab")]
        public string Tab { get; set; }
        private int AccountId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }
        [FromQuery(Name = "searchKeyword")]
        public string SearchKeyword { get; set; } = "";
        [FromQuery]

        public PaginationParams Paging { get; set; }

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();

        public Account Account { get; set; } = new Account();

        public IEnumerable<Account> FollowingRelationAccounts{get;set;}
        public bool IsDisplayFollowingButton { get; set; } = true;
        public bool IsFollowed { get; set; } = true;

        public IndexModel(IPostRepository postRepository, IBookmarkRepository bookmarkRepository, IFollowingRepository followingRepository, IAccountRepository accountRepository) {

            Tabs = new List<TabItem>{
                new TabItem { Text = "Blogs", Key = "blogs"},
            };
            Tabs.Add(new TabItem { Text = "Following", Key = "following" });
            Tabs.Add(new TabItem { Text = "Followers", Key = "follower" });
            Tabs.Add(new TabItem { Text = "Tag", Key = "tag" });
            Tabs.Add(new TabItem { Text = "Contact", Key = "contact" });
            _postRepository = postRepository;
            _bookmarkRepository = bookmarkRepository;
            _followingRepository = followingRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            AccountId = Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            if (Id is not null)
            {
                if(Id == AccountId)
                {
                    return Redirect("/profile/me");
                }
                Account = await _accountRepository.GetById((int)Id);
            }

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
                                Value = Id
                            }
                        }
                }

            })).FirstOrDefault();
            if (AccountId < 0 || Id == AccountId)
            {
                IsDisplayFollowingButton = false;
            }
            if (followingConnection is null) { IsFollowed = false; }




            switch (Tab)
            {
                case "blogs":
                    {
                        Pagable pagable = new()
                        {
                            PageIndex = Paging.Page,
                            PageSize = Paging.PageSize,
                            Sort = new List<Sort>()
                        {
                            new()
                            {
                                Field = "CreatedDate",
                                Dir = "DESC"
                            }
                        },
                            Filter = new()
                            {
                                Logic = "and",
                                Filters = new List<Filter>()
                            {
                                new()
                                {
                                    Field = "IsPublic",
                                    Operator= "eq",
                                    Value = true
                                },
                                new()
                                {
                                    Field = "CreatorId",
                                    Operator = "eq",
                                    Value = Id
                                },
                                new()
                                {
                                    Logic = "or",
                                    Filters = new List<Filter>()
                                    {
                                         new()
                                        {
                                            Field = "Title",
                                            Operator = "contains",
                                            Value = SearchKeyword?? ""
                                        },
                                         new()
                                        {
                                            Field = "Content",
                                            Operator = "contains",
                                            Value = SearchKeyword ?? ""
                                        }
                                    }
                                }
                            }
                            }
                        };
                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "following":
                    {
                        Pagable pagable = new()
                        {
                            PageIndex = Paging.Page,
                            PageSize = 10,
                            Filter = new Filter()
                            {
                                Logic = "and",
                                Filters = new List<Filter>()
                                {
                                    new Filter()
                                    {
                                        Field = "FollowerId",
                                        Operator = "eq",
                                        Value = Id
                                    }
                                }
                            }
                        };
                        var following = await _followingRepository.GetList(pagable, f => f.Include(f => f.FollowingNavigation));
                        FollowingRelationAccounts = following.Select(f => f.FollowingNavigation);
                        var count = await _followingRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "follower":
                    {
                        Pagable pagable = new()
                        {
                            PageIndex = Paging.Page,
                            PageSize = 10,
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
                        var follower = await _followingRepository.GetList(pagable, f => f.Include(f => f.Follower));
                        FollowingRelationAccounts = follower.Select(f => f.Follower);
                        var count = await _followingRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "tag":
                    {
                        break;
                    }
                case "contact":
                    {
                        break;
                    }
                default:
                    
                        {
                            Pagable pagable = new()
                            {
                                PageIndex = Paging.Page,
                                PageSize = Paging.PageSize,
                                Sort = new List<Sort>()
                        {
                            new()
                            {
                                Field = "CreatedDate",
                                Dir = "DESC"
                            }
                        },
                                Filter = new()
                                {
                                    Logic = "and",
                                    Filters = new List<Filter>()
                            {
                                new()
                                {
                                    Field = "IsPublic",
                                    Operator= "eq",
                                    Value = true
                                },
                                new()
                                {
                                    Field = "CreatorId",
                                    Operator = "eq",
                                    Value = Id
                                },
                                new()
                                {
                                    Logic = "or",
                                    Filters = new List<Filter>()
                                    {
                                         new()
                                        {
                                            Field = "Title",
                                            Operator = "contains",
                                            Value = SearchKeyword?? ""
                                        },
                                         new()
                                        {
                                            Field = "Content",
                                            Operator = "contains",
                                            Value = SearchKeyword ?? ""
                                        }
                                    }
                                }
                            }
                                }
                            };
                            Posts = await _postRepository.GetAllPost(pagable);
                            var count = await _postRepository.CountList(pagable);
                            Paging.Total = count.TotalCount;
                            Paging.PageCount = count.TotalPage;
                            break;
                        }
                    
            }
          

            return Page();
        }
        [BindProperty]

        public string ActionFollow { get; set; } //follow / unfollow
        [BindProperty]

        public int CreatorId { get; set; }
        public async Task<IActionResult> OnPostFollowAsync()
        {
            var accountId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "-1");
            if (accountId < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/Profile/{Id}");
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
            return await OnGetAsync();
        }
    }
}
