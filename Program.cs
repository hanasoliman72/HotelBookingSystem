using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookingSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BookingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddRazorPages();

            // ✅ Add full Identity (with roles and email confirmation)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<BookingContext>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            // ✅ Seed roles and SuperAdmin user
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Seed roles
                await BookingContext.SeedRolesAsync(scope.ServiceProvider, roleManager);

                // Seed SuperAdmin user (if first user)
                await BookingContext.SeedDefaultSuperAdminUserAsync(scope.ServiceProvider, userManager);
            }

            // ✅ Seed other database data (rooms, room classes, etc.)
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<BookingContext>();
                DbInitializer.Initalize(context);
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Must come before UseAuthorization
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            app.Run();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Ensure roles are created on application startup
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
            {
                var role = new IdentityRole("SuperAdmin");
                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                var role = new IdentityRole("User");
                roleManager.CreateAsync(role).Wait();
            }
        }

    }
}
