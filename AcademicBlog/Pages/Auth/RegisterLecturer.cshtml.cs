
using AcademicBlog.BussinessObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademicBlog.Pages.Auth
{
    public class RegisterLecturerModel : PageModel
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterLecturerModel(IAccountRepository accountRepository, IRoleRepository roleRepository, ISkillRepository skillRepository, ILogger<RegisterModel> logger)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _skillRepository = skillRepository;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();




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
            public List<int> Skills { get; set; } = new List<int>();

        }
        
        

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("../Index");
            }
            ViewData["ReturnUrl"] = returnUrl;
            Skills = (await _skillRepository.GetAll()).ToList();
            
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToPage("../Index");
                }
                Skills = (await _skillRepository.GetAll()).ToList();
                return Page();
            }
            try
            {
                var role = await _roleRepository.GetByName("Mod");
                if (role == null)
                {
                    throw new Exception("Role not found");
                }
                //check email
                var isExist = await _accountRepository.GetByEmail(Input.Email);
                if (isExist != null)
                {
                    throw new Exception("Email already exist");
                }
                var account = new Account { Email = Input.Email, Password = Input.Password, Fullname = Input.Fullname, RoleId = role.Id };
                var skills = await _skillRepository.GetByIds(Input.Skills);
                
                var result = await _accountRepository.Add(account);
                if (result == null)
                {
                    throw new Exception("Register failed");
                }
                result.Skills = skills.ToList();
                result.IsActive = false;
                await _accountRepository.Update(account);

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
