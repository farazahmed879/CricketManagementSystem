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


    public class MatchSummaryController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMatchSummary _matchSummary;
        private IHostingEnvironment _env;
        private readonly IHostingEnvironment _hosting;

        public MatchSummaryController(
            CricketContext context,
            UserManager<ApplicationUser> userManager, IMatchSummary matchSummary,
            IMapper mapper, IHostingEnvironment env, IHostingEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _matchSummary = matchSummary;
            _env = env;
            _hosting = hosting;
        }

        [HttpGet]
        public async Task<IActionResult> FirstInning(int matchId, int homeTeamId, int oppTeamId, string inning)
        {
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;
            ViewBag.Inning = inning;
            var model = await _matchSummary.FirstInning(matchId, homeTeamId,oppTeamId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SecondInning(int matchId, int homeTeamId, int oppTeamId, string inning)
        {
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;
            ViewBag.Inning = inning;
            var model = await _matchSummary.SecondInning(matchId, homeTeamId, oppTeamId);
            return View(model);
        }


        // GET: MatchSummary
        [HttpGet]
        public IActionResult Summary(int matchId, int homeTeamId, int oppTeamId, bool isApi)
        {
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;
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
            
            s.Select(i => i.HomeTeamTeamLogo ?? "noLogo.png");
            s.Select(i => i.OpponentTeamLogo ?? "noLogo.png");
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

        [HttpGet]
        public IActionResult Index(int matchId, int homeTeamId, int oppTeamId)
        {
            var matchSummary = new Summary();
            var connection = _context.Database.GetDbConnection();
            var summary = connection.Query<Summary2dto>(
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
           
            matchSummary.Summary2dto = summary.SingleOrDefault();
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;
            return View(matchSummary);
        }
    }
}
