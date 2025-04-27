namespace BookingSystem.Models
{
    public class CancelBookingViewModel
    {
        public int ID { get; set; }

        public RoomType RoomType { get; set; }  // Flattened from Room.RoomClass.Type
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal PaymentAmount { get; set; }

        // Add any other properties needed for the cancel view
        public bool CanCancel { get; set; }  // Example: could add business logic
        public decimal? RefundAmount { get; set; }  // Example: calculated field
    }
}
