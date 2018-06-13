using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using WebApp.ViewModels;
using Dapper;
using System.Data;

namespace WebApp.Controllers
{
    public class PlayerScoresController : Controller
    {
        private readonly CricketContext _context;

        public PlayerScoresController(CricketContext context)
        {
            _context = context;
        }

        [Route("PlayerScores/Index")]
        public async Task<IActionResult> Index(int? matchId, int? homeTeamId, int? oppTeamId, int? playerScoreId)
        {
            var scoreDto = new ScoreCarddto();
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;

            ViewBag.OpponentTeam = new SelectList(_context.Teams
                .AsNoTracking()
                .Where(i => i.TeamId == oppTeamId), "TeamId", "Team_Name");

            ViewBag.HomeTeam = new SelectList(_context.Teams
               .AsNoTracking()
               .Where(i => i.TeamId == homeTeamId), "TeamId", "Team_Name");

            scoreDto.HomeTeamScoreCard = _context.PlayerScores
                .AsNoTracking()
                .Where(i => i.Player.TeamId == homeTeamId && i.MatchId == matchId)
                .Select(i => new MatchSummarydto
                {
                    PlayerId = i.PlayerId,
                    Position = i.Position,
                    MatchId = i.MatchId,
                    Bowler = i.Bowler,
                    Catches = i.Catches,
                    IsPlayedInning = i.IsPlayedInning,
                    Ball_Runs = i.Ball_Runs,
                    Bat_Balls = i.Bat_Balls,
                    Bat_Runs = i.Bat_Runs,
                    Four = i.Four,
                    HowOut = i.HowOut,
                    Maiden = i.Maiden,
                    Overs = i.Overs,
                    RunOut = i.RunOut,
                    Six = i.Six,
                    Stump = i.Stump,
                    Wickets = i.Wickets,
                    Player = i.Player


                })
                .ToList();


            scoreDto.OpponentTeamScoreCard = _context.PlayerScores
               .AsNoTracking()
               .Where(i => i.Player.TeamId == oppTeamId && i.MatchId == matchId)
               .Select(i => new MatchSummarydto
               {
                   PlayerId = i.PlayerId,
                   Position = i.Position,
                   MatchId = i.MatchId,
                   Bowler = i.Bowler,
                   Catches = i.Catches,
                   IsPlayedInning = i.IsPlayedInning,
                   Ball_Runs = i.Ball_Runs,
                   Bat_Balls = i.Bat_Balls,
                   Bat_Runs = i.Bat_Runs,
                   Four = i.Four,
                   HowOut = i.HowOut,
                   Maiden = i.Maiden,
                   Overs = i.Overs,
                   RunOut = i.RunOut,
                   Six = i.Six,
                   Stump = i.Stump,
                   Wickets = i.Wickets,
                   Player = i.Player

               })
               .ToList();

            return View(scoreDto);

        }

        // GET: PlayerScores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerScore = await _context.PlayerScores
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.PlayerScoreId == id);
            if (playerScore == null)
            {
                return NotFound();
            }

