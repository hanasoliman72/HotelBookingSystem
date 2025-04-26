using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public enum RoomType { Suite, Standard, Deluxe}
    public class RoomClass
    {
        public int ID { get; set; }
        public RoomType Type { get; set; }
        public string? Description { get; set; }
        [Range(1, 10, ErrorMessage = "Number of beds must be between 1 and 10.")]
        public int NumberOfBeds { get; set; }

        public virtual List<Room>? Rooms { get; set; } = new();
    }
}
