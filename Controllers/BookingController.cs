using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

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

    // POST: Booking/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Booking booking)
    {
        var guestId = HttpContext.Session.GetInt32("GuestID");
        if (guestId == null)
        {
            return RedirectToAction("Login", "Home");
        }

        booking.GuestID = guestId.Value;

        if (ModelState.IsValid)
        {
            _context.bookings.Add(booking);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Room = _context.rooms.Include(r => r.RoomClass).FirstOrDefault(r => r.ID == booking.RoomID);
        return View(booking);
    }
}
