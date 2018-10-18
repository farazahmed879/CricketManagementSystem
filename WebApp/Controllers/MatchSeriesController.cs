using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApp.ViewModels;
using AutoMapper.QueryableExtensions;
using WebApp.Helper;

namespace WebApp.Controllers
{

    public class MatchSeriesController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;

        public MatchSeriesController(CricketContext context,
            UserManager<IdentityUser<int>> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;

        }

        // GET: MatchSeries

        public async Task<IActionResult> Index(int? page, int? userId)
        {
            ViewBag.Name = "Match Series";
            int pageSize = 20;
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;

            return View(await PaginatedList<MatchSeriesdto>.CreateAsync(
              _context.MatchSeries
               .Where(i => !userId.HasValue || i.UserId == users.Id)
                .Select(i => new MatchSeriesdto
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
        public async Task<IActionResult> Create(MatchSeriesdto matchSeries)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.GetUserAsync(HttpContext.User);
                var matchSeriesModal = _mapper.Map<MatchSeries>(matchSeries);
                matchSeriesModal.UserId = users.Id;
                _context.MatchSeries.Add(matchSeriesModal);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
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

            var matchSeries = await _context.MatchSeries
                .AsNoTracking()
                .ProjectTo<MatchSeriesdto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(m => m.MatchSeriesId == id);
            if (matchSeries == null)
            {
                return NotFound();
            }
            return View(matchSeries);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(MatchSeriesdto matchSeries)
        {

            if (ModelState.IsValid)
            {

                var users = await _userManager.GetUserAsync(HttpContext.User);
                var matchSeriesModal = _mapper.Map<MatchSeries>(matchSeries);
                matchSeriesModal.UserId = users.Id;
                _context.MatchSeries.Update(matchSeriesModal);
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }

            return Json(ResponseHelper.UpdateUnSuccess());
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
