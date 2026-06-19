using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTKTGK_BCS240043_NgoTuanTu.Models;

namespace BTKTGK_BCS240043_NgoTuanTu.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? roomTypeId, bool? isAvailable, decimal? maxPrice, string sortOrder)
        {
            ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
            ViewData["SearchString"] = searchString;
            ViewData["RoomTypeId"] = roomTypeId;
            ViewData["IsAvailable"] = isAvailable;
            ViewData["MaxPrice"] = maxPrice;
            ViewData["SortOrder"] = sortOrder;
            ViewData["PriceSort"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["AreaSort"] = sortOrder == "area_desc" ? "" : "area_desc";

            var query = _context.Rooms_BCS240043
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(r => r.Name.Contains(searchString));

            if (roomTypeId.HasValue)
                query = query.Where(r => r.RoomTypeId == roomTypeId.Value);

            if (isAvailable.HasValue)
                query = query.Where(r => r.IsAvailable == isAvailable.Value);

            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice.Value);

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(r => r.Price),
                "price_desc" => query.OrderByDescending(r => r.Price),
                "area_desc" => query.OrderByDescending(r => r.Area),
                _ => query.OrderBy(r => r.Name)
            };

            var rooms = await query.ToListAsync();

            if (!rooms.Any())
                ViewBag.NoResults = true;

            return View(rooms);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var room = await _context.Rooms_BCS240043
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            if (!room.RoomImages.Any())
                ViewBag.NoImages = true;

            return View(room);
        }

        public async Task<IActionResult> Create()
        {
            var roomTypes = await _context.RoomTypes_BCS240043.ToListAsync();
            if (!roomTypes.Any())
            {
                TempData["ErrorMessage"] = "Vui lòng tạo loại phòng trước khi thêm phòng.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypes"] = roomTypes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Area,IsAvailable,Description,RoomTypeId")] Room_BCS240043 room)
        {
            if (ModelState.IsValid)
            {
                var roomTypeExists = await _context.RoomTypes_BCS240043.AnyAsync(rt => rt.Id == room.RoomTypeId);
                if (!roomTypeExists)
                {
                    ModelState.AddModelError("RoomTypeId", "Loại phòng không tồn tại.");
                    ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
                    return View(room);
                }

                var nameExists = await _context.Rooms_BCS240043.AnyAsync(r => r.Name == room.Name && r.RoomTypeId == room.RoomTypeId);
                if (nameExists)
                {
                    ModelState.AddModelError("Name", "Tên phòng đã tồn tại trong loại phòng này.");
                    ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
                    return View(room);
                }

                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
            return View(room);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var room = await _context.Rooms_BCS240043.FindAsync(id);
            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Area,IsAvailable,Description,RoomTypeId")] Room_BCS240043 room)
        {
            if (id != room.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var roomTypeExists = await _context.RoomTypes_BCS240043.AnyAsync(rt => rt.Id == room.RoomTypeId);
                if (!roomTypeExists)
                {
                    ModelState.AddModelError("RoomTypeId", "Loại phòng không tồn tại.");
                    ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
                    return View(room);
                }

                var nameExists = await _context.Rooms_BCS240043.AnyAsync(r => r.Name == room.Name && r.RoomTypeId == room.RoomTypeId && r.Id != room.Id);
                if (nameExists)
                {
                    ModelState.AddModelError("Name", "Tên phòng đã tồn tại trong loại phòng này.");
                    ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
                    return View(room);
                }

                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Rooms_BCS240043.AnyAsync(r => r.Id == id))
                    {
                        TempData["ErrorMessage"] = "Phòng không tồn tại.";
                        return RedirectToAction(nameof(Index));
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomTypes"] = await _context.RoomTypes_BCS240043.ToListAsync();
            return View(room);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var room = await _context.Rooms_BCS240043
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms_BCS240043.FindAsync(id);
            if (room == null)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            _context.Rooms_BCS240043.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
