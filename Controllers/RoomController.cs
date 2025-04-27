using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting; 
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingSystem.Controllers
{
    public class RoomController : Controller
    {
        private readonly BookingContext _context;
        private readonly IWebHostEnvironment _environment;

        public RoomController(BookingContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Room/Index
        public async Task<IActionResult> Index(RoomStatus? status, RoomType? type, int? numberOfBeds)
        {
            var rooms = _context.rooms
                .Include(r => r.RoomClass)
                .AsQueryable();

            // Filtering
            if (status.HasValue)
            {
                rooms = rooms.Where(r => r.Status == status);
            }

            if (type.HasValue)
            {
                rooms = rooms.Where(r => r.RoomClass.Type == type);
            }

            if (numberOfBeds.HasValue)
            {
                rooms = rooms.Where(r => r.RoomClass.NumberOfBeds == numberOfBeds.Value);
            }

            // Check if user is admin
            string Role = HttpContext.Session.GetString("role");
            bool isAdmin = Role == "Admin";

            if (!isAdmin)
            {
                rooms = rooms.Where(r => r.Status == RoomStatus.Available);
            }

            var model = await rooms.ToListAsync();
            ViewBag.IsAdmin = isAdmin; // Pass this to the view

            return View(model);
        }


        // GET: Room/Add
        public IActionResult Add()
        {
            var roomClasses = _context.roomClasses.ToList();
            ////Console.WriteLine($"RoomClasses count: {roomClasses?.Count ?? 0}"); // Debug
            ViewBag.RoomClasses = roomClasses ?? new List<RoomClass>();
            return View();
        }

        // POST: Room/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Room room, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Set your desired path
                    var uploadsFolder = Path.Combine("wwwroot", "assets", "img", "rooms");
                    Directory.CreateDirectory(uploadsFolder); // Create if doesn't exist

                    // Generate unique filename
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Store relative path
                    room.ImageUrl = "/assets/img/rooms/" + uniqueFileName;
                }

                room.PreviousStatus = room.Status;
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.RoomClasses = _context.roomClasses.ToList();
            return View(room);
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.rooms
                .Include(r => r.RoomClass)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            ViewBag.RoomClasses = await _context.roomClasses.ToListAsync();
            return View(room);
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,RoomClassID,Floor,Price,View,Status,Rate,ImageUrl")] Room room, IFormFile? imageFile)
        {
            if (id != room.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload if new file provided
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(room.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_environment.WebRootPath, room.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "assets", "img", "rooms");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        room.ImageUrl = $"/assets/img/rooms/{uniqueFileName}";
                    }

                    _context.Update(room);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Room updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.RoomClasses = await _context.roomClasses.ToListAsync();
            return View(room);
        }
        private bool RoomExists(int id)
        {
            return _context.rooms.Any(e => e.ID == id);
        }

    }
}
