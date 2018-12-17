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

namespace WebApp.Controllers
{

    public class PlayerPastRecordController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;

        public PlayerPastRecordController(CricketContext context,
            UserManager<IdentityUser<int>> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }


        // GET: PlayerPastRecord
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> PastRecord(int? playerId,string PlayerName)
        {
            ViewBag.Name = "Past Record";
            if (playerId == null)
            {
                return NotFound();
            }

            ViewBag.PlayerName = PlayerName;

            var playerPastRecord = await _context.PlayerPastRecord
                .AsNoTracking()
                .Select(i => new PlayerPastRecorddto
                {
                    PlayerPastRecordId = i.PlayerPastRecordId,
                    PlayerId = i.PlayerId,
                    TotalMatch = i.TotalMatch,
                    TotalInnings = i.TotalInnings,
                    TotalBatRuns = i.TotalBatRuns,
                    TotalBatBalls = i.TotalBatBalls,
                    TotalFours = i.TotalFours,
                    TotalSixes = i.TotalSixes,
                    NumberOf50s = i.NumberOf50s,
                    NumberOf100s = i.NumberOf100s,
                    TotalNotOut = i.TotalNotOut,
                    GetBowled = i.GetBowled,
                    GetCatch = i.GetCatch,
                    GetHitWicket = i.GetHitWicket,
                    GetLBW = i.GetLBW,
                    GetRunOut = i.GetRunOut,
                    GetStump = i.GetStump,
                    TotalOvers = i.TotalOvers,
                    TotalBallRuns = i.TotalBallRuns,
                    TotalWickets = i.TotalWickets,
                    TotalMaidens = i.TotalMaidens,
                    FiveWickets = i.FiveWickets,
                    DoBowled = i.DoBowled,
                    DoCatch = i.DoCatch,
                    DoHitWicket = i.DoHitWicket,
                    DoLBW = i.DoLBW,
                    DoStump = i.DoStump,
                    OnFieldCatch = i.OnFieldCatch,
                    OnFieldRunOut = i.OnFieldRunOut,
                    OnFieldStump = i.OnFieldStump,
                    BestScore = i.BestScore,


                })
                .SingleOrDefaultAsync(m => m.PlayerId == playerId);
            if (playerPastRecord == null)
            {
                playerPastRecord = new PlayerPastRecorddto();
            }
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

                _context.PlayerPastRecord.Update(_mapper.Map<PlayerPastRecord>(playerPastRecord));
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
