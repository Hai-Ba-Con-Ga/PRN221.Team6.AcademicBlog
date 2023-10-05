
using AcademicBlog.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(opt =>
{
    opt.Conventions.AddPageRoute("/Auth/Login", "/login");
});

builder.Services.AddDbContext<AcademicBlogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AcademicBlogDB"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
