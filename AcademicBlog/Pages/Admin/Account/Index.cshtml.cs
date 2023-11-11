using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AcademicBlog.Pages.Admin.Account
{
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            //return Redirect("/admin/modrequest");
            return Page();
        }
    }
}
