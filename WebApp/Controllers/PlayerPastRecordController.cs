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

        // GET: PlayerPastRecord/Create/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create(int? playerId)
        {
            return View();
        }

        // GET: PlayerPastRecord/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> PastRecord(int? playerId)
        {
            ViewBag.Name = "Past Record";
            if (playerId == null)
            {
                return NotFound();
            }

            var playerPastRecord = await _context.PlayerPastRecord
                .AsNoTracking()
                .ProjectTo<PlayerPastRecorddto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(m => m.PlayerId == playerId);
            if(playerPastRecord == null)
            {
                playerPastRecord = new PlayerPastRecorddto();
            }
            return View(playerPastRecord);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> PastRecordSave(int id, PlayerPastRecorddto playerPastRecord)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.PlayerPastRecord.Update(_mapper.Map<PlayerPastRecord>(playerPastRecord));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(playerPastRecord.PlayerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Players");
            }
            return View(playerPastRecord);
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}
