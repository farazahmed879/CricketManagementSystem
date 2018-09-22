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
        [Route("Matches/Index")]
        public async Task<IActionResult> Index(int? teamId, int? matchTypeId,
                                               int? tournamentId,
                                                int? season, int? overs, int? userId, int? page)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.Name = "Match";
            if (users != null)
                userId = users.Id;

            ViewBag.Overs = new SelectList(_context.Matches
                .Where(i => (userId.HasValue || i.UserId == userId))
                .Select(i => i.MatchOvers)
                .ToList().Distinct(), "MatchOvers");

            ViewBag.Season = new SelectList(_context.Matches
                .Where(i => (userId.HasValue || i.UserId == userId))
                .Select(i => i.Season)
                .ToList().Distinct(), "Season");

            ViewBag.MatchTypeId = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");
            int pageSize = 20;


            ViewBag.TournamentId = new SelectList(_context.Tournaments
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => new { i.TournamentId, i.TournamentName })
           , "TournamentId", "TournamentName");

            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => (!userId.HasValue || i.clubAdmin.UserId == userId))
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            return View(await PaginatedList<ViewModels.Matchdto>.CreateAsync(
                _context.Matches
                .AsNoTracking()
               .Where(i => (!matchTypeId.HasValue || i.MatchTypeId == matchTypeId) &&
                           (!teamId.HasValue || i.HomeTeamId == teamId || i.OppponentTeamId == teamId) &&
                           (!tournamentId.HasValue || i.TournamentId == tournamentId) && (!season.HasValue || i.Season == season) &&
                           (!overs.HasValue || i.MatchOvers == overs) && (!userId.HasValue || i.UserId == userId))
                .Select(i => new ViewModels.Matchdto
                {
                    MatchId = i.MatchId,
                    GroundName = i.GroundName,
                    DateOfMatch = i.DateOfMatch.HasValue ? i.DateOfMatch.Value.ToShortDateString() : "",
                    MatchOvers = i.MatchOvers,
                    Result = i.Result,
                    MatchType = i.MatchType.MatchTypeName,
                    TournamentId = i.TournamentId,
                    MatchTypeId = i.MatchTypeId,
                    Tournament = i.Tournament.TournamentName,
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppponentTeam = i.OppponentTeam.Team_Name,
                    HomeTeamId = i.HomeTeamId,
                    OppponentTeamId = i.OppponentTeamId,
                    MatchLogo = i.MatchLogo,
                    HasFilledHomeTeamData = i.PlayerScores.Any() && i.PlayerScores.Any(o => o.Player != null && o.Player.TeamId == i.HomeTeamId),
                    HasFilledOpponentTeamData = i.PlayerScores.Any() && i.PlayerScores.Any(o => o.Player != null && o.Player.TeamId == i.OppponentTeamId),
                    HasFilledTeamScoreData = i.TeamScores.Any() && i.TeamScores.Any(o => i.MatchId == i.MatchId)
                })
               .OrderBy(i => i.MatchId)
               .AsQueryable()
                                 , page ?? 1, pageSize));

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
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            if (tournamentId != null)
            {
                ViewBag.IsTournament = true;
                ViewBag.TournamentId = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Where(i => i.TournamentId == tournamentId && i.UserId == users.Id)
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName", tournamentId);
                ViewBag.MatchType = new SelectList(_context.MatchType
                    .Select(i => new { i.MatchTypeId, i.MatchTypeName })
                    , "MatchTypeId", "MatchTypeName", 2);
            }
            else
            {
                ViewBag.MatchType = new SelectList(_context.MatchType
                    .Select(i => new { i.MatchTypeId, i.MatchTypeName })
                    , "MatchTypeId", "MatchTypeName");
                ViewBag.TournamentId = new SelectList(_context.Tournaments
                    .Where(i => i.UserId == users.Id)
                     .Select(i => new { i.TournamentId, i.TournamentName })
                    , "TournamentId", "TournamentName");
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
                var users = await _userManager.GetUserAsync(HttpContext.User);
                match.UserId = users.Id;
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
            ViewBag.MatchType = new SelectList(_context.MatchType
                .Select(i => new { i.MatchTypeId, i.MatchTypeName })
                , "MatchTypeId", "Name");
            ViewData["TeamId"] = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");
            ViewBag.TournamentId = new SelectList(_context.Tournaments
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");
            var match = await _context.Matches.SingleOrDefaultAsync(m => m.MatchId == id);
            if (match == null)
            {
                return NotFound();
            }
            return View(match);
        }

        // POST: Matches/Edit/5
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
                    var users = await _userManager.GetUserAsync(HttpContext.User);
                    match.UserId = users.Id;
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
