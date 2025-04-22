using Microsoft.AspNetCore.Mvc;
using BookingSystem.Models;
using BookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookingSystem.Controllers
{
    public class RoomController : Controller
    {
        private readonly BookingContext _context;

        public RoomController(BookingContext context)
        {
            _context = context;
        }

        // GET: Room/Index
        public IActionResult Index(string roomType = null, decimal? minPrice = null, decimal? maxPrice = null, RoomStatus? status = null)
        {
            // Start with all rooms and include their related RoomClass
            var rooms = _context.rooms
                                .Include(r => r.RoomClass)
                                .AsQueryable();

            // Apply filters if values are provided
            if (!string.IsNullOrWhiteSpace(roomType))
            {
                roomType = roomType.Trim();
                rooms = rooms.Where(r => r.RoomClass.Type.Contains(roomType));
            }

            if (minPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price <= maxPrice.Value);
            }

            if (status.HasValue)
            {
                rooms = rooms.Where(r => r.Status == status.Value);
            }

            // Convert to list and send to view
            return View(rooms.ToList());
        }


        // GET: Room/Edit/{id}
        public IActionResult Edit(int id)
        {
            var room = _context.rooms.Include(r => r.RoomClass).FirstOrDefault(r => r.ID == id);
            if (room == null)
            {
                return NotFound();
            }

            // Populate the RoomClasses for the dropdown
            ViewBag.RoomClasses = _context.roomClasses.ToList();

            return View(room);
        }

        // POST: Room/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Room room)
        {
            if (id != room.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.rooms.Any(r => r.ID == room.ID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate RoomClasses for the dropdown in case of validation failure
            ViewBag.RoomClasses = _context.roomClasses.ToList();

            return View(room);
        }

        // GET: Room/Delete/5
        public IActionResult Delete(int id)
        {
            var room = _context.rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.rooms.Remove(room);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Room/Details/{id}
        public IActionResult Details(int id)
        {
            var room = _context.rooms
                               .Include(r => r.RoomClass) // Include related RoomClass for room details
                               .FirstOrDefault(r => r.ID == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }


        // GET: Room/ADD
        public IActionResult Add()
        {
            // Populate the RoomClasses for the dropdown
            ViewBag.RoomClasses = _context.roomClasses.ToList();
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.rooms.Add(room);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate RoomClasses for the dropdown in case of validation failure
            ViewBag.RoomClasses = _context.roomClasses.ToList();
            return View(room);
        }


    }
}
