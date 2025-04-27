using BookingSystem.Models;
using BookingSystem.Validations;
using System.ComponentModel.DataAnnotations;

public class BookingViewModel
{
    [Required]
    public int RoomID { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CurrentOrFutureDate(ErrorMessage = "Check-in date must be today or in the future")]
    public DateTime CheckInDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DateGreaterThan("CheckInDate", ErrorMessage = "Check-out date must be after check-in date")]
    public DateTime CheckOutDate { get; set; }

    [Required]
    [Range(90, 1000000)]
    public decimal PaymentAmount { get; set; }

    [Required]
    public PaymentMethod paymentMethod { get; set; }
}
