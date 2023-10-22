using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IPostRepository postRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
        }
        // [BindProperty]
        public IEnumerable<Post> Posts { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            //get post
            //Posts = await _postRepository.GetAll(9);
            Posts = await _postRepository.GetAllPost(new()
            {
                PageIndex = 1,
                PageSize = 9,
                Sort = new List<Sort>
                    {
                        new Sort
                        {
                            Field = "CreatedDate",
                            Dir = "DESC"
                        }
                    },
                Filter = new Filter(){ 
                    Filters = new List<Filter>
                    {
                        new Filter()
                        {
                            Field = "IsPublic",
                            Operator = "eq",
                            Value = true,
                        },
                        /* Nested Object*/
                        //new Filter()
                        //{
                        //    Field = "Creator.Email",
                        //    Operator = "contains",
                        //    Value = "admin",
                        //},
                        /* In operator */

                        // new Filter()
                        //{
                        //    Field = "CreatorId",
                        //    Operator = "in",
                        //    Value = new List<int>{
                        //        7,1
                        //    },
                        //},
                        /* like */
                        //new Filter()
                        //{
                        //    Logic = "or",
                        //    Filters = new List<Filter>
                        //    {
                        //        new()
                        //        {
                        //            Field = "Title",
                        //            Operator = "contains",
                        //            Value = "awd"
                        //        },
                        //         new()
                        //        {
                        //            Field = "Content",
                        //            Operator = "contains",
                        //            Value = "awd"
                        //        }
                        //    }
                        //}

                    },
                    Logic = "and"
                }
            });
            return Page();
        }


    }
}