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
        private  int AccountId { get; set; }
        

        public List<TabItem> Tabs { get; set; }
        public IndexModel(IPostRepository postRepository, IBookmarkRepository bookmarkRepository)
        {
            _postRepository = postRepository;
            _bookmarkRepository = bookmarkRepository;
            AccountId = Convert.ToInt32(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            Tabs = new List<TabItem>{
                new TabItem { Text = "Lastest", Key = "lastest"},
            };
            if(AccountId > 0)
            {
                Tabs.Add(new TabItem { Text = "Following", Key = "following" });
                Tabs.Add(new TabItem { Text = "Bookmark", Key = "bookmark" });
                Tabs.Add(new TabItem { Text = "Pending Publication", Key = "pending" });
               
            }
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
                        var followerIds = new List<int> { 2 };
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
