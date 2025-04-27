using BookingSystem.Models;

public class BookingViewModel
{
    public List<Room> Rooms { get; set; }  // List of rooms
    public Booking Booking { get; set; }  // Booking model for user input
}
