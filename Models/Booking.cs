using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public enum PaymentMethod { CreditCard, Debit_Card, PayPal, Apple_Pay }
    public class Booking
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public int GuestID { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomID { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Range(1, 1000000)]
        [Required]
        public decimal PaymentAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public PaymentMethod paymentMethod { get; set; }

        public ApplicationUser applicationUser { get; set; }
        public Room Room { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; } = new();
    }
}
