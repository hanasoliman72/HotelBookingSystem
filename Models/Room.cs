using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public enum RoomStatus { Available, Occupied, Maintenance }
    public class Room
    {
        public int ID { get; set; }
        [ForeignKey("RoomClass")]
        public int RoomClassID { get; set; }
        [Range(1, 10, ErrorMessage = "Floor must be between 1 and 10.")]
        public int Floor { get; set; }
        [Range(1, 1000000)]
        public decimal Price { get; set; }
        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5.")]
        public int Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? View { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        [Required]
        public RoomStatus? PreviousStatus { get; set; }

        public virtual RoomClass RoomClass { get; set; }
        public virtual List<Booking> Bookings { get; set; } = new();


        //Method to validate status transition
        //preventing a room from going directly from Maintenance to Occupied
        public bool IsValidStatusTransition(RoomStatus newStatus)
        {
            if (PreviousStatus == RoomStatus.Maintenance && newStatus == RoomStatus.Occupied)
            {
                return false;
            }
            return true;
        }
    }
}
