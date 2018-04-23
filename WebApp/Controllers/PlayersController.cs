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
using WebApp.Utilities;
using Dapper;
using WebApp.ViewModels;
using System.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PlayersController : Controller
    {
        private readonly CricketContext _context;

        public PlayersController(CricketContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(int? teamId, string role, string playerName, int? page)
        {
            ViewBag.TeamId = new SelectList(_context.Teams, "TeamId", "Team_Name");
            int pageSize = 10;
            return View(await PaginatedList<Player>.CreateAsync(
                             _context.Players
                           .AsNoTracking()
                           .Where(i => (!teamId.HasValue || i.TeamId == teamId) &&
                                        (string.IsNullOrEmpty(role) || (i.Role == role)) &&
                                        (string.IsNullOrEmpty(playerName) || (i.Player_Name == playerName)))
                           .Include(i => i.Team), page ?? 1, pageSize));


        }


        // GET: PlayerProfile
        public async Task<IActionResult> PlayerProfile(int? teamId)
        {
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
        public IActionResult Create()
        {

            ViewBag.TeamId = new SelectList(_context.Teams, "TeamId", "Team_Name");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(int? id)
        {
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.SingleOrDefaultAsync(m => m.PlayerId == id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: PlayerStatistics
        public IActionResult PlayerStatistics(int playerId)
        {
            var connection = _context.Database.GetDbConnection();
            var model = connection.QuerySingle<PlayerStatisticsdto>(
                "[usp_GetMatchStatistics]",
                new { paramPlayerId = playerId },
                commandType: CommandType.StoredProcedure);

            return View(model);
        }
        // GET: AllPlayerStatistics
        public IActionResult AllPlayerStatistics(int teamId)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var model = connection.Query<AllPlayerStatisticsdto>(
                    "[usp_GetAllPlayerStatistics]",
                    new { paramTeamId = teamId },
                    commandType: CommandType.StoredProcedure);
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
