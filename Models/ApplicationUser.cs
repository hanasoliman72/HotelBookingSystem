using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public enum Role { Admin, Guest }
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public Role role { get; set; }

        public virtual List<Booking> Bookings { get; set; }
    }
}
