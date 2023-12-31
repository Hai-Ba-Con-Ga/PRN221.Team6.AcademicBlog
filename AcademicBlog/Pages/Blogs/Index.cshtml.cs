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
        private readonly ISkillRepository _skillRepository;
        private readonly IAccountRepository _accountRepository;
        private int AccountId { get; set; }


        public List<TabItem> Tabs { get; set; }
        public IndexModel(IPostRepository postRepository, IBookmarkRepository bookmarkRepository, IFollowingRepository followingRepository, ISkillRepository skillRepository, IAccountRepository accountRepository)
        {
            _postRepository = postRepository;
            _bookmarkRepository = bookmarkRepository;
            _followingRepository = followingRepository;
            _skillRepository = skillRepository;
            _accountRepository = accountRepository;
            Tabs = new List<TabItem>{
                new TabItem { Text = "Lastest", Key = "lastest"},
            };
        }
        [FromQuery(Name = "tab")]
        public string Tab { get; set; }
        [FromQuery(Name = "searchKeyword")]
        public string SearchKeyword { get; set; } = "";
        [FromQuery(Name = "category")]
        public List<int> CategoryIds { get; set; } = new List<int>();
        [FromQuery]

        public PaginationParams Paging { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

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
                Tabs.Add(new TabItem { Text = "My pending", Key = "mypending" });

            }
            if (AccountId > 0 && User.IsInRole("Mod"))
            {
                Tabs.Add(new TabItem { Text = "Approved", Key = "approve" });
                Tabs.Add(new TabItem { Text = "Request", Key = "pending" });
                Tabs.Add(new TabItem { Text = "Rejected", Key = "reject" });
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
                            },
                           
                        }
            };
            Filter categoryFilter = null;
            if(CategoryIds.Count > 0)
            {
                categoryFilter = new()
                {
                    Field = "CategoryId",
                    Operator = "in",
                    Value = CategoryIds
                };
            }
            //get name identity
            switch (Tab)
            {
                case "following":
                    {
                      
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
                                followingFilter,
                            }
                        };
                        pagable.Filter = commonFilter;
                         if (categoryFilter is not null)
                        {
                            var filterList = pagable.Filter.Filters.ToList();
                            filterList.Add(categoryFilter);
                            pagable.Filter.Filters = filterList;
                        }
                        Posts = (await _postRepository.GetAllPost(pagable)).ToList();
                        var count = await _postRepository.CountList(pagable);
                        Paging.Total = count.TotalCount;
                        Paging.PageCount = count.TotalPage;
                        break;
                    }
                case "bookmark":
                    Posts = ((await _bookmarkRepository.GetAll(AccountId, SearchKeyword ?? "", Paging.Page, Paging.PageSize)).Select(x => x.Post)).ToList();
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
                         if (categoryFilter is not null)
                        {
                            var filterList = pagable.Filter.Filters.ToList();
                            filterList.Add(categoryFilter);
                            pagable.Filter.Filters = filterList;
                        }
                        pagable.Filter = filter;
                        Posts = (await _postRepository.GetAllPost(pagable)).ToList();
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
                                Operator = "eq",
                                Value = 1
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
                        if (categoryFilter is not null)
                        {
                            var filterList = pagable.Filter.Filters.ToList();
                            filterList.Add(categoryFilter);
                            pagable.Filter.Filters = filterList;
                        }
                        Posts = (await _postRepository.GetAllPost(pagable)).ToList();
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
                            new()
                            {
                                Field = "CreatorId",
                                Operator = "neq",
                                Value = AccountId
                            }
                        }
                        };
                        pagable.Filter = filter;
                         if (categoryFilter is not null)
                        {
                            var filterList = pagable.Filter.Filters.ToList();
                            filterList.Add(categoryFilter);
                            pagable.Filter.Filters = filterList;
                        }
                        var posts = await _postRepository.GetAllPost(pagable);
                        var accountSkillIds = (await _accountRepository.GetSkillById(AccountId)).Skills.Select(s => s.Id).ToList();
                        foreach (var post in posts)
                        {
                            var postSkillIds = post.Skills.Select(s => s.Id).ToList();
                            if (postSkillIds.Intersect(accountSkillIds).Count() != 0)
                            {
                                Posts.Add(post);
                            }
                        }
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
                        if (categoryFilter is not null)
                        {
                            var filterList = pagable.Filter.Filters.ToList();
                            filterList.Add(categoryFilter);
                            pagable.Filter.Filters = filterList;
                        }
                        Posts = (await _postRepository.GetAllPost(pagable)).ToList();
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
                            searchFilter,
                        }
                        };
                        pagable.Filter = lastestFilter;
                        if (categoryFilter is not null)
                        {
                            var filterList = pagable.Filter.Filters.ToList();
                            filterList.Add(categoryFilter);
                            pagable.Filter.Filters = filterList;
                        }
                        Posts = (await _postRepository.GetAllPost(pagable)).ToList();
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
