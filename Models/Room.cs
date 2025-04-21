using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public enum RoomStatus { Available, Occupied, Maintenance }
    public class Room
    {
        public int ID { get; set; }
        [ForeignKey("RoomClass")]
        public int RoomClassID { get; set; }
        public int Floor { get; set; }
        public decimal Price { get; set; }
        public int Rate { get; set; }
        public string ImageUrl { get; set; }
        public string View { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        public virtual RoomClass RoomClass { get; set; }
        public virtual List<Booking> Bookings { get; set; } = new();
    }
}
