using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Diagnostics;

public class BookingController : Controller
{
    private readonly BookingContext _context;

    public BookingController(BookingContext context)
    {
        _context = context;
    }

    // GET: Booking/Create/1
    public IActionResult Create(int roomId)
    {
        var room = _context.rooms
            .Include(r => r.RoomClass)
            .FirstOrDefault(r => r.ID == roomId);

        if (room == null)
        {
            return NotFound();
        }

        var viewModel = new BookingViewModel
        {
            RoomID = roomId,
            PaymentAmount = room.Price,
            // Set default dates
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddDays(1)
        };

        ViewBag.Room = room;
        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel viewModel)
    {
        // Get user ID from session
        var userId = HttpContext.Session.GetString("Id");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Get the room
        var room = await _context.rooms.FindAsync(viewModel.RoomID);

        // Manual validation
        if (viewModel.CheckInDate == default)
            ModelState.AddModelError("CheckInDate", "Check-in date is required.");

        if (viewModel.CheckOutDate == default)
            ModelState.AddModelError("CheckOutDate", "Check-out date is required.");

        if (viewModel.CheckOutDate <= viewModel.CheckInDate)
            ModelState.AddModelError("CheckOutDate", "Check-out date must be after check-in date.");


        if (ModelState.IsValid)
        {
            try
            {
                // Map ViewModel to Entity
                var booking = new Booking
                {
                    RoomID = viewModel.RoomID,
                    ApplicationUserID = userId,
                    CheckInDate = viewModel.CheckInDate,
                    CheckOutDate = viewModel.CheckOutDate,
                    PaymentAmount = viewModel.PaymentAmount,
                    paymentMethod = viewModel.paymentMethod
                };

                _context.Add(booking);
                room.Status = RoomStatus.Occupied;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                Debug.WriteLine(ex.Message);
            }
        }
        ViewBag.Room = room;
        return View(viewModel);
    }
    public IActionResult Success()
    {
        return View();
    }

    public async Task<IActionResult> MyBookings()
    {
        var userId = HttpContext.Session.GetString("Id");
        if (string.IsNullOrEmpty(userId))
        {
            return Redirect("/Identity/Account/Login");
        }

        var bookings = await _context.bookings
            .Include(b => b.Room)
                .ThenInclude(r => r.RoomClass)
            .Where(b => b.ApplicationUserID == userId)
            .OrderByDescending(b => b.CheckInDate)
            .ToListAsync();

        return View(bookings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var booking = await _context.bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.ID == id);

        if (booking == null)
        {
            TempData["ErrorMessage"] = "Booking not found.";
            return RedirectToAction("MyBookings");
        }

        // Optional: free the room if needed
        if (booking.Room != null)
        {
            booking.Room.Status = RoomStatus.Available;
        }

        _context.bookings.Remove(booking);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Booking canceled successfully!";
        return RedirectToAction("MyBookings");
    }

}
