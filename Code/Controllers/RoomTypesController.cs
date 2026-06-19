using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTKTGK_BCS240043_NgoTuanTu.Models;

namespace BTKTGK_BCS240043_NgoTuanTu.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.RoomTypes_BCS240043.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] RoomType_BCS240043 roomType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var roomType = await _context.RoomTypes_BCS240043.FindAsync(id);
            if (roomType == null)
            {
                TempData["ErrorMessage"] = "Loại phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] RoomType_BCS240043 roomType)
        {
            if (id != roomType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.RoomTypes_BCS240043.AnyAsync(rt => rt.Id == id))
                    {
                        TempData["ErrorMessage"] = "Loại phòng không tồn tại.";
                        return RedirectToAction(nameof(Index));
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var roomType = await _context.RoomTypes_BCS240043
                .Include(rt => rt.Rooms)
                .FirstOrDefaultAsync(rt => rt.Id == id);

            if (roomType == null)
            {
                TempData["ErrorMessage"] = "Loại phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            if (roomType.Rooms.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa loại phòng này vì đang có phòng sử dụng.";
                return RedirectToAction(nameof(Index));
            }

            return View(roomType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomType = await _context.RoomTypes_BCS240043
                .Include(rt => rt.Rooms)
                .FirstOrDefaultAsync(rt => rt.Id == id);

            if (roomType == null)
            {
                TempData["ErrorMessage"] = "Loại phòng không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            if (roomType.Rooms.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa loại phòng này vì đang có phòng sử dụng.";
                return RedirectToAction(nameof(Index));
            }

            _context.RoomTypes_BCS240043.Remove(roomType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
