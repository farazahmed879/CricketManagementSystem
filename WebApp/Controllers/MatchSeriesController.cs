using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{

    public class MatchSeriesController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public MatchSeriesController(CricketContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: MatchSeries

        public async Task<IActionResult> Index(int? page, int? userId)
        {
            ViewBag.Name = "Match Series";
            int pageSize = 20;
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;

            return View(await PaginatedList<ViewModels.MatchSeriesdto>.CreateAsync(
              _context.MatchSeries
               .Where(i => !userId.HasValue || i.UserId == users.Id)
                .Select(i => new ViewModels.MatchSeriesdto
                {
                    MatchSeriesId = i.MatchSeriesId,
                    Name = i.Name,
                    Organizor = i.Organizor,
                    StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToShortDateString() : "",

                })
                .OrderByDescending(i => i.MatchSeriesId)
       , page ?? 1, pageSize));

        }

        // GET: MatchSeries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matchSeries = await _context.MatchSeries
                .SingleOrDefaultAsync(m => m.MatchSeriesId == id);
            if (matchSeries == null)
            {
                return NotFound();
            }

            return View(matchSeries);
        }

        // GET: MatchSeries/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add MatchSeries";
            return View();
        }

        // POST: MatchSeries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(MatchSeries matchSeries)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.GetUserAsync(HttpContext.User);
                matchSeries.UserId = users.Id;
                _context.Add(matchSeries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(matchSeries);
        }

        // GET: MatchSeries/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Name = "Edit MatchSeries";
            if (id == null)
            {
                return NotFound();
            }

            var matchSeries = await _context.MatchSeries.SingleOrDefaultAsync(m => m.MatchSeriesId == id);
            if (matchSeries == null)
            {
                return NotFound();
            }
            return View(matchSeries);
        }

        // POST: MatchSeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int id, MatchSeries matchSeries)
        {
            if (id != matchSeries.MatchSeriesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var users = await _userManager.GetUserAsync(HttpContext.User);
                    matchSeries.UserId = users.Id;
                    _context.Update(matchSeries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchSeriesExists(matchSeries.MatchSeriesId))
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
            return View(matchSeries);
        }

        // GET: MatchSeries/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matchSeries = await _context.MatchSeries
                .SingleOrDefaultAsync(m => m.MatchSeriesId == id);
            if (matchSeries == null)
            {
                return NotFound();
            }

            return View(matchSeries);
        }

        // POST: MatchSeries/Delete/5
        [Authorize(Roles = "Club Admin,Administrator")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int matchSeriesId)
        {
            var matchSeries = await _context.MatchSeries.SingleOrDefaultAsync(m => m.MatchSeriesId == matchSeriesId);
            _context.MatchSeries.Remove(matchSeries);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool MatchSeriesExists(int id)
        {
            return _context.MatchSeries.Any(e => e.MatchSeriesId == id);
        }
    }
}
