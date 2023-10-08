
using AcademicBlog.BussinessObject;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AcademicBlog.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(IAccountRepository accountRepository, IRoleRepository roleRepository, ILogger<RegisterModel> logger)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

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

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = null!;

            [Required]
            public string Fullname { get; set; } = null!;
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
            Console.WriteLine(Input.Fullname);
            if (!ModelState.IsValid)
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToPage("../Index");
                }
                return Page();
            }
            try
            {
                var role = await _roleRepository.GetByName("User");
                if (role == null)
                {
                    throw new Exception("Role not found");
                }
                var account = new Account { Email = Input.Email, Password = Input.Password, Fullname = Input.Fullname, RoleId = role.Id };
                var result = await _accountRepository.Add(account);
                if (result == null)
                {
                    throw new Exception("Register failed");
                }
                return RedirectToPage("/Auth/Login");
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                _logger.LogError(e.Message);
                return Page();
            }
        }

    }
}
