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
using WebApp.Helper;
using WebApp.IServices;

namespace WebApp.Controllers
{

    public class PlayerPastRecordController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly IPlayers _players;

        public PlayerPastRecordController(CricketContext context,
            UserManager<IdentityUser<int>> userManager, IPlayers players,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _players = players;

        }


        // GET: PlayerPastRecord
        [HttpGet]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> PastRecord(int? playerId,string PlayerName)
        {
            ViewBag.Name = "Past Record";
            if (playerId == null)
            {
                return NotFound();
            }

            ViewBag.PlayerName = PlayerName;
            var playerPastRecord = await _players.GetPlayerPastRecordByPlayerId(playerId);
            return View(playerPastRecord);
        }

        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> PastRecordSave(PlayerPastRecorddto playerPastRecord)
        {

            if (ModelState.IsValid)
            {
                _context.Update(playerPastRecord);
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }

    }
}
