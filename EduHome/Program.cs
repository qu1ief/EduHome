using EduHome.Contexts;
using EduHome.Models;
using EduHome.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduHome;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();


        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<ILayoutService, LayoutService>();

        builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = false;

            opt.SignIn.RequireConfirmedEmail = true;
            opt.Lockout.AllowedForNewUsers = false;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            opt.Lockout.MaxFailedAccessAttempts = 3;

        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


        var app = builder.Build();


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );
        });


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}