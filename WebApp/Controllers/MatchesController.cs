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
using WebApp.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApp.Helper;
using System.Data;
using Dapper;
using WebApp.IServices;
using Microsoft.AspNetCore.Hosting;
using System;

namespace WebApp.Controllers
{

    public class MatchesController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly IMatches _matches;
        private IHostingEnvironment _env;

        public MatchesController(
            CricketContext context,
            UserManager<IdentityUser<int>> userManager,IMatches matches,
            IMapper mapper, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _matches = matches;
            _env = env;
        }

        // GET: Matches
        [Route("Matches/Index")]
        public async Task<IActionResult> Index(int? teamId, int? matchTypeId,
                                               int? tournamentId, int? matchSeriesId,
                                                int? season, int? matchOvers, int? userId, int? page)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.Name = "Match";
            if (users != null)
                userId = users.Id;

            ViewBag.Overs = new SelectList(_context.Matches
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => i.MatchOvers)
                .ToList().Distinct(), "MatchOvers");

            ViewBag.Season = new SelectList(_context.Matches
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => i.Season)
                .ToList().Distinct(), "Season");

            ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => new { i.TournamentId, i.TournamentName })
           , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => new { i.MatchSeriesId, i.Name })
           , "MatchSeriesId", "Name");

            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => (!userId.HasValue || i.clubAdmin.UserId == userId))
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            var model = await _matches.GetAllMatches(teamId, matchTypeId, tournamentId, matchSeriesId, season, matchOvers, userId, page);
            return View(model);
        }

        [Route("Matches/List/teamId/{teamId}/matchTypeId/{matchTypeId}/tournamentId/{tournamentId}/matchSeriesId/{matchSeriesId}/season/{season}/matchOvers/{matchOvers}/userId/{userId}/page/{page}")]
        public async Task<IActionResult> List(int? teamId, int? matchTypeId,
                                              int? tournamentId, int? matchSeriesId,
                                               int? season, int? matchOvers, int? userId, int? page)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.Name = "Match";
            if (users != null)
                userId = users.Id;

            ViewBag.Overs = new SelectList(_context.Matches
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => i.MatchOvers)
                .ToList().Distinct(), "MatchOvers");

            ViewBag.Season = new SelectList(_context.Matches
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => i.Season)
                .ToList().Distinct(), "Season");

            ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");

            ViewBag.Tournament = new SelectList(_context.Tournaments
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => new { i.TournamentId, i.TournamentName })
           , "TournamentId", "TournamentName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
                .Where(i => (!userId.HasValue || i.UserId == userId))
                .Select(i => new { i.MatchSeriesId, i.Name })
           , "MatchSeriesId", "Name");

            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => (!userId.HasValue || i.clubAdmin.UserId == userId))
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            var model = await _matches.GetAllMatches(teamId, matchTypeId, tournamentId, matchSeriesId, season, matchOvers, userId, page);
            return Json(model);
        }

        // GET: MatchSummary
        public IActionResult Summary(int matchId, int homeTeamId, int oppTeamId, bool isApi)
        {
            ViewBag.Name = "Match Summary";
            var connection = _context.Database.GetDbConnection();
            var matchSummary = new Summary();

            var HomeTeamBatting = connection.Query<HomeTeamBatting>(
               "[usp_HomeTeamBatting]",
               new
               {
                   paramMatchId = matchId,
                   paramHomeTeamId = homeTeamId,

               },
               commandType: CommandType.StoredProcedure) ?? new List<HomeTeamBatting>()
               {
               };
            var HomeTeamBowling = connection.Query<HomeTeamBowling>(
              "[usp_HomeTeamBowling]",
              new
              {
                  paramMatchId = matchId,
                  paramHomeTeamId = homeTeamId,

              },
              commandType: CommandType.StoredProcedure) ?? new List<HomeTeamBowling>()
              {
              };
            var OppTeamBatting = connection.Query<OppTeamBatting>(
             "[usp_OppTeamBatting]",
             new
             {
                 paramMatchId = matchId,
                 paramOppTeamId = oppTeamId,

             },
             commandType: CommandType.StoredProcedure) ?? new List<OppTeamBatting>()
             {
             };
            var OppTeamBowling = connection.Query<OppTeamBowling>(
             "[usp_OppTeamBowling]",
             new
             {
                 paramMatchId = matchId,
                 paramOppTeamId = oppTeamId,

             },
             commandType: CommandType.StoredProcedure) ?? new List<OppTeamBowling>()
             {
             };

            var s = connection.Query<Summary2dto>(
                "[usp_Summary2]",
                new
                {
                    paramMatchId = matchId,
                    paramHomeTeamId = homeTeamId,
                    paramOpponentTeamId = oppTeamId
                },
                commandType: CommandType.StoredProcedure) ?? new List<Summary2dto>()
                {
                };
            matchSummary.OppTeamBatting = OppTeamBatting.ToList();
            matchSummary.OppTeamBowling = OppTeamBowling.ToList();
            matchSummary.HomeTeamBatting = HomeTeamBatting.ToList();
            matchSummary.HomeTeamBowling = HomeTeamBowling.ToList();
            matchSummary.Summary2dto = s.SingleOrDefault();
            if (isApi)
            {
                return Json(matchSummary);
            }
            return View(matchSummary);
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? matchId)
        {
            ViewBag.Name = "Match Detail";
            if (matchId == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Select(i => new Matchdto
                {
                    MatchId = i.MatchId,
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppponentTeam = i.OppponentTeam.Team_Name,
                    DateOfMatch = i.DateOfMatch.HasValue ? i.DateOfMatch.Value.ToShortDateString() : "",
                    GroundName = i.GroundName,
                    Season = i.Season,
                    MatchType = i.MatchType.MatchTypeName,
                    Tournament = i.Tournament.TournamentName,
                    Series = i.MatchSeries.Name,
                    MatchOvers = i.MatchOvers,
                    Place = i.Place,
                    Result = i.Result,
                    PlayerOfTheMatch = i.Player.Player_Name


                })
                .SingleOrDefaultAsync(m => m.MatchId == matchId);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        //GET: Matches/Create

        [Authorize(Roles = "Club Admin,Administrator")]
        [Route("Matches/Create")]
        public async Task<IActionResult> Create(int? tournamentId, int? matchSeriesId)
        {
            ViewBag.Name = "Add Match";
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => i.clubAdmin.UserId == users.Id)
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            ViewBag.PlayerOTM = new SelectList(_context.Players
                        .Where(i => i.Team.clubAdmin.UserId == users.Id)
                      .Select(i => new { i.PlayerId, i.Player_Name })
                      , "PlayerId", "Player_Name");

            if (tournamentId != null && matchSeriesId == null)
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
            else if (matchSeriesId != null && tournamentId == null)
            {
                ViewBag.IsMatchSeries = true;
                ViewBag.MatchSeries = new SelectList(_context.MatchSeries
                    .Where(i => i.UserId == users.Id)
                    .Select(i => new { i.MatchSeriesId, i.Name })
                    , "MatchSeriesId", "Name", matchSeriesId);

                ViewBag.MatchType = new SelectList(_context.MatchType
                    .Select(i => new { i.MatchTypeId, i.MatchTypeName })
                    , "MatchTypeId", "MatchTypeName", 3);
            }
            else
            {
                ViewBag.MatchType = new SelectList(_context.MatchType
                    .Select(i => new { i.MatchTypeId, i.MatchTypeName })
                    , "MatchTypeId", "MatchTypeName");

                ViewBag.MatchSeries = new SelectList(_context.MatchSeries
                    .Where(i => i.UserId == users.Id)
                    .Select(i => new { i.MatchSeriesId, i.Name })
                    , "MatchSeriesId", "Name");

                ViewBag.TournamentId = new SelectList(_context.Tournaments
                    .Where(i => i.UserId == users.Id)
                     .Select(i => new { i.TournamentId, i.TournamentName })
                    , "TournamentId", "TournamentName");
            }

            return View();
        }


        [HttpPost]
        [Route("Matches/Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Matchdto match)
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
                _context.Matches.Add(_mapper.Map<Match>(match));
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
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
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["TeamId"] = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            ViewBag.PlayerOTM = new SelectList(_context.Players
                     .Where(i => i.Team.clubAdmin.UserId == users.Id)
                   .Select(i => new { i.PlayerId, i.Player_Name })
                   , "PlayerId", "Player_Name");

            ViewBag.MatchType = new SelectList(_context.MatchType
                    .Select(i => new { i.MatchTypeId, i.MatchTypeName })
                    , "MatchTypeId", "MatchTypeName");

            ViewBag.MatchSeries = new SelectList(_context.MatchSeries
                .Where(i => i.UserId == users.Id)
                .Select(i => new { i.MatchSeriesId, i.Name })
                , "MatchSeriesId", "Name");

            ViewBag.TournamentId = new SelectList(_context.Tournaments
                .Where(i => i.UserId == users.Id)
                 .Select(i => new { i.TournamentId, i.TournamentName })
                , "TournamentId", "TournamentName");

            var match = await _context.Matches
                .AsNoTracking()
                .Select(i => new Matchdto
                {
                    MatchId = i.MatchId,
                    GroundName = i.GroundName,
                    MatchImage = i.MatchImage,
                    MatchLogo = i.MatchLogo,
                    MatchOvers = i.MatchOvers,
                    MatchSeriesId = i.MatchSeriesId,
                    MatchTypeId = i.MatchTypeId,
                    HomeTeamId = i.HomeTeamId,
                    OppponentTeamId = i.OppponentTeamId,
                    DateOfMatch = i.DateOfMatch.HasValue ? i.DateOfMatch.Value.ToShortDateString() : "",
                    Place = i.Place,
                    Result = i.Result,
                    Season = i.Season,
                    TournamentId = i.TournamentId,
                    PlayerOTM = i.PlayerOTM,
                    HomeTeamOvers = i.HomeTeamOvers,
                    OppTeamOvers = i.OppTeamOvers


                })
                .SingleOrDefaultAsync(m => m.MatchId == id);
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
        public async Task<IActionResult> Edit(Matchdto match)
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
                _context.Update(_mapper.Map<Match>(match));
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }



        // POST: Matches/Delete/5
        [Route("Matches/DeleteConfirmed")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int matchId)
        {
            var match = await _context.Matches
                .SingleOrDefaultAsync(m => m.MatchId == matchId);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult MatchChartJson(int matchId)
        {
            ViewBag.MatchId = matchId;
            var graph = _context.FallOFWickets
                .Where(i => i.MatchId == matchId)
                .Select(i => new WebApp.ViewModels.FallOfWicketdto
                {
                    First = i.First,
                    Second = i.Second,
                    Third = i.Third,
                    Fourth = i.Fourth,
                    Fifth = i.Fifth,
                    Sixth = i.Sixth,
                    Seventh = i.Seventh,
                    Eight = i.Eight,
                    Ninth = i.Ninth,
                    Tenth = i.Tenth

                }).ToList();
            return Json(graph);

        }
        public IActionResult MatchChart(int matchId)
        {
            ViewBag.MatchId = matchId;
            ViewBag.MatchName = _context.FallOFWickets
                .Where(i => i.MatchId == matchId)
                .Select(i => i.Team.Team_Name).FirstOrDefault();
            return View();

        }

        public IActionResult ScreenShot([FromBody]ScreenShotdto screenShotdto)
        {
            screenShotdto.fileName = screenShotdto.fileName + ".png";
            var directory = Path.Combine(_env.WebRootPath, "ScreenShots");
            bool exists = Directory.Exists(directory);

            if (!exists)
                Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, screenShotdto.fileName);
            byte[] bytes = Convert.FromBase64String(screenShotdto.baseUrl);
            System.IO.File.WriteAllBytes(filePath, bytes);
            return Json($"http://{HttpContext.Request.Host}{(filePath.Substring(filePath.IndexOf(_env.WebRootPath) + _env.WebRootPath.Length)).Replace('\\', '/')}");
        }

    }
}
