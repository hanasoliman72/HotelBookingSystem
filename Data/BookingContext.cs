using BookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Data
{
    public class BookingContext : IdentityDbContext<ApplicationUser>
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

        public DbSet<EndUser> endUsers { get; set; }
        public DbSet<Guest> guests { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<RoomClass> roomClasses { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }

        // Seed roles and initial user data
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed RoomClass data
            modelBuilder.Entity<RoomClass>().HasData(
                new RoomClass { ID = 1, Type = "Deluxe", Description = "A spacious deluxe room with sea view.", NumberOfBeds = 2 },
                new RoomClass { ID = 2, Type = "Standard", Description = "A cozy standard room perfect for solo travelers.", NumberOfBeds = 1 },
                new RoomClass { ID = 3, Type = "Suite", Description = "Luxurious suite with a private living area.", NumberOfBeds = 3 }
            );

            // Seed Room data
            modelBuilder.Entity<Room>().HasData(
                new Room { ID = 1, RoomClassID = 1, Floor = 1, Price = 120.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room1.jpg", View = "Sea View", Status = RoomStatus.Available },
                new Room { ID = 2, RoomClassID = 2, Floor = 2, Price = 90.00m, Rate = 3, ImageUrl = "/assets/img/rooms/room2.jpg", View = "City View", Status = RoomStatus.Occupied },
                new Room { ID = 3, RoomClassID = 1, Floor = 1, Price = 150.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room3.jpg", View = "Sea View", Status = RoomStatus.Available },
                new Room { ID = 4, RoomClassID = 2, Floor = 3, Price = 75.00m, Rate = 2, ImageUrl = "/assets/img/rooms/room4.jpg", View = "Garden View", Status = RoomStatus.Maintenance },
                new Room { ID = 5, RoomClassID = 3, Floor = 4, Price = 200.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room5.jpg", View = "Mountain View", Status = RoomStatus.Available },
                new Room { ID = 6, RoomClassID = 2, Floor = 5, Price = 250.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room6.jpg", View = "Mountain View", Status = RoomStatus.Available }
            );
        }

        // Seed roles method
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "SuperAdmin", "Admin", "User" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        //Seed default SuperAdmin user
        public static async Task SeedDefaultSuperAdminUserAsync(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync("superadmin@example.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "superadmin@example.com",
                    Email = "superadmin@example.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    PhoneNumber = "1234567890"
                };

                var result = await userManager.CreateAsync(user, "SuperAdminPassword!123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Seed(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Your_Connection_String_Here", options =>
                        options.CommandTimeout(180));  // Set command timeout to 180 seconds
            }
        }
    }
}
