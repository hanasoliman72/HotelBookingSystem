using BookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Data
{
    public class BookingContext : IdentityDbContext<ApplicationUser>
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

        public DbSet<RoomClass> roomClasses { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Feedback> feedbacks { get; set; }

        // Seed roles and initial user data
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed RoomClass data
            modelBuilder.Entity<RoomClass>().HasData(
                new RoomClass { ID = 1, Type = RoomType.Deluxe, Description = "A spacious deluxe room with sea view.", NumberOfBeds = 2 },
                new RoomClass { ID = 2, Type = RoomType.Standard, Description = "A cozy standard room perfect for solo travelers.", NumberOfBeds = 1 },
                new RoomClass { ID = 3, Type = RoomType.Suite, Description = "Luxurious suite with a private living area.", NumberOfBeds = 3 }
            );

            // Seed Room data
            modelBuilder.Entity<Room>().HasData(
                new Room { ID = 1, RoomClassID = 1, Floor = 1, Price = 120.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room1.jpg", View = "Sea View", Status = RoomStatus.Available,PreviousStatus = RoomStatus.Available },
                new Room { ID = 2, RoomClassID = 2, Floor = 2, Price = 90.00m, Rate = 3, ImageUrl = "/assets/img/rooms/room2.jpg", View = "City View", Status = RoomStatus.Occupied , PreviousStatus = RoomStatus.Available },
                new Room { ID = 3, RoomClassID = 1, Floor = 1, Price = 150.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room3.jpg", View = "Sea View", Status = RoomStatus.Available, PreviousStatus = RoomStatus.Maintenance },
                new Room { ID = 4, RoomClassID = 2, Floor = 3, Price = 75.00m, Rate = 2, ImageUrl = "/assets/img/rooms/room4.jpg", View = "Garden View", Status = RoomStatus.Maintenance, PreviousStatus = RoomStatus.Occupied },
                new Room { ID = 5, RoomClassID = 3, Floor = 4, Price = 200.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room5.jpg", View = "Mountain View", Status = RoomStatus.Available, PreviousStatus = RoomStatus.Occupied },
                new Room { ID = 6, RoomClassID = 2, Floor = 5, Price = 250.00m, Rate = 5, ImageUrl = "/assets/img/rooms/room6.jpg", View = "Mountain View", Status = RoomStatus.Available, PreviousStatus = RoomStatus.Occupied }
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Booking>()
            //    .HasOne(b => b.Room)
            //    .WithMany()
            //    .HasForeignKey(b => b.RoomID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Booking>()
            //    .HasOne(b => b.applicationUser)
            //    .WithMany()
            //    .HasForeignKey(b => b.GuestID)
            //    .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
            Seed(modelBuilder);
        }

    }
}
