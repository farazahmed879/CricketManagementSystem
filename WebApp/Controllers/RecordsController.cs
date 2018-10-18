using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dapper;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{

    public class RecordsController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public RecordsController(CricketContext context,
            UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // View: Records/Index
        public IActionResult Index()
        {
            ViewBag.Name = "Records";
            return View();
        }
        // GET: Batting
        public async Task<IActionResult> Batting(int? teamId, int? season, int? overs,
            int? position, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players Records";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.UserId == users.Id))
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.UserId == users.Id))
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.clubAdmin.UserId == users.Id))
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.UserId == users.Id))
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
               .Where(i => (!userId.HasValue || i.UserId == users.Id))
               .Select(i => new { i.MatchSeriesId, i.Name })
               , "MatchSeriesId", "Name");

            ViewBag.PlayerRole = new SelectList(_context.PlayerRole
                .AsNoTracking()
                , "PlayerRoleId", "Name");

            ViewBag.MatchType = new SelectList(_context.MatchType
                .AsNoTracking(), "MatchTypeId", "MatchTypeName");

            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = connection.Query<BattingRecorddto>(
                    "[usp_GetAllBattingStatistics]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramPosition = position,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<BattingRecorddto>();
                if (isApi)
                {
                    return Json(model);
                }
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (teamId != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }
        // GET: Bowling
        public async Task<IActionResult> Bowling(int? teamId, int? season, int? overs, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players Records";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.UserId == users.Id))
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.UserId == users.Id))
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.clubAdmin.UserId == users.Id))
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.TournamentId = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Where(i => (!userId.HasValue || i.UserId == users.Id))
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");
            
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole
              .AsNoTracking()
              , "PlayerRoleId", "Name");

            ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");

            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = connection.Query<BowlingRecorddto>(
                    "[usp_GetAllBowlingStatistics]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<BowlingRecorddto>();
                if (isApi)
                {
                    return Json(model);
                }
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (teamId != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

    }
}
