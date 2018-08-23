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

namespace WebApp.Controllers
{

    public class PlayersController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public PlayersController(CricketContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Players

        public async Task<IActionResult> Index(int? teamId, int? playerRoleId, int? page)
        {

            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players";
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole, "PlayerRoleId", "Name");
            ViewBag.TeamId = new SelectList(_context.Teams
                .Where(i => i.clubAdmin.UserId == users.Id)
                , "TeamId", "Team_Name");
            int pageSize = 10;
            if (users == null)
            {
                return View(await PaginatedList<Player>.CreateAsync(
                                 _context.Players
                               .AsNoTracking()
                               .Where(i => (!teamId.HasValue || i.TeamId == teamId)
                                             && (!playerRoleId.HasValue || i.PlayerRoleId == playerRoleId)
                                            )
                               .Include(i => i.Team)
                               .Include(i => i.PlayerRole)
                               .Include(i => i.BattingStyle)
                               .Include(i => i.BowlingStyle), page ?? 1, pageSize));
            }
            else
            {
                return View(await PaginatedList<Player>.CreateAsync(
                                _context.Players
                              .AsNoTracking()
                              .Where(i => (!teamId.HasValue || i.TeamId == teamId)
                                            && (!playerRoleId.HasValue || i.PlayerRoleId == playerRoleId)
                                            && (i.Team.clubAdmin.UserId == users.Id || users == null)
                                           )
                              .Include(i => i.Team)
                              .Include(i => i.PlayerRole)
                              .Include(i => i.BattingStyle)
                              .Include(i => i.BowlingStyle), page ?? 1, pageSize));
            }


        }


        // GET: PlayerProfile

        public async Task<IActionResult> PlayerProfile(int? teamId)
        {
            ViewBag.Name = "Players Profile";
            if (teamId != 0 && teamId != null)
            {
                return View(await _context.Players
                               .AsNoTracking()
                               .Where(i => i.TeamId == teamId)
                               .Include(i => i.Team)
                               .ToListAsync());
            }
            else
            {
                return View(await _context.Players
               .AsNoTracking()
               .Include(i => i.Team)
               .ToListAsync());
            }
        }
        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Name = "Match Detail";
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
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole, "PlayerRoleId", "Name");
            ViewBag.BattingStyle = new SelectList(_context.BattingStyle, "BattingStyleId", "Name");
            ViewBag.BowlingStyle = new SelectList(_context.BowlingStyle, "BowlingStyleId", "Name");
            ViewBag.Name = "Add Player";
            if (teamId != null)
            {
                ViewBag.HasTeamId = true;
                ViewBag.TeamId = new SelectList(_context.Teams
                    .AsNoTracking()
                    .Where(i => i.TeamId == teamId), "TeamId", "Team_Name");
            }
            else
            {
                ViewBag.TeamId = new SelectList(_context.Teams
                    .Where(i => i.clubAdmin.UserId == users.Id)
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
        public async Task<IActionResult> Create(Player player)
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
                _context.Add(player);
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
            ViewBag.PlayerRole = new SelectList(_context.PlayerRole, "PlayerRoleId", "Name");
            ViewBag.BattingStyle = new SelectList(_context.BattingStyle, "BattingStyleId", "Name");
            ViewBag.BowlingStyle = new SelectList(_context.BowlingStyle, "BowlingStyleId", "Name");
            ViewBag.TeamId = new SelectList(_context.Teams, "TeamId", "Team_Name");
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .AsNoTracking()
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
        public async Task<IActionResult> Edit(int id, Player player)
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
                    _context.Update(player);
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

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
            ViewBag.Overs = new SelectList(_context.Matches.Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");
            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = connection.QuerySingle<PlayerStatisticsdto>(
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
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult AllPlayerStatistics(int? teamId, int? season, int? overs, int? position, int? matchTypeId, int? tournamentId, int? opponentTeamId, bool isApi)
        {
            ViewBag.Name = "Players Records";
            ViewBag.Overs = new SelectList(_context.Matches.Select(i => i.MatchOvers).ToList().Distinct(), "MatchOvers");
            ViewBag.Season = new SelectList(_context.Matches.Select(i => i.Season).ToList().Distinct(), "Season");
            ViewBag.TeamId = new SelectList(_context.Teams, "TeamId", "Team_Name", teamId);
            ViewBag.MatchType = new SelectList(_context.MatchType, "MatchTypeId", "MatchTypeName");
            ViewBag.TournamentId = new SelectList(_context.Tournaments, "TournamentId", "TournamentName");
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
                        paramTournamentId = tournamentId,
                        paramOpponentTeamId = opponentTeamId

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
