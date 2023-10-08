using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AcademicBlog.Domain.Entities;
using AcademicBlog.Domain.Interfaces;
using AcademicBlog.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace AcademicBlog.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IRepository<Account> accountRepository, ILogger<LoginModel> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
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
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public IActionResult OnGetAsync(string? returnUrl = null)
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("../Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine(Input.Email);
            Console.WriteLine(Input.Password);
            if (!ModelState.IsValid)
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToPage("../Index");
                }
            }
            var account = await _accountRepository.GetByConditionAsync(x => x.Email == Input.Email && x.Password == Input.Password);
            
            if (account == null)
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Fullname),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                IsPersistent = Input.RememberMe
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToPage("../Index");
        }
    }
}
