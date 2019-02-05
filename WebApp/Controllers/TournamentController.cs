using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApp.ViewModels;
using WebApp.Helper;
using WebApp.IServices;

namespace WebApp.Controllers
{

    public class TournamentController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly ITournaments _tournaments;

        public TournamentController(CricketContext context,
            UserManager<IdentityUser<int>> userManager, ITournaments tournaments,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _tournaments = tournaments;
        }

        // GET: Tournaments

        public async Task<IActionResult> Index(int? page, int? userId)
        {
            ViewBag.Name = "Tournaments";
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;
            var model = await _tournaments.GetAllTournaments(page, userId);

            return View(model);
        }

        public async Task<IActionResult> List(int? page, int? userId)
        {
            ViewBag.Name = "Tournaments";
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;
            var model = await _tournaments.GetAllTournaments(page, userId);

            return Json(model);
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

        // GET: Tournament/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add Tournament";
            return View();
        }

        // POST: Tournament/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Tournament/Create")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Tournamentdto tournament)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.GetUserAsync(HttpContext.User);
                var tournamentModel = _mapper.Map<Tournament>(tournament);
                tournamentModel.UserId = users.Id;
                _context.Tournaments.Add(tournamentModel);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
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
                .Select(i => new ViewModels.Tournamentdto
                {
                    TournamentId = i.TournamentId,
                    TournamentName = i.TournamentName,
                    Organizor = i.Organizor,
                    StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToShortDateString() : "",

                })
                .SingleOrDefaultAsync(m => m.TournamentId == id);
            if (tournament == null)
            {
                return NotFound();
            }
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Tournament/Edit")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(Tournamentdto tournament)
        {

            if (ModelState.IsValid)
            {

                var users = await _userManager.GetUserAsync(HttpContext.User);
                var tournamentModal = _mapper.Map<Tournament>(tournament);
                tournamentModal.UserId = users.Id;
                _context.Tournaments.Update(tournamentModal);
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }


        // POST: Tournaments/Delete/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int tournamentId)
        {
            var tournament = await _context.Tournaments.SingleOrDefaultAsync(m => m.TournamentId == tournamentId);
            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
