using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CricketApp.Domain;
using System.IO;
using Dapper;
using WebApp.ViewModels;
using System.Data;
using WebApp.Models;
using CricketApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace WebApp.Controllers
{

    public class PlayersController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;

        public PlayersController(CricketContext context,
            UserManager<IdentityUser<int>> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Players

        public async Task<IActionResult> Index(int? teamId, int? playerRoleId, int? userId, int? page)
        {

            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players";
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole
                .AsNoTracking()
                .Select(i => new { i.Name, i.PlayerRoleId }), "PlayerRoleId", "Name");

            int pageSize = 10;
            if (users != null)
                userId = users.Id;


            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Where(i => userId.HasValue || i.clubAdmin.UserId == userId)
                .Select(i => new { i.TeamId, i.Team_Name })
           , "TeamId", "Team_Name");

            return View(await PaginatedList<ViewModels.Playersdto>.CreateAsync(
                            _context.Players
                          .AsNoTracking()
                          .Where(i => (!teamId.HasValue || i.TeamId == teamId)
                                        && (!playerRoleId.HasValue || i.PlayerRoleId == playerRoleId)
                                        && (!userId.HasValue || i.Team.clubAdmin.UserId == userId)
                                       )
                        .Select(i => new ViewModels.Playersdto
                        {
                            PlayerId = i.PlayerId,
                            Player_Name = i.Player_Name,
                            BattingStyle = i.BattingStyle.Name,
                            BowlingStyle = i.BowlingStyle.Name,
                            PlayerRole = i.PlayerRole.Name,
                            PlayerLogo = i.PlayerLogo,
                            DOB = i.DOB.HasValue ? i.DOB.Value.ToShortDateString() : "",
                            Team = i.Team.Team_Name,
                            //   PlayerRoleId = i.PlayerRoleId,
                            //   BowlingStyleId = i.BowlingStyleId,
                            // BattingStyleId = i.BattingStyleId,
                            //TeamId = i.TeamId

                        })
                          .OrderByDescending(i => i.PlayerId)
                            , page ?? 1, pageSize));



        }


        // GET: PlayerProfile

        public async Task<IActionResult> PlayerProfile(int? teamId)
        {
            ViewBag.Name = "Players";

            return View(await _context.Teams
                           .AsNoTracking()
                           .Where(i => i.TeamId == teamId)
                           .Select(i => new TeamDetailsdto
                           {
                               TeamId = i.TeamId,
                               Team_Name = i.Team_Name,
                               TeamLogo = i.TeamLogo,
                               Zone = i.Zone,
                               Place = i.Place,
                               City = i.City,                      
                               TeamPlayers = i.Players != null && i.Players.Any() ?
                                               i.Players.Select(o => new TeamPlayersdto
                                               {
                                                   PlayerId = o.PlayerId,
                                                   Player_Name = o.Player_Name,
                                                   PlayerLogo = o.PlayerLogo,
                                               }).ToList() : null
                           })
                           .SingleAsync());

        }
        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Name = "Player Detail";
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .SingleOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(int? teamId)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole
                .AsNoTracking()
                .Select(i => new { i.PlayerRoleId, i.Name })
                , "PlayerRoleId", "Name");

            ViewBag.BattingStyle = new SelectList(_context.BattingStyle
                    .AsNoTracking()
                    .Select(i => new { i.BattingStyleId, i.Name })
                , "BattingStyleId", "Name");

            ViewBag.BowlingStyle = new SelectList(_context.BowlingStyle
                .AsNoTracking()
                .Select(i => new { i.BowlingStyleId, i.Name })
                , "BowlingStyleId", "Name");
            ViewBag.Name = "Add Player";
            if (teamId != null)
            {
                ViewBag.HasTeamId = true;
                ViewBag.TeamId = new SelectList(_context.Teams
                    .AsNoTracking()
                    .Where(i => i.TeamId == teamId)
                    .Select(i => new { i.TeamId, i.Team_Name })
                    , "TeamId", "Team_Name");
            }
            else
            {
                ViewBag.TeamId = new SelectList(_context.Teams
                    .AsNoTracking()
                    .Where(i => i.clubAdmin.UserId == users.Id)
                    .Select(i => new { i.TeamId, i.Team_Name })
                    , "TeamId", "Team_Name");
            }

            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Playersdto player)
        {

            if (ModelState.IsValid)
            {

                var form = Request.Form;
                byte[] fileBytes = null;
                if (player.PlayerImage != null)
                {
                    using (var stream = player.PlayerImage.OpenReadStream())
                    {
                        fileBytes = ReadStream(stream);
                    }
                }
                //var file = await FileHelpers.ProcessFormFile(player.PlayerImage, ModelState);

                player.PlayerLogo = fileBytes ?? null;
                _context.Players.Add(_mapper.Map<Player>(player));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(player);

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

        // GET: Players/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Name = "Edit Mode";
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole
                .AsNoTracking()
                .Select(i => new { i.PlayerRoleId, i.Name })
                , "PlayerRoleId", "Name");

            ViewBag.BattingStyle = new SelectList(_context.BattingStyle
                .AsNoTracking()
                .Select(i => new { i.BattingStyleId, i.Name })
                , "BattingStyleId", "Name");

            ViewBag.BowlingStyle = new SelectList(_context.BowlingStyle
                .AsNoTracking()
                .Select(i => new { i.BowlingStyleId, i.Name })
                , "BowlingStyleId", "Name");

            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .AsNoTracking()
                .ProjectTo<Playersdto>(_mapper.ConfigurationProvider)
                .Include(i => i.Team)
                .SingleOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int id, Playersdto player)
        {
            if (id != player.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fokrm = Request.Form;
                    byte[] fileBytes = null;
                    if (player.PlayerImage != null)
                    {
                        using (var stream = player.PlayerImage.OpenReadStream())
                        {
                            fileBytes = ReadStream(stream);
                        }
                    }
                    player.PlayerLogo = fileBytes ?? null;
                    _context.Players.Update(_mapper.Map<Player>(player));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.PlayerId))
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
            return View(player);
        }



        // POST: Players/Delete/5
        [Route("Players/DeleteConfirmed")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int playerId)
        {
            var player = await _context.Players.SingleOrDefaultAsync(m => m.PlayerId == playerId);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // GET: PlayerStatistics
        public IActionResult PlayerStatistics(int playerId, int? matchOvers)
        {
            ViewBag.Name = "Players / Profile";
            ViewBag.Overs = new SelectList(_context.Matches.Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");
            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = connection.QuerySingleOrDefault<PlayerStatisticsdto>(
                    "[usp_GetSinglePlayerStatistics]",
                    new
                    {
                        @paramPlayerId = playerId,
                        @paramOvers = matchOvers

                    },
                    commandType: CommandType.StoredProcedure) ?? new PlayerStatisticsdto
                    {

                    };

                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        // GET: AllPlayerStatistics
        //[Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> AllPlayerStatistics(int? teamId, int? season, int? overs, int? position, int? matchTypeId, int? tournamentId, int? userId, bool isApi)
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


            ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");

            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = connection.Query<AllPlayerStatisticsdto>(
                    "[usp_GetAllPlayerStatistics]",
                    new
                    {
                        paramTeamId = teamId,
                        paramSeason = season,
                        paramOvers = overs,
                        paramPosition = position,
                        paramMatchType = matchTypeId,
                        paramTournamentId = tournamentId

                    },
                    commandType: CommandType.StoredProcedure) ?? new List<AllPlayerStatisticsdto>();
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




        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
