using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BookingSystem.Services;

namespace BookingSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            //Connection string
            builder.Services.AddDbContext<BookingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            //Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<BookingContext>()
            .AddDefaultTokenProviders();

            //DummyEmailSender
            builder.Services.AddTransient<IEmailSender, DummyEmailSender>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddRazorPages();

            builder.Services.AddSession();//Add session services
            builder.Services.AddDistributedMemoryCache(); // For storing session in memory


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<BookingContext>();
                DbInitializer.Initalize(context);
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roleNames = { "Admin", "Guest" };
                foreach (var roleName in roleNames)
                {
                    var roleExists = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
