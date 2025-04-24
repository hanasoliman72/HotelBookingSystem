using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class BookingController : Controller
{
    private readonly BookingContext _context;

    public BookingController(BookingContext context)
    {
        _context = context;
    }

    // GET: Booking/Create
    public IActionResult Create(int roomId)
    {
        var room = _context.rooms.Include(r => r.RoomClass).FirstOrDefault(r => r.ID == roomId);

        if (room == null)
        {
            return NotFound();
        }

        var booking = new Booking
        {
            RoomID = roomId,
            PaymentAmount = room.Price,
        };

        ViewBag.Room = room;

        return View(booking);
    }

    
    // GET: Booking/BookAny
    public IActionResult BookAny()
    {
        ViewBag.Rooms = _context.rooms
                                .Include(r => r.RoomClass)
                                .Where(r => r.Status == RoomStatus.Available)
                                .ToList();

        return View();
    }

    public IActionResult MyBookings()
    {
        return View();
    }

}
