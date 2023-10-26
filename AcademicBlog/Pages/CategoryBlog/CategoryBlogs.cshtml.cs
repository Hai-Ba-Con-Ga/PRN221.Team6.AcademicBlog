using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository.Interface;
using AcademicBlog.Repository;
using AcademicBlog.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademicBlog.BussinessObject.PagingObject;
using Azure;

namespace AcademicBlog.Pages.CategoryBlog
{
    public class CategoryBlogsModel : PageModel

    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryBlogsModel(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;


        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [FromQuery(Name = "searchKeyword")]
        public string SearchKeyword { get; set; } = "";
        [FromQuery]

        public PaginationParams Paging { get; set; }

        public IEnumerable<Post> Posts { get; set; }
        public Category Category { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Category = (await _categoryRepository.Find(c => c.Id == Id)).FirstOrDefault();
            if (Category is not null)
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
                                Field = "CategoryId",
                                Operator = "eq",
                                Value = Category.Id
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
            }
            return Page();
        }
    }
}
