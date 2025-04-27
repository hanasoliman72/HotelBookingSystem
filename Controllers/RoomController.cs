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
        public async Task<IActionResult> Index(RoomType? type, int? numberOfBeds, decimal? minPrice, decimal? maxPrice)
        {
            var rooms = _context.rooms
                .Include(r => r.RoomClass)
                .AsQueryable();

            
            if (type.HasValue)
            {
                rooms = rooms.Where(r => r.RoomClass.Type == type);
            }

            if (numberOfBeds.HasValue)
            {
                rooms = rooms.Where(r => r.RoomClass.NumberOfBeds == numberOfBeds.Value);
            }

            // Add price filtering
            if (minPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price <= maxPrice.Value);
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
            ViewBag.RoomClasses = _context.roomClasses.ToList();
            return View(new RoomViewModel());
        }

        // POST: Room/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoomViewModel model)
        {
            var roomClasses = _context.roomClasses.ToList();

            if (!ModelState.IsValid)
            {
                ViewBag.RoomClasses = roomClasses;
                return View(model);
            }

            // Map ViewModel to Entity
            var room = new Room
            {
                RoomClassID = model.RoomClassID,
                Floor = model.Floor,
                Price = model.Price,
                Rate = model.Rate,
                View = model.View,
                Status = RoomStatus.Available,
                PreviousStatus = null
            };

            // Handle image upload
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot", "assets", "img", "rooms");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                room.ImageUrl = "/assets/img/rooms/" + uniqueFileName;
            }

            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

            var viewModel = new RoomViewModel
            {
                ID = room.ID,
                RoomClassID = room.RoomClassID,
                Status = room.Status,
                Floor = room.Floor,
                Price = room.Price,
                Rate = room.Rate,
                View = room.View,
                ExistingImageUrl = room.ImageUrl
            };

            ViewBag.RoomClasses = await _context.roomClasses.ToListAsync();
            return View(viewModel);
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomViewModel model)
        {
            var roomClasses = await _context.roomClasses.ToListAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.RoomClasses = roomClasses;
                return View(model);
            }

            var room = await _context.rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Update properties from view model
            room.RoomClassID = model.RoomClassID;
            room.Status = model.Status;
            room.Floor = model.Floor;
            room.Price = model.Price;
            room.Rate = model.Rate;
            room.View = model.View;

            // Handle image upload if new file provided
            if (model.ImageFile != null && model.ImageFile.Length > 0)
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

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                room.ImageUrl = $"/assets/img/rooms/{uniqueFileName}";
            }

            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
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
        private bool RoomExists(int id)
        {
            return _context.rooms.Any(e => e.ID == id);
        }

    }
}
