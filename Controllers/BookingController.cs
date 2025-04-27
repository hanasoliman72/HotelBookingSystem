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

    // Booking/BookAny
    public IActionResult BookAny()
    {
        var availableRooms = _context.rooms
                                    .Include(r => r.RoomClass)
                                    .Where(r => r.Status == RoomStatus.Available)
                                    .ToList();

        var viewModel = new BookingViewModel
        {
            Rooms = availableRooms,
            Booking = new Booking() // Initialize a new booking object
        };

        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> BookAny(BookingViewModel viewModel)
    {
        var userId = HttpContext.Session.GetString("Id");
        if (userId == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        // Set GuestID before model validation
        viewModel.Booking.GuestID = userId;
        //bool isValid = true;
        //if (viewModel.Booking.RoomID == 0)
        //{
        //    isValid = false;
        //    //ModelState.AddModelError("Booking.RoomID", "Room is required.");
        //}

        //else if (viewModel.Booking.CheckInDate == default || viewModel.Booking.CheckInDate < DateTime.Now.Date)
        //{
        //    isValid = false;
        //    //ModelState.AddModelError("Booking.CheckInDate", "Check-in date must be today or in the future.");
        //}

        //else if (viewModel.Booking.CheckOutDate <= viewModel.Booking.CheckInDate)
        //{
        //    isValid = false;
        //    //ModelState.AddModelError("Booking.CheckOutDate", "Check-out date must be after the check-in date.");
        //}

        //else if (viewModel.Booking.PaymentAmount < 90 || viewModel.Booking.PaymentAmount > 1000000)
        //{
        //    isValid = false;
        //    //ModelState.AddModelError("Booking.PaymentAmount", "Payment amount must be between $90 and $1,000,000.");
        //}

        //else if (viewModel.Booking.paymentMethod == 0)
        //{
        //    isValid = false;
        //    //ModelState.AddModelError("Booking.paymentMethod", "Payment method is required.");
        //}

        //if (!isValid)
        //{
        //    return View(viewModel);
        //}

        // Proceed with the rest of the code after validation passes
        var room = await _context.rooms
                          .FirstOrDefaultAsync(r => r.ID == viewModel.Booking.RoomID);
        if (room != null)
        {
            room.Status = RoomStatus.Occupied;
            _context.rooms.Update(room);
        }

        _context.bookings.Add(viewModel.Booking);
        await _context.SaveChangesAsync();

        return RedirectToAction("Success");
    }


    public IActionResult Success()
    {
        return View();
    }

    public IActionResult MyBookings()
    {
        return View();
    }

}
