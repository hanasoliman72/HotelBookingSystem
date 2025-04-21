using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Booking
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey("Guest")]
        public int GuestID { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomID { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }

        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }

        public Guest Guest { get; set; }
        public Room Room { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; } = new();
    }
}
