using AcademicBlog.BussinessObject;
using AcademicBlog.Pages.Blogs.Component;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages.Blogs
{

    public class IndexModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly IBookmarkRepository _bookmarkRepository;


        public List<TabItem> Tabs { get; set; }
        public IndexModel(IPostRepository postRepository, IBookmarkRepository bookmarkRepository)
        {
            _postRepository = postRepository;
            _bookmarkRepository = bookmarkRepository;
            Tabs = new List<TabItem>{
                new TabItem { Text = "Lastest", Key = "lastest"},
                new TabItem { Text = "Following", Key = "following" },
                new TabItem { Text = "Bookmark", Key = "bookmark" }
            };
        }

        [BindProperty]
        public string SearchKeyword { get; set; }
        public IEnumerable<Post> Posts { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        //get data switch tab and paging with search name
        public async Task<IActionResult> OnGetAsync(string tab, int page = 1, int pageSize = 9, string keyword = "")
        {
            //get name identity
            switch (tab)
            {
                case "lastest":
                    Posts = await _postRepository.GetAllFrontByName(page, pageSize, keyword);
                    break;
                case "trending":
                    //Posts = await _postRepository.GetAllFrontByName(page, pageSize, keyword);
                    break;
                case "following":
                    //Posts = await _postRepository.GetAllFrontByName(page, pageSize, keyword);
                    break;
                case "bookmark":
                    var accountId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id").Value);
                    Posts = (await _bookmarkRepository.GetAll(accountId, keyword, page, pageSize)).Select(x => x.Post);
                    break;
                default:
                    Posts = await _postRepository.GetAllFrontByName(page, pageSize, keyword);
                    break;
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
