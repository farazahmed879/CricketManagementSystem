using System;
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

namespace WebApp.Controllers
{
    public class MatchesController : Controller
    {
        private readonly CricketContext _context;

        public MatchesController(CricketContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index(int? homeTeamId, int? opponentTeamId,
                                               int? tournamentId, string result, string status,
                                                int? season, int? overs, int? page)
        {
            ViewBag.Overs = new SelectList(_context.Matches.Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");
            ViewBag.Season = new SelectList(_context.Matches.Select(i => i.Season).ToList().Distinct(), "Season");
            ViewBag.TournamentId = new SelectList(_context.Tournaments, "TournamentId", "TournamentName");
            ViewBag.TeamId = new SelectList(_context.Teams, "TeamId", "Team_Name");
            int pageSize = 10;
            return View(await PaginatedList<Match>.CreateAsync(
                 _context.Matches
                .AsNoTracking()
                .Where(i => (string.IsNullOrEmpty(result) || i.Result == result) && (string.IsNullOrEmpty(status) || i.GroundName == status) &&
                            (!homeTeamId.HasValue || i.HomeTeamId == homeTeamId) && (!opponentTeamId.HasValue || i.OppponentTeamId == opponentTeamId) &&
                            (!tournamentId.HasValue || i.TournamentId == tournamentId) && (!season.HasValue || i.Season == season) &&
                            (!overs.HasValue || i.MatchOvers == overs))
                .Include(i => i.HomeTeam)
                .Include(i => i.OppponentTeam)
                , page ?? 1, pageSize));


        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Matches/Create
        public IActionResult Create()
        {
            var ground = new SelectList(new[] { "Ground A", "Ground B" });
            ViewBag.TeamId = new SelectList(_context.Teams, "TeamId", "Team_Name");
            ViewBag.TournamentId = new SelectList(_context.Tournaments, "TournamentId", "TournamentName");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Team_Name");
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
        public async Task<IActionResult> Edit(int id, [Bind("MatchId,GroundName,Place,Type,Result,DateOfMatch,TeamId")] Match match)
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

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.SingleOrDefaultAsync(m => m.MatchId == id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.MatchId == id);
        }
    }
}
