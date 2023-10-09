
using AcademicBlog.BussinessObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(opt =>
{
    opt.Conventions.AddPageRoute("/Auth/Login", "/login");
});

builder.Services.AddDbContext<AcademicBlogDbContext>(options =>
{
    options.UseSqlServer("server =wyvernpserver.tech; database = AcademicBlogDB;uid=sa;pwd=ThanhPhong2506;TrustServerCertificate=True");
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//                .AddEntityFrameworkStores<AcademicBlogDbContext>()
//                .AddDefaultTokenProviders();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// seed data create role
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AcademicBlogDbContext>();
    var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == "Admin");
    if (role == null)
    {
        role = new Role
        {
            Name = "Admin"
        };
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();
    }
    role = await context.Roles.FirstOrDefaultAsync(x => x.Name == "User");
    if (role == null)
    {
        role = new Role
        {
            Name = "User"
        };
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();
    }
    // seed data create user
    var user = await context.Accounts.FirstOrDefaultAsync(x => x.Email == "admin@gmail.com");
    if (user == null)
    {
        user = new Account
        {
            Email = "admin@gmail.com",
            Password = "123456",
            Fullname = "Admin",
            RoleId = 1,
        };
        await context.Accounts.AddAsync(user);
        await context.SaveChangesAsync();
    }

    var category = await context.Categories.FirstOrDefaultAsync(x => x.Name == "Category 1");
    if (category == null)
    {
        category = new Category
        {
            Name = "Category 1",
        };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }

    var post = await context.Posts.FirstOrDefaultAsync(x => x.Title == "Post 1");
   
    if (post == null)
    {
        post = new Post
        {
            Title = "Post 1",
            Content = "Content 1",
            CreatorId = 2,
            CategoryId = 1,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            IsPublic = true,
            ThumbnailUrl = "https://kenh14cdn.com/thumb_w/660/203336854389633024/2021/10/5/059f8009221cda21281fd9551614a2b7-1633406035772546090167.jpg",

        };
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();


app.MapRazorPages();

app.Run();
