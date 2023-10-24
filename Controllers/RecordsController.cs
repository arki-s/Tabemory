using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tabemory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tabemory.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Tabemory.ViewModels;
using System.Text;

namespace Tabemory.Controllers
{
    public class RecordsController : Controller
    {
        private readonly TabemoryDbContext _context;

        public RecordsController(TabemoryDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Record != null ?
                         View(await _context.Record.ToListAsync()) :
                         Problem("Entity set 'CookingRecipesContext.Recipe'  is null.");
        }


        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] items)
        {

            var loginUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Record record = new Record()
            {
                Genre_catch = items[0],
                Genre_name = items[1],
                Name = items[2],
                Address = items[3],
                Station_name = items[4],
                Open = items[5],
                Close = items[6],
                Url = items[7],
                Photo = items[8],
                UserId = loginUserId
            };


            ModelState.Remove("Reviews");
            if (ModelState.IsValid)
            {
                _context.Add(record);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "新しいレストランを登録しました。";
                return RedirectToAction(nameof(Index));
            }

            return View(record);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Record == null)
            {
                return NotFound();
            }

            var record = await _context.Record.Include("Reviews")
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (record == null)
            {
                return NotFound();
            }

            var recordView = new Record()
            {
                RecordId = record.RecordId,
                Name = record.Name,
                Address = record.Address,
                Station_name = record.Station_name,
                Genre_catch = record.Genre_catch,
                Genre_name = record.Genre_name,
                Open = record.Open,
                Close = record.Close,
                Photo = record.Photo,
                UserId = record.UserId
             };

            var reviewList = new List<Review>();

            if (record.Reviews.Count > 0)
            {
                foreach (var item in record.Reviews)
                {
                    reviewList.Add(new Review
                    {
                        ReviewId = item.ReviewId,
                        VisitDate = item.VisitDate,
                        Rating = item.Rating,
                        Comment = item.Comment
                    });
                }
            }

            var recordreview = new RecordReview() {
                Record = recordView,
                ReviewList = reviewList,
                Review = new Review()
            };

            return View(recordreview);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id, RecordReview model, string[] values)
        {

            var review = new Review()
            {
                VisitDate = DateTime.Parse(values[0]),
                Rating = int.Parse(values[1]),
                Comment = values[2],
                RecordId = int.Parse(values[3])
            };

            _context.Add(review);
            await _context.SaveChangesAsync();

            if (id == null || _context.Record == null)
            {
                return NotFound();
            }

            var record = await _context.Record.Include("Reviews")
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (record == null)
            {
                return NotFound();
            }

            var recordView = new Record()
            {
                RecordId = record.RecordId,
                Name = record.Name,
                Address = record.Address,
                Station_name = record.Station_name,
                Genre_catch = record.Genre_catch,
                Genre_name = record.Genre_name,
                Open = record.Open,
                Close = record.Close,
                Photo = record.Photo,
                UserId = record.UserId
            };

            var reviewList = new List<Review>();

            if (record.Reviews.Count > 0)
            {
                foreach (var item in record.Reviews)
                {
                    reviewList.Add(new Review
                    {
                        ReviewId = item.ReviewId,
                        VisitDate = item.VisitDate,
                        Rating = item.Rating,
                        Comment = item.Comment,
                        Image = item.Image,
                    });
                }
            }

            model.Record = recordView;
            model.ReviewList = reviewList;
            model.Review = review;

            return View(model);

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int[] value)
        {
            if (_context.Record == null)
            {
                return Problem("Entity set 'CookingRecipesContext.Recipe'  is null.");
            }
            var record = await _context.Record.FindAsync(value[0]);
            if (record != null)
            {
                _context.Record.Remove(record);
                TempData["AlertMessage"] = "選択したレストランを削除しました。";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}



