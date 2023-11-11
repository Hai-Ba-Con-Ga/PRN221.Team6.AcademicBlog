using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAccountRepository _accountRepository;

        public LoginModel(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = null!;

        }
        public IActionResult OnGetAsync([FromQuery] string? returnUrl = null)
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage(returnUrl ?? "/Index");
            }
            ReturnUrl = returnUrl ?? "/Index";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    return Redirect(ReturnUrl);
                }
            }
            var account = await _accountRepository.Login(Input.Email, Input.Password);

            if (account == null)
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }
            if (account.IsActive == false)
            {
                ErrorMessage = "Account pending approval. Please check your email again!";
                return Page();
            }
            
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Email),
                    new Claim("fullname", account.Fullname),
                    new Claim(ClaimTypes.Role, account.Role.Name),
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
                };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

           
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true,
            };
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            if (account.Role.Name.Equals("Admin"))
            {
                return Redirect("/admin/modrequest");
            }


            //but isAuthenticated false after login how to fix it

            return RedirectToPage(ReturnUrl);
        }
        
    }
}
