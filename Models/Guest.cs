using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Guest : EndUser
    {
        public virtual List<Booking> Bookings { get; set; }
    }
}
