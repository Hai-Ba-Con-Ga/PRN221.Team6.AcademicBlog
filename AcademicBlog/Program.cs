
using AcademicBlog.BussinessObject;
using AcademicBlog.Repository;
using AcademicBlog.Repository.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
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

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AcademicBlogDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();



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
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
