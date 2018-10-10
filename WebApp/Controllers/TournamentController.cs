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

namespace WebApp.Controllers
{

    public class TournamentController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;

        public TournamentController(CricketContext context,
            UserManager<IdentityUser<int>> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Tournaments

        public async Task<IActionResult> Index(int? page, int? userId)
        {
            ViewBag.Name = "Tournaments";
            int pageSize = 20;
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;

            return View(await PaginatedList<ViewModels.Tournamentdto>.CreateAsync(
              _context.Tournaments
               .Where(i => !userId.HasValue || i.UserId == users.Id)
                .Select(i => new ViewModels.Tournamentdto
                {
                    TournamentId = i.TournamentId,
                    TournamentName = i.TournamentName,
                    Organizor = i.Organizor,
                    StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToShortDateString() : "",

                })
                .OrderByDescending(i => i.TournamentId)
       , page ?? 1, pageSize));

        }

        // GET: Tournaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .SingleOrDefaultAsync(m => m.TournamentId == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // GET: Tournaments/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add Tournament";
            return View();
        }

        // POST: Tournaments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Tournamentdto tournament)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.GetUserAsync(HttpContext.User);
                tournament.UserId = users.Id;
                _context.Tournaments.Add(_mapper.Map<Tournament>(tournament));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        // GET: Tournaments/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Name = "Edit Tournament";
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .AsNoTracking()
                .ProjectTo<Tournamentdto>()
                .SingleOrDefaultAsync(m => m.TournamentId == id);
            if (tournament == null)
            {
                return NotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int id, Tournamentdto tournament)
        {
            if (id != tournament.TournamentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var users = await _userManager.GetUserAsync(HttpContext.User);
                    tournament.UserId = users.Id;
                    _context.Tournaments.Update(_mapper.Map<Tournament>(tournament));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.TournamentId))
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
            return View(tournament);
        }

        // GET: Tournaments/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .SingleOrDefaultAsync(m => m.TournamentId == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [Authorize(Roles = "Club Admin,Administrator")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int tournamentId)
        {
            var tournament = await _context.Tournaments.SingleOrDefaultAsync(m => m.TournamentId == tournamentId);
            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.TournamentId == id);
        }
    }
}
