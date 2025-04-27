using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingSystem.Validations;

namespace BookingSystem.Models
{
    public enum PaymentMethod {
        [Display(Name = "Credit Card")]
        CreditCard,
        [Display(Name = "Debit Card")]
        Debit_Card,
        [Display(Name = "PayPal")]
        PayPal,
        [Display(Name = "Apple Pay")]
        Apple_Pay,
        [Display(Name = "Cash")]
        Cash
    }
    public class Booking
    {
        public int ID { get; set; }

        [Required]
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int RoomID { get; set; }
        public virtual Room Room { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CurrentOrFutureDate(ErrorMessage = "Check-in date must be today or in the future")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be after check-in date")]
        public DateTime CheckOutDate { get; set; }

        [Range(90, 1000000)]
        [Required]
        public decimal PaymentAmount { get; set; }

        [Required]
        public PaymentMethod paymentMethod { get; set; }

        public virtual List<Feedback> Feedbacks { get; set; } = new();
    }
}
