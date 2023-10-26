using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AcademicBlog.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CategoryMiddleware:IMiddleware
    {
     
        private readonly ICategoryRepository _categoryRepository;

        public CategoryMiddleware(ICategoryRepository categoryRepository)
        {
       
            _categoryRepository = categoryRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var categories = await _categoryRepository.GetAll(); // Replace with your data retrieval logic
            context.Items["Categories"] = categories;
            await next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CategoryMiddlewareExtensions
    {
        public static IApplicationBuilder UseCategoryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CategoryMiddleware>();
        }
    }
}
