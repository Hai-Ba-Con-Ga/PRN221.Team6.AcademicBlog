using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using AcademicBlog.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AcademicBlog.Pages.Blogs
{

    public class IndexModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IFollowingRepository _followingRepository;
        private int AccountId { get; set; }


        public List<TabItem> Tabs { get; set; }
        public IndexModel(IPostRepository postRepository, IBookmarkRepository bookmarkRepository, IFollowingRepository followingRepository)
        {
            _postRepository = postRepository;
            _bookmarkRepository = bookmarkRepository;
            _followingRepository = followingRepository;
            Tabs = new List<TabItem>{
                new TabItem { Text = "Lastest", Key = "lastest"},
            };

        }
        [FromQuery(Name = "tab")]
        public string Tab { get; set; }
        [FromQuery(Name = "searchKeyword")]
        public string SearchKeyword { get; set; } = "";
        [FromQuery]

        public PaginationParams Paging { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        //get data switch tab and paging with search name
        public async Task<IActionResult> OnGetAsync()
        {
            AccountId = Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            if (AccountId > 0)
            {
                Tabs.Add(new TabItem { Text = "Following", Key = "following" });
                Tabs.Add(new TabItem { Text = "Bookmark", Key = "bookmark" });
                Tabs.Add(new TabItem { Text = "Pending Publication", Key = "mypending" });

            }
            if (AccountId > 0 && User.IsInRole("Mod"))
            {
                Tabs.Add(new TabItem { Text = "Approve", Key = "approve" });
                Tabs.Add(new TabItem { Text = "Pending", Key = "pending" });
                Tabs.Add(new TabItem { Text = "Reject", Key = "reject" });
            }
            var pagable = new Pagable()
            {
                PageIndex = Paging?.Page ?? 1,
                PageSize = Paging?.PageSize ?? 10,
                Sort = new List<Sort>()
                        {
                            new()
                            {
                                Field= "CreatedDate",
                                Dir = "DESC"
                            }
                        },
            };
            var searchFilter = new Filter()
            {
                Logic = FilterLogic.OR,
                Filters = new List<Filter>
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
            };
            //get name identity
            switch (Tab)
            {
                case "following":
                    {
                        //TODO : get from Following table
                        var followings = (await _followingRepository.Find(f => f.FollowerId == AccountId)).ToList();
                        var followerIds = new List<int>();
                        if (followings.Count > 0)
                        {
                            followings.Select(
                                f => f.FollowingId
                            ).ToList().ForEach(f => followerIds.Add(f));
                        }
                        else
                        {
                            followerIds.Add(-1);
                        }


                        var followingFilter = new Filter()
                        {
                            Field = "CreatorId",
                            Operator = FilterOps.IN,
                            Value = followerIds
                        };
                        var commonFilter = new Filter()
                        {
                            Logic = FilterLogic.AND,
                            Filters = new List<Filter>()
                            {
                                new ()
                                {
                                    Field = "IsPublic",
                                    Operator = "eq",
                                    Value = true
                                },
                                searchFilter,
                                followingFilter
                            }
                        };
                        pagable.Filter = commonFilter;

                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "bookmark":
                    Posts = (await _bookmarkRepository.GetAll(AccountId, SearchKeyword ?? "", Paging.Page, Paging.PageSize)).Select(x => x.Post);
                    break;
                case "mypending":
                    {

                        var filter = new Filter()
                        {
                            Logic = FilterLogic.AND,
                            Filters = new List<Filter>()
                        {
                            new ()
                            {
                                Field = "IsPublic",
                                Operator = "eq",
                                Value = false
                            },
                            new()
                            {
                                Field = "ApproverID",
                                Operator = "eq",
                                Value = null
                            },
                            new Filter()
                            {
                                Field = "CreatorId",
                                Operator = "eq",
                                Value = AccountId
                            }
                        }
                        };
                        pagable.Filter = filter;
                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "approve":
                    {
                        var filter = new Filter()
                        {
                            Logic = FilterLogic.AND,
                            Filters = new List<Filter>()
                        {
                            new ()
                            {
                                Field = "IsPublic",
                                Operator = "eq",
                                Value = true
                            },
                            new()
                            {
                                Field = "Status",
                                Operator = "neq",
                                Value = 1
                            },
                            new()
                            {
                                Field = "ApproverID",
                                Operator = "eq",
                                Value = AccountId
                            },
                        }
                        };
                        pagable.Filter = filter;
                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "pending":
                    {
                        var filter = new Filter()
                        {
                            Logic = FilterLogic.AND,
                            Filters = new List<Filter>()
                        {
                            new ()
                            {
                                Field = "IsPublic",
                                Operator = "eq",
                                Value = false
                            },
                            new()
                            {
                                Field = "Status",
                                Operator = "eq",
                                Value = 0
                            },
                            //TODO: Check post skill
                        }
                        };
                        pagable.Filter = filter;
                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "reject":
                    {
                        var filter = new Filter()
                        {
                            Logic = FilterLogic.AND,
                            Filters = new List<Filter>()
                        {
                            new ()
                            {
                                Field = "IsPublic",
                                Operator = "eq",
                                Value = false
                            },
                            new()
                            {
                                Field = "Status",
                                Operator = "eq",
                                Value = 2
                            },
                            new()
                            {
                                Field = "ApproverID",
                                Operator = "eq",
                                Value = AccountId
                            }
                        }
                        };
                        pagable.Filter = filter;
                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                default:
                    {

                        var lastestFilter = new Filter()
                        {
                            Logic = FilterLogic.AND,
                            Filters = new List<Filter>()
                        {
                            new ()
                            {
                                Field = "IsPublic",
                                Operator = "eq",
                                Value = true
                            },
                            searchFilter
                        }
                        };
                        pagable.Filter = lastestFilter;

                        Posts = await _postRepository.GetAllPost(pagable);
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
            }
            return Page();
        }


        //get data with search name
        //public async Task<IActionResult> OnGetLoadDataByNameAsync(int page, int pageSize)
        //{
        //    Posts = await _postRepository.GetAllFrontByName(page, pageSize, SearchKeyword);
        //    return Partial("_PostList", this);
        //}

    }
}
