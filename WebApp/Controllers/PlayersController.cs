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
using CricketApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApp.Helper;
using WebApp.IServices;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Controllers
{

    public class PlayersController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly IPlayers _players;
        private readonly IHostingEnvironment _hosting;

        public PlayersController(CricketContext context,
            UserManager<IdentityUser<int>> userManager, IPlayers players, IHostingEnvironment hosting,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _players = players;
            _hosting = hosting;
        }

        // GET: Players

        [HttpGet]
        [Route("Players/GetAllPlayers")]
        public List<PlayersDropDowndto> GetAllPlayers()
        {
            var players = _players.GetAllPlayers();
            return players;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int? teamId, int? playerRoleId, int? battingStyleId, int? bowlingStyleId, string name, int? userId, int? page)
        {

            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players";
            ViewBag.PlayerRoleId = new SelectList(_context.PlayerRole
                .AsNoTracking()
                .Select(i => new { i.Name, i.PlayerRoleId }), "PlayerRoleId", "Name");

            ViewBag.BattingStyleId = new SelectList(_context.BattingStyle
               .AsNoTracking()
               .Select(i => new { i.Name, i.BattingStyleId }), "BattingStyleId", "Name");

            ViewBag.BowlingStyleId = new SelectList(_context.BowlingStyle
               .AsNoTracking()
               .Select(i => new { i.Name, i.BowlingStyleId }), "BowlingStyleId", "Name");
            if (users != null)
                userId = users.Id;


            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
           , "TeamId", "Team_Name");
            var model = await _players.GetAllPlayersList(teamId, playerRoleId, battingStyleId, bowlingStyleId, name, userId, page);
            return View(model);



        }


        [HttpGet]
        public async Task<IActionResult> List(int? teamId, int? playerRoleId, int? battingStyleId, int? bowlingStyleId, string name, int? userId, int? page)
        {

            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Players";
            ViewBag.PlayerRoleId = new SelectList(_context.PlayerRole
                .AsNoTracking()
                .Select(i => new { i.Name, i.PlayerRoleId }), "PlayerRoleId", "Name");

            ViewBag.BattingStyleId = new SelectList(_context.BattingStyle
               .AsNoTracking()
               .Select(i => new { i.Name, i.BattingStyleId }), "BattingStyleId", "Name");

            ViewBag.BowlingStyleId = new SelectList(_context.BowlingStyle
               .AsNoTracking()
               .Select(i => new { i.Name, i.BowlingStyleId }), "BowlingStyleId", "Name");

            if (users != null)
                userId = users.Id;


            ViewBag.TeamId = new SelectList(_context.Teams
                .AsNoTracking()
                .Select(i => new { i.TeamId, i.Team_Name })
           , "TeamId", "Team_Name");
            var model = await _players.GetAllPlayersList(teamId, playerRoleId, battingStyleId, bowlingStyleId, name, userId, page);
            //if (partialView)
            //    return PartialView("_PlayerPartial", model);
            //else
            return Json(model);



        }

        // GET: PlayerList
        [HttpGet]
        public async Task<IActionResult> PlayersList(int? teamId)
        {
            ViewBag.Name = "Players";

            return View(await _context.Teams
                           .AsNoTracking()
                           .Where(i => i.TeamId == teamId)
                           .Select(i => new TeamDetailsdto
                           {
                               TeamId = i.TeamId,
                               Team_Name = i.Team_Name,
                               FileName = i.FileName,
                               Zone = i.Zone,
                               Place = i.Place,
                               City = i.City,
                               TeamPlayers = i.Players != null && i.Players.Any() ?
                                               i.Players
                                               .Where(p => !p.IsDeactivated)
                                               .Select(o => new TeamPlayersdto
                                               {
                                                   PlayerId = o.PlayerId,
                                                   Player_Name = o.Player_Name,
                                               }).ToList() : null
                           })
                           .SingleAsync());

        }
        // GET: Players/Details/5
        [HttpGet]
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
        [HttpGet]
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
                    .Select(i => new { i.TeamId, i.Team_Name })
                    , "TeamId", "Team_Name");
            }

            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Playersdto player)
        {

            if (ModelState.IsValid)
            {

                //var form = Request.Form;
                //byte[] fileBytes = null;
                //if (player.PlayerImage != null)
                //{
                //    using (var stream = player.PlayerImage.OpenReadStream())
                //    {
                //        fileBytes = ReadStream(stream);
                //    }
                //}
                player.FileName = player.PlayerImage.FileName;
                if (player.PlayerImage.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(_hosting.WebRootPath,"Home","Images", "Players", player.FileName), FileMode.Create))
                    {
                        await player.PlayerImage.CopyToAsync(stream);
                    }
                }
                
                //player.PlayerLogo = fileBytes ?? null;
                
                _context.Players.Add(_mapper.Map<Player>(player));
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }

            return Json(ResponseHelper.UnSuccess());

        }
        //[HttpGet]
        //public static byte[] ReadStream(Stream input)
        //{
        //    byte[] buffer = new byte[16 * 1024];
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        int read;
        //        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            ms.Write(buffer, 0, read);
        //        }
        //        return ms.ToArray();
        //    }
        //}

        // GET: Players/Edit/5
        [HttpGet]
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
                .Select(i => new Playersdto
                {
                    PlayerId = i.PlayerId,
                    Player_Name = i.Player_Name,
                    PlayerRoleId = i.PlayerRoleId,
                    BattingStyleId = i.BattingStyleId,
                    BowlingStyleId = i.BowlingStyleId,
                    Contact = i.Contact,
                    CNIC = i.CNIC,
                    DOB = i.DOB.HasValue ? i.DOB.Value.ToShortDateString() : "",
                    Address = i.Address,
                    IsGuestorRegistered = i.IsGuestorRegistered,
                    IsDeactivated = i.IsDeactivated,
                    TeamId = i.TeamId,
                    FileName = i.FileName


                })
                .SingleOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // GET: Players/getPlayerbyId/5
        //[Authorize(Roles = "Club Admin,Administrator")]
        [HttpGet]
        public async Task<IActionResult> getPlayerbyId(int? id)
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
                .Select(i => new Playersdto
                {
                    PlayerId = i.PlayerId,
                    Player_Name = i.Player_Name,
                    PlayerRoleId = i.PlayerRoleId,
                    BattingStyleId = i.BattingStyleId,
                    BowlingStyleId = i.BowlingStyleId,
                    Contact = i.Contact,
                    CNIC = i.CNIC,
                    DOB = i.DOB.HasValue ? i.DOB.Value.ToShortDateString() : "",
                    Address = i.Address,
                    IsGuestorRegistered = i.IsGuestorRegistered,
                    IsDeactivated = i.IsDeactivated,
                    TeamId = i.TeamId

                })
                .SingleOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }
            return Json(player);
        }

        // POST: Players/Edit/5
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(Playersdto player)
        {
            if (ModelState.IsValid)
            {
                
                if (player.PlayerImage != null)
                {
                    player.FileName = player.PlayerImage.FileName;
                    using (var stream = new FileStream(Path.Combine(_hosting.WebRootPath, "Home", "Images", "Players", player.FileName), FileMode.Create))
                    {
                        await player.PlayerImage.CopyToAsync(stream);
                    }
                }
                _context.Players.Update(_mapper.Map<Player>(player));
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }



        // POST: Players/Delete/5
        [HttpDelete]
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

        [HttpGet]
        public IActionResult PlayerStatistics(int playerId, bool Api)
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
                        @paramPlayerId = playerId

                    },
                    commandType: CommandType.StoredProcedure) ?? new PlayerStatisticsdto
                    {

                    };

                if (Api)
                {
                    return Json(model);
                }
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }




        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
