namespace AcademicBlog.Middleware
{
    public class AuthMiddleware
    {

        private readonly RequestDelegate _next;


        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the current request is for an authentication-related page
            if (IsAuthenticationPage(context.Request.Path))
            {
                // If it's an authentication page, pass the request to the next middleware
                await _next(context);
            }
            else
            {
                // Check if the user is authenticated
                if (context.User.Identity.IsAuthenticated)
                {
                    // Check the user's role
                    if (IsAdminPage(context.Request.Path))
                    {
                        //if (!context.User.IsInRole("Mod"))
                        //{
                        //    context.Response.Redirect("/403");
                        //}
                    }
                    else if (IsCustomerPage(context.Request.Path))

                    {
                        //if (!context.User.IsInRole(IndexModel.IdentityRole.CUSTOMER.ToString()))
                        //{
                        //    context.Response.Redirect("/403");
                        //}
                    }
                    await _next(context);

                }
                else
                {
                    // If not authenticated, redirect to the login page or perform any other action
                    context.Response.Redirect("/"); // Replace with your login page path
                }
            }
        }

        private bool IsAuthenticationPage(PathString path)
        {
            // Define a list of paths that should not require authentication
            var authenticationPages = new[]
            {
                "/login",
                "/register",
                "/logout",
                "/auth",
                "/403"

                // Add more authentication-related pages as needed
            };

            return Array.Exists(authenticationPages, page => path.StartsWithSegments(page, StringComparison.OrdinalIgnoreCase)) || path.ToString().Trim().Equals("/");
        }
        private bool IsAdminPage(PathString path)
        {
            // Define a list of paths that should not require authentication
            var authenticationPages = new[]
            {
                "/admin",

                // Add more authentication-related pages as needed
            };

            return Array.Exists(authenticationPages, page => path.StartsWithSegments(page, StringComparison.OrdinalIgnoreCase));
        }
        private bool IsCustomerPage(PathString path)
        {
            // Define a list of paths that should not require authentication
            var authenticationPages = new[]
            {
                "/customer",

                // Add more authentication-related pages as needed
            };

            return Array.Exists(authenticationPages, page => path.StartsWithSegments(page, StringComparison.OrdinalIgnoreCase));
        }
    }
}
