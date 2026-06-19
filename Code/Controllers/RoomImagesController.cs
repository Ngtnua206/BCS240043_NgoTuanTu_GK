using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTKTGK_BCS240043_NgoTuanTu.Models;

namespace BTKTGK_BCS240043_NgoTuanTu.Controllers
{
    public class RoomImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int roomId, string imageUrl)
        {
            var roomExists = await _context.Rooms_BCS240043.AnyAsync(r => r.Id == roomId);
            if (!roomExists)
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại.";
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đường dẫn ảnh.";
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }

            var hasImages = await _context.RoomImages_BCS240043.AnyAsync(ri => ri.RoomId == roomId);

            var image = new RoomImage_BCS240043
            {
                ImageUrl = imageUrl,
                IsThumbnail = !hasImages,
                RoomId = roomId
            };

            _context.Add(image);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Rooms", new { id = roomId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int id, int roomId)
        {
            var image = await _context.RoomImages_BCS240043.FindAsync(id);
            if (image == null)
            {
                TempData["ErrorMessage"] = "Ảnh không tồn tại.";
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }

            var currentThumbnails = await _context.RoomImages_BCS240043
                .Where(ri => ri.RoomId == roomId && ri.IsThumbnail)
                .ToListAsync();

            foreach (var thumb in currentThumbnails)
            {
                thumb.IsThumbnail = false;
            }

            image.IsThumbnail = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Rooms", new { id = roomId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int roomId)
        {
            var image = await _context.RoomImages_BCS240043.FindAsync(id);
            if (image == null)
            {
                TempData["ErrorMessage"] = "Ảnh không tồn tại.";
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }

            bool wasThumbnail = image.IsThumbnail;

            _context.RoomImages_BCS240043.Remove(image);
            await _context.SaveChangesAsync();

            if (wasThumbnail)
            {
                var nextImage = await _context.RoomImages_BCS240043
                    .Where(ri => ri.RoomId == roomId)
                    .FirstOrDefaultAsync();

                if (nextImage != null)
                {
                    nextImage.IsThumbnail = true;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Details", "Rooms", new { id = roomId });
        }
    }
}