            return View(playerScore);
        }

        // GET: PlayerScores/Create
        public IActionResult Create(int homeTeamId, int oppTeamId, int matchId)
        {
            ViewBag.OpponentTeam = new SelectList(_context.Teams
               .AsNoTracking()
               .Where(i => i.TeamId == oppTeamId), "TeamId", "Team_Name");

            ViewBag.HomeTeam = new SelectList(_context.Teams
               .AsNoTracking()
               .Where(i => i.TeamId == homeTeamId), "TeamId", "Team_Name");

            ViewBag.HomePlayerId = new SelectList(_context.Players
                .AsNoTracking()
                .Where(i => i.TeamId == homeTeamId), "PlayerId", "Player_Name");

            ViewBag.OpponentPlayerId = new SelectList(_context.Players
             .AsNoTracking()
             .Where(i => i.TeamId == oppTeamId), "PlayerId", "Player_Name");
            var model = new List<MatchScoreDto>(12);
            for (int i = 0; i < 12; i++)
            {
                model.Add(new MatchScoreDto
                {
                    MatchId = matchId
                });
            }

            ViewBag.homeTeamId = homeTeamId;

            return View(model);
        }

        // POST: PlayerScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromForm] List<MatchScoreDto> Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {
                _context.AddRange(Matchplayers.Select(i => new PlayerScore
                {
                    Position = i.Position,
                    IsPlayedInning = i.IsPlayedInning,
                    PlayerId = i.PlayerId,
                    HowOut = i.HowOut,
                    Bowler = i.Bowler,
                    MatchId = i.MatchId,
                }
                ));
                await _context.SaveChangesAsync();
                return Ok();
                // return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("PlayerScores/PlayerScoreModal")]
        public async Task<IActionResult> PlayerScoreModal([FromBody]MatchScoreDto playerScores)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var playercoreCard = _context.PlayerScores
                        .AsNoTracking()
                        .Where(i => i.PlayerScoreId == playerScores.PlayerScoreId)
                        .SingleOrDefault();
                    playercoreCard.Bat_Runs = playerScores.Bat_Runs;
                    playercoreCard.Bat_Balls = playerScores.Bat_Balls;
                    playercoreCard.Four = playerScores.Four;
                    playercoreCard.Six = playerScores.Six;
                    playercoreCard.Ball_Runs = playerScores.Ball_Runs;
                    playercoreCard.Overs = playerScores.Overs;
                    playercoreCard.Wickets = playerScores.Wickets;
                    playercoreCard.Maiden = playerScores.Maiden;
                    playercoreCard.RunOut = playerScores.RunOut;
                    playercoreCard.Catches = playerScores.Catch;
                    playercoreCard.Stump = playerScores.Stump;
                    _context.Update(playercoreCard);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        // GET: PlayerScores/Edit/5
        public async Task<IActionResult> Edit(int? teamId, int? matchId)
        {
            //ViewBag.matchId = matchId;
            ViewBag.teamId = teamId;

            if (teamId != 0 && teamId != null)
            {
                ViewBag.PlayerId = new SelectList(_context.Players
                .AsNoTracking()
                .Where(i => i.TeamId == teamId), "PlayerId", "Player_Name");
            }

            var playerScores = await _context.PlayerScores
                .Where(m => m.MatchId == matchId)
                .Select(i => new MatchScoreDto
                {
                    Position = i.Position,
                    IsPlayedInning = i.IsPlayedInning,
                    PlayerId = i.PlayerId,
                    HowOut = i.HowOut,
                    Bowler = i.Bowler,
                    MatchId = i.MatchId
                })
                .ToListAsync();
            if (playerScores == null)
            {
                return NotFound();
            }
            return View(playerScores);
        }

        // POST: PlayerScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(List<MatchScoreDto> Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var matchId = Matchplayers.Select(i => i.MatchId).First();
                    var playerScores = _context.PlayerScores
                        .AsNoTracking()
                        .Where(i => i.MatchId == matchId)
                        .ToList();

                    foreach (var playerScore in playerScores)
                    {
                        var updateModel = Matchplayers.Where(i => i.PlayerId == playerScore.PlayerId).Single();
                        playerScore.Position = updateModel.Position;
                        playerScore.IsPlayedInning = updateModel.IsPlayedInning;
                        playerScore.PlayerId = updateModel.PlayerId;
                        playerScore.HowOut = updateModel.HowOut;
                        playerScore.Bowler = updateModel.Bowler;
                    }
                    _context.PlayerScores.UpdateRange(playerScores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
                return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return View();
        }

        // GET: PlayerScores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerScore = await _context.PlayerScores
                .SingleOrDefaultAsync(m => m.PlayerScoreId == id);
            if (playerScore == null)
            {
                return NotFound();
            }

            return View(playerScore);
        }

        // POST: PlayerScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerScore = await _context.PlayerScores.SingleOrDefaultAsync(m => m.PlayerScoreId == id);
            _context.PlayerScores.Remove(playerScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Add: PlayerScores/AddBating/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBat(int playerId, int matchId, MatchScoreDto playerScore)
        {
            if (playerId != playerScore.PlayerId && matchId != playerScore.MatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.PlayerScores.Where(i => i.PlayerId == playerId && i.MatchId == matchId);
                    _context.Update(playerScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerScoreExists(playerScore.PlayerScoreId))
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
            return View(playerScore);
        }



        private bool PlayerScoreExists(int id)
        {
            return _context.PlayerScores.Any(e => e.PlayerScoreId == id);
        }
    }
}
