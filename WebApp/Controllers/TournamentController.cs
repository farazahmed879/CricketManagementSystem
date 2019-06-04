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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Controllers
{

    public class TournamentController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly ITournaments _tournaments;
        private readonly IHostingEnvironment _hosting;

        public TournamentController(CricketContext context,
            UserManager<IdentityUser<int>> userManager, ITournaments tournaments, IHostingEnvironment hosting,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _tournaments = tournaments;
            _hosting = hosting;
        }

        // GET: Tournaments
        [HttpGet]
        public async Task<IActionResult> Index(DataTableAjaxPostModel model, int? page, bool isApi)
        {
            ViewBag.Name = "Tournaments";

            var result = await _tournaments.GetAllTournaments(model.Init(), page);

            if (isApi == true)
                return Json(new
                {
                    data = result,
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                });
            else
                return View(result);
        }

        [HttpGet("Tournament/List")]
        public async Task<IActionResult> List(DataTableAjaxPostModel model, int? page)
        {
            ViewBag.Name = "Tournaments";
            var result = await _tournaments.GetAllTournaments(model.Init(), page);

            return Json(result);
        }

        [HttpGet("Tournament/Details/{id}")]
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

        [HttpGet("Tournament/Create")]
        // GET: Tournament/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add Tournament";
            return View();
        }

        // POST: Tournament/Create

        [ValidateAntiForgeryToken]
        [HttpPost("Tournament/Create")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Tournamentdto tournament)
        {
            if (ModelState.IsValid)
            {
                var directory = Path.Combine(_hosting.WebRootPath, "Home", "images", "Tournament");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                if (tournament.TournamentImage != null)
                {
                    tournament.FileName = tournament.TournamentImage.FileName;
                    using (var stream = new FileStream(Path.Combine(directory, tournament.FileName), FileMode.Create))
                    {
                        await tournament.TournamentImage.CopyToAsync(stream);
                    }
                }

                var users = await _userManager.GetUserAsync(HttpContext.User);
                var tournamentModel = _mapper.Map<Tournament>(tournament);
                tournamentModel.UserId = users.Id;
                _context.Tournaments.Add(tournamentModel);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }

        [HttpGet("Tournament/Edit/{id}")]
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
                    FileName = i.FileName,
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
        [HttpPut("Tournament/Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(Tournamentdto tournament)
        {

            if (ModelState.IsValid)
            {
                var directory = Path.Combine(_hosting.WebRootPath, "Home", "images", "Tournament");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                if (tournament.TournamentImage != null)
                {
                    tournament.FileName = tournament.TournamentImage.FileName;
                    using (var stream = new FileStream(Path.Combine(directory, tournament.FileName), FileMode.Create))
                    {
                        await tournament.TournamentImage.CopyToAsync(stream);
                    }
                }
                var users = await _userManager.GetUserAsync(HttpContext.User);
                var tournamentModal = _mapper.Map<Tournament>(tournament);
                tournamentModal.UserId = users.Id;
                _context.Tournaments.Update(tournamentModal);
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }

        [HttpDelete("Tournament/DeleteConfirmed/{tournamentId}")]
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
