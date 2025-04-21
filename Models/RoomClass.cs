namespace BookingSystem.Models
{
    public class RoomClass
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int NumberOfBeds { get; set; }

        public virtual List<Room>? Rooms { get; set; } = new();
    }
}
