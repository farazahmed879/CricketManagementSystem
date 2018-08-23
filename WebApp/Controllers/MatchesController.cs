using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using System.IO;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{

    public class MatchesController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public MatchesController(CricketContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Matches

        public async Task<IActionResult> Index(int? teamId, int? matchTypeId,
                                               int? tournamentId,
                                                int? season, int? overs, int? page)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Match";
            ViewBag.Overs = new SelectList(_context.Matches.Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");
            ViewBag.Season = new SelectList(_context.Matches.Select(i => i.Season).ToList().Distinct(), "Season");
            ViewBag.TournamentId = new SelectList(_context.Tournaments
                .Where(i => i.UserId == users.Id)
                , "TournamentId", "TournamentName");
            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => i.clubAdmin.UserId == users.Id)
                , "TeamId", "Team_Name");
            ViewBag.MatchTypeId = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");
            int pageSize = 10;
            if (users == null)
            {
                return View(await PaginatedList<Match>.CreateAsync(
                    _context.Matches
                   .AsNoTracking()
                   .Where(i => (!matchTypeId.HasValue || i.MatchTypeId == matchTypeId) &&
                               (!teamId.HasValue || i.HomeTeamId == teamId || i.OppponentTeamId == teamId) &&
                               (!tournamentId.HasValue || i.TournamentId == tournamentId) && (!season.HasValue || i.Season == season) &&
                               (!overs.HasValue || i.MatchOvers == overs))
                   .Include(i => i.HomeTeam)
                   .Include(i => i.FallOfWickets)
                   .Include(i => i.TeamScores)
                   .Include(i => i.MatchType)
                   .Include(i => i.OppponentTeam)
                   .Include(i => i.PlayerScores)
                   .ThenInclude(i => i.Player.Team)
                                     , page ?? 1, pageSize));

            }
            else
            {
                return View(await PaginatedList<Match>.CreateAsync(
                _context.Matches
               .AsNoTracking()
               .Where(i => (!teamId.HasValue || i.HomeTeamId == teamId || i.OppponentTeamId == teamId) || (!matchTypeId.HasValue || i.MatchTypeId == matchTypeId) &&
                           (!tournamentId.HasValue || i.TournamentId == tournamentId) && (!season.HasValue || i.Season == season) &&
                           (!overs.HasValue || i.MatchOvers == overs) && (i.Tournament.UserId == users.Id || users == null))
               .Include(i => i.HomeTeam)
                .Include(i => i.FallOfWickets)
               .Include(i => i.TeamScores)
               .Include(i => i.MatchType)
               .Include(i => i.OppponentTeam)
               .Include(i => i.PlayerScores)
               .ThenInclude(i => i.Player.Team)
                                 , page ?? 1, pageSize));

            }



        }


        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Name = "Match Detail";
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .SingleOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        //GET: Matches/Create

        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(int? tournamentId)
        {
            ViewBag.Name = "Add Match";
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => i.clubAdmin.UserId == users.Id)
                , "TeamId", "Team_Name");

            if (tournamentId != null)
            {
                ViewBag.IsTournament = true;
                ViewBag.TournamentId = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Where(i => i.TournamentId == tournamentId && i.UserId == users.Id), "TournamentId", "TournamentName", tournamentId);
                ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName", 2);
            }
            else
            {
                ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");
                ViewBag.TournamentId = new SelectList(_context.Tournaments
                    .Where(i => i.UserId == users.Id), "TournamentId", "TournamentName");
            }

            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Route("Matches/Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Match match)
        {
            if (ModelState.IsValid)
            {

                var form = Request.Form;
                byte[] fileBytes = null;
                if (match.MatchImage != null)
                {
                    using (var stream = match.MatchImage.OpenReadStream())
                    {
                        fileBytes = ReadStream(stream);

                    }
                }
                match.MatchLogo = fileBytes ?? null;
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(match);
        }
        public static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        // GET: Matches/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Name = "Edit Match";
            ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Team_Name");
            ViewBag.TournamentId = new SelectList(_context.Tournaments, "TournamentId", "TournamentName");
            var match = await _context.Matches.SingleOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int id, Match match)
        {
            if (id != match.MatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var form = Request.Form;
                    byte[] fileBytes = null;
                    if (match.MatchImage != null)
                    {
                        using (var stream = match.MatchImage.OpenReadStream())
                        {
                            fileBytes = ReadStream(stream);

                        }
                    }
                    match.MatchLogo = fileBytes ?? null;
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.MatchId))
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
            return View(match);
        }



        // POST: Matches/Delete/5
        [Route("Matches/DeleteConfirmed")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int matchId)
        {
            var match = await _context.Matches.SingleOrDefaultAsync(m => m.MatchId == matchId);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult MatchChart(int matchId)
        {

            var graph = _context.FallOFWickets.ToList();
            return View(graph);

        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.MatchId == id);
        }
    }
}
