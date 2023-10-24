using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tabemory.Data;
using Tabemory.Models;

namespace Tabemory.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly TabemoryDbContext _context;

        public ReviewsController(TabemoryDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var tabemoryContext = _context.Review.Include(r => r.Record);
            return View(await tabemoryContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Review == null)
            {
                return NotFound();
            }

            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var loginUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userReviews = _context.Record.Where(a => a.UserId == loginUserId);

            ViewData["RecordId"] = new SelectList(userReviews, "RecordId", "Name", review.RecordId);
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,VisitDate,Rating,Comment,RecordId")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            ModelState.Remove("Record");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "レビューを編集しました。";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecordId"] = new SelectList(_context.Record, "RecordId", "Name", review.RecordId);
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Review == null)
            {
                return Problem("Entity set 'TabemoryDbContext.Review'  is null.");
            }
            var review = await _context.Review.FindAsync(id);
            if (review != null)
            {
                _context.Review.Remove(review);
                TempData["AlertMessage"] = "レビューを削除しました。";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return (_context.Review?.Any(e => e.ReviewId == id)).GetValueOrDefault();
        }
    }
}
