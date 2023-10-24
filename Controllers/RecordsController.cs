using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tabemory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tabemory.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Record == null)
            {
                return NotFound();
            }

            var record = await _context.Record
                .FirstOrDefaultAsync(m => m.RecordId == id);
            if (record == null)
            {
                return NotFound();
            }

            return View(record);

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



