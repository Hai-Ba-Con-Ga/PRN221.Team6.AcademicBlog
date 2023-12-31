using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository.Interface;
using AcademicBlog.Repository;
using AcademicBlog.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AcademicBlog.Pages.Profile
{
    public class ProfileMeModel : PageModel
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

        public IEnumerable<Account> FollowingRelationAccounts { get; set; }

        public ProfileMeModel(IPostRepository postRepository, IBookmarkRepository bookmarkRepository, IFollowingRepository followingRepository, IAccountRepository accountRepository)
        {

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
            if (AccountId >= 0)
            {
                Id = AccountId;
                Account = await _accountRepository.GetById((int)Id);
            }else
            {
                return Redirect($"/Auth/Login?returnUrl=/profile/me");
            }


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
    }
}
