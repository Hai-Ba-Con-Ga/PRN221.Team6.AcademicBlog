
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace AcademicBlog.Pages.Admin.Account
{
    public class ManageAccountModel : PageModel
    {
        [FromQuery(Name = "keyword")]
        public string? Keyword { get; set; } = "";
        [FromQuery(Name = "page")]
        public int PageIndex { get; set; } = 1;
        [BindProperty]
        public int? PageSize { get; set; } = 5;

        [BindProperty]
        public string SortBy { get; set; }

        [BindProperty]
        public string Direction { get; set; }
        public int totalCar { get; set; }

        private IAccountRepository accountRepo;
        public List<AcademicBlog.BussinessObject.Account> Accounts { get; set; } = new List<AcademicBlog.BussinessObject.Account>();
        public ManageAccountModel(IAccountRepository accountRepository)
        {
            accountRepo = accountRepository;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var pageable = new Pagable()
            {
                PageIndex = PageIndex,
                PageSize = PageSize ?? 10,
                Filter = new()
                {
                    Field = "Title",
                    Operator = "contains",
                    Value = Keyword??""
                }
            };
            Accounts = (await accountRepo.GetList(pageable, a => a.Include(a => a.Skills).Include(a => a.Role))).ToList();
            var count = await accountRepo.CountList(pageable);
            totalCar = count.TotalCount;
            //Paging.PageCount = count.TotalPage;
            return Page();

        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string ActionBan { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var account = await accountRepo.GetById(Id);
            if(account is not null)
            {
                if (ActionBan.Equals("ban"))
                {
                    account.IsActive = false;
                }else
                {
                    account.IsActive = true;
                }
                await accountRepo.Update(account);
            }
            return await OnGetAsync();

        }
    }
}
