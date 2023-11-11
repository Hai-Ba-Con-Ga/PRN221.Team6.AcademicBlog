using AcademicBlog.BussinessObject;
using AcademicBlog.BussinessObject.PagingObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using AcademicBlog.Utils;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AcademicBlog.Pages.Admin.Account
{
    public class ModRequestModel : PageModel
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;
        public IEnumerable<BussinessObject.Account> RequestAccount { get; set; }
        public ModRequestModel (IAccountRepository accountRepository, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _accountRepository = accountRepository;
        }
        [FromQuery(Name = "searchKeyword")]
        public string SearchKeyword { get; set; } = "";
        [FromQuery]
        public PaginationParams Paging { get; set; }

        [BindProperty]
        public int AccountId { get; set; }

        [BindProperty]
        public string ApprovalAction { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            const string modRoleName = "Mod";
            int roleId = (await _roleRepository.GetByName (modRoleName))?.Id  ?? -1;
            if(roleId < 0)
            {
                return Redirect("/admin");
            }

            Pagable pagable = new()
            {
                PageIndex = Paging.Page,
                PageSize = 9,
                Filter = new()
                {
                    Logic = FilterLogic.AND,
                    Filters = new List<Filter>()
                    {
                        new()
                        {
                            Field = "RoleId",
                            Operator = "eq",
                            Value = roleId
                        },
                        new()
                        {
                            Field = "ApproverId",
                            Operator = "eq",
                            Value = null
                        },
                        new()
                        {
                            Logic = "OR",
                            Filters = new List<Filter>()
                            {
                                new Filter()
                                {
                                    Field = "Email",
                                    Operator = "contains",
                                    Value = SearchKeyword
                                },
                                new Filter()
                                {
                                    Field = "Fullname",
                                    Operator = "contains",
                                    Value = SearchKeyword
                                },
                                new Filter()
                                {
                                    Field = "Username",
                                    Operator = "contains",
                                    Value = SearchKeyword
                                },
                            }
                        },
                    }
                }
            };

            var (accounts, count) = await _accountRepository.GetListWithPaging(pagable, a => a.Include(a => a.Skills));
            RequestAccount = accounts;
            Paging.Total = count.TotalCount;
            Paging.PageCount = count.TotalPage;
            return Page();
        }
        public async Task<IActionResult> OnPostApproval()
        {
            var id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            if(id < 0)
            {
                return Redirect($"/Auth/Login?returnUrl=/admin/modrequest");
            }
            switch (ApprovalAction)
            {
                case "approve":
                    {
                        
                        var account = await _accountRepository.GetById(AccountId);
                        if (account is not null)
                        {
                            account.IsActive = true;
                            account.ApproverId = id;
                            account.ApproveDate = DateTime.Now;
                            await _accountRepository.Update(account);
                        }
                        string emailTemplate = Utils.Utils.SystemApprovalEmail(true);
                        var body = JsonSerializer.Serialize(new
                        {
                            to = account?.Email, // Replace with the actual email address
                            content = emailTemplate
                        });
                        using (var client = new HttpClient())
                        {
                            var requestContent = new StringContent(body, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("http://notification.wyvernpserver.tech/mail/send", requestContent);
                        }
                        break;
                    }
                case "reject":
                    {
                        var account = await _accountRepository.GetById(AccountId);
                        if (account is not null)
                        { 
                            await _accountRepository.Delete(account);
                        }
                        string emailTemplate = Utils.Utils.SystemApprovalEmail(true);
                        var body = JsonSerializer.Serialize(new
                        {
                            to = account?.Email, // Replace with the actual email address
                            content = emailTemplate
                        });
                        using (var client = new HttpClient())
                        {
                            var requestContent = new StringContent(body, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("http://notification.wyvernpserver.tech/mail/send", requestContent);
                        }
                        break;
                    }
            }
            return await OnGetAsync();
        }
    }
}
