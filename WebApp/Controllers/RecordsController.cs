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

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Name = "Records";
            return View();
        }
        // GET: Batting
        [HttpGet]
        public async Task<IActionResult> Batting(int? teamId, int? season, int? overs,
            int? position, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players Records";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

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
        [HttpGet]
        public async Task<IActionResult> Bowling(int? teamId, int? season, int? overs, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players Records";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.TournamentId = new SelectList(_context.Tournaments
                .AsNoTracking()
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
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

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
        // Get: MostRuns
        [HttpGet]
        public async Task<IActionResult> MostRuns(int? teamId, int? season, int? overs,
          int? position, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most Runs";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostRunsdto>(
                    "[usp_GetMostRuns]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramPosition = position,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostRunsdto>();
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

        // Get: MostFours
        [HttpGet]
        public async Task<IActionResult> MostFours(int? teamId, int? season, int? overs,
          int? position, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most Fours";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostFoursdto>(
                    "[usp_GetMostFours]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramPosition = position,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostFoursdto>();
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


        // Get: MostSixes
        [HttpGet]
        public async Task<IActionResult> MostSixes(int? teamId, int? season, int? overs,
          int? position, int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most Six";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostSixesdto>(
                    "[usp_GetMostSixes]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramPosition = position,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostSixesdto>();
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


        // Get: MostWickets
        [HttpGet]
        public async Task<IActionResult> MostWickets(int? teamId, int? season, int? overs,
            int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most Runs";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostWicketsdto>(
                    "[usp_GetMostWickets]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostWicketsdto>();
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

        // Get: MostCatches
        [HttpGet]
        public async Task<IActionResult> MostCatches(int? teamId, int? season, int? overs,
            int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most Catch";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostCatchesdto>(
                    "[usp_GetMostCatches]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostCatchesdto>();
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

        // Get: MostStumps
        [HttpGet]
        public async Task<IActionResult> MostStumps(int? teamId, int? season, int? overs,
            int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most Catch";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostStumpsdto>(
                    "[usp_GetMostStumps]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostStumpsdto>();
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

        // Get: MostFifties
        [HttpGet]
        public async Task<IActionResult> MostFifties(int? teamId, int? season, int? overs, int? position,
            int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most 50s";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostFiftiesdto>(
                    "[usp_GetMostFifties]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId,
                        paramPosition = position

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostFiftiesdto>();
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
        // Get: MostFifties
        [HttpGet]
        public async Task<IActionResult> MostCenturies(int? teamId, int? season, int? overs, int? position,
            int? matchTypeId, int? tournamentId, int? matchseriesId, int? playerRoleId, int? userId, bool isApi)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Most 100s";


            if (users != null)
                userId = users.Id;

            ViewBag.Season = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.Season).ToList().Distinct(), "Season");


            ViewBag.Overs = new SelectList(_context.Matches
                .AsNoTracking()
                .Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                  , "TeamId", "Team_Name", teamId);

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .AsNoTracking()
                .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
               .AsNoTracking()
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
                var model = connection.Query<MostCenturiesdto>(
                    "[usp_GetMostCenturies]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId,
                        paramMatchseriesId = matchseriesId,
                        paramPlayerRoleId = playerRoleId,
                        paramUserId = userId,
                        paramPosition = position,


                    },
                    commandType: CommandType.StoredProcedure) ?? new List<MostCenturiesdto>();
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
