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
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Index(int? matchId, int? homeTeamId, int? oppTeamId, int? playerScoreId)
        {
            ViewBag.Name = "Score Card";
            var scoreDto = new ScoreCarddto();
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;

            ViewBag.OpponentTeam = _context.Teams
                .AsNoTracking()
                .Where(i => i.TeamId == oppTeamId)
                .Select(i => i.Team_Name)
                .Single();
           

            ViewBag.HomeTeam = _context.Teams
               .AsNoTracking()
               .Where(i => i.TeamId == homeTeamId)
               .Select(i => i.Team_Name)
               .Single();

            scoreDto.HomeTeamScoreCard = _context.PlayerScores
                .AsNoTracking()
                .Where(i => i.Player.TeamId == homeTeamId && i.MatchId == matchId)
                .Select(i => new MatchSummarydto
                {
                    PlayerScoreId = i.PlayerScoreId,
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
                    HowOutId = i.HowOutId,
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
                   PlayerScoreId = i.PlayerScoreId,
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
                   HowOutId = i.HowOutId,
                   Maiden = i.Maiden,
                   Overs = i.Overs,
                   RunOut = i.RunOut,
                   Six = i.Six,
                   Stump = i.Stump,
                   Wickets = i.Wickets,
                   Player = i.Player

               })
               .ToList();

            scoreDto.TeamScoreCard = _context.TeamScores
                .AsNoTracking()
                .Include(i => i.Team)
                .Where(i => i.MatchId == matchId)
                .Select(i => new TeamScoredto
                {
                    TeamId = i.TeamId,
                    TotalScore = i.TotalScore,
                    Wideballs = i.Wideballs,
                    NoBalls = i.NoBalls,
                    Byes = i.Byes,
                    LegByes = i.LegByes


                }).ToList();

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
        [Authorize(Roles = "Admin , ClubUser")]
        public IActionResult Create(int homeTeamId, int oppTeamId, int matchId)
        {
            ViewBag.Name = "Add Match Players";
            ViewBag.HowOut = new SelectList(_context.HowOut, "HowOutId", "Name");
            ViewBag.OpponentTeam = _context.Teams
                 .AsNoTracking()
                 .Where(i => i.TeamId == oppTeamId)
                 .Select(i => i.Team_Name)
                 .Single();


            ViewBag.HomeTeam = _context.Teams
               .AsNoTracking()
               .Where(i => i.TeamId == homeTeamId)
               .Select(i => i.Team_Name)
               .Single();

            ViewBag.HomePlayerId = new SelectList(_context.Players
                .AsNoTracking()
                .Where(i => i.TeamId == homeTeamId), "PlayerId", "Player_Name");

            ViewBag.OpponentPlayerId = new SelectList(_context.Players
             .AsNoTracking()
             .Where(i => i.TeamId == oppTeamId), "PlayerId", "Player_Name");
            var model = new TeamMatchScoredto();
            for (int i = 0; i < 2; i++)

                model.TeamScore.Add(new TeamScoredto
                {
                    MatchId = matchId
                });


            for (int i = 0; i < 12; i++)
            {
                model.MatchScore.Add(new MatchScoreDto
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
        [Authorize(Roles = "Admin , ClubUser")]
        public async Task<IActionResult> Save([FromForm] TeamMatchScoredto Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {

                _context.AddRange(Matchplayers.MatchScore.Select(i => new PlayerScore
                {
                    Position = i.Position,
                    IsPlayedInning = i.IsPlayedInning,
                    PlayerId = i.PlayerId,
                    HowOutId = i.HowOutId,
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
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin , ClubUser")]
        public async Task<IActionResult> TeamScoreSave([FromForm] TeamMatchScoredto TeamScores)
        {
            if (ModelState.IsValid)
            {

                _context.AddRange(TeamScores.TeamScore.Select(i => new TeamScore
                {
                    TeamId = i.TeamId,
                    MatchId = i.MatchId,
                    TotalScore = i.TotalScore,
                    Wideballs = i.Wideballs,
                    NoBalls = i.NoBalls,
                    Byes = i.Byes,
                    LegByes = i.LegByes
                }
                ));

                await _context.SaveChangesAsync();
                return Ok();
                // return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin , ClubUser")]
        public async Task<IActionResult> UpdateTeamScore([FromForm] TeamMatchScoredto TeamScores)
        {
            if (ModelState.IsValid)
            {
                _context.UpdateRange(TeamScores.TeamScore.Select(i => new TeamScore
                {
                    TeamScoreId = i.TeamScoreId,
                    TeamId = i.TeamId,
                    MatchId = i.MatchId,
                    TotalScore = i.TotalScore,
                    Wideballs = i.Wideballs,
                    NoBalls = i.NoBalls,
                    Byes = i.Byes,
                    LegByes = i.LegByes
                }
                 ));


                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest(ModelState);

        }

        [HttpPost]
        [Route("PlayerScores/PlayerScoreModal")]
        [Authorize(Roles = "Admin , ClubUser")]
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
        // GET: PlayerScoresModal/Edit/5
        [Route("PlayerScores/GetPlayerScoreModal")]
        [Authorize(Roles = "Admin , ClubUser")]
        public IActionResult GetPlayerScoreModal(int playerScoreId)
        {

            var playerScore = _context.PlayerScores
      .AsNoTracking()
      .Where(i => i.PlayerScoreId == playerScoreId)
      .Select(i => new MatchSummarydto
      {
          Catches = i.Catches,
          Ball_Runs = i.Ball_Runs,
          Bat_Balls = i.Bat_Balls,
          Bat_Runs = i.Bat_Runs,
          Four = i.Four,
          Maiden = i.Maiden,
          Overs = i.Overs,
          RunOut = i.RunOut,
          Six = i.Six,
          Stump = i.Stump,
          Wickets = i.Wickets,
          PlayerScoreId = i.PlayerScoreId
      })
      .ToList();

            return Json(playerScore);
        }
        // GET: PlayerScores/Edit/5
        [Authorize(Roles = "Admin , ClubUser")]
        public IActionResult Edit(int? matchId, int? homeTeamId, int? oppTeamId, int? playerScoreId)
        {
            ViewBag.Name = "Edit Match Players";
            ViewBag.HowOut = new SelectList(_context.HowOut, "HowOutId", "Name");
            var scoreDto = new ScoreCarddto();
            //ViewBag.matchId = matchId;
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

            scoreDto.HomeTeamScoreCard = _context.PlayerScores
                .AsNoTracking()
                .Where(m => m.MatchId == matchId && m.Player.TeamId == homeTeamId)
                .Select(i => new MatchSummarydto
                {
                    PlayerScoreId = i.PlayerScoreId,
                    IsPlayedInning = i.IsPlayedInning,
                    PlayerId = i.PlayerId,
                    HowOutId = i.HowOutId,
                    Bowler = i.Bowler,
                    MatchId = i.MatchId
                })
                .ToList();

            if (matchId.HasValue)
                for (var index = 0; index < 12; index++)
                {
                    if (scoreDto.HomeTeamScoreCard.Count == index)
                    {
                        scoreDto.HomeTeamScoreCard.Add(new MatchSummarydto
                        {
                            MatchId = matchId.Value,
                        });
                    }
                }

            scoreDto.OpponentTeamScoreCard = _context.PlayerScores
             .AsNoTracking()
             .Where(m => m.MatchId == matchId && m.Player.TeamId == oppTeamId)
             .Select(i => new MatchSummarydto
             {
                 PlayerScoreId = i.PlayerScoreId,
                 IsPlayedInning = i.IsPlayedInning,
                 PlayerId = i.PlayerId,
                 HowOutId = i.HowOutId,
                 Bowler = i.Bowler,
                 MatchId = i.MatchId
             })
             .ToList();

            if (matchId.HasValue)
                for (var index = 0; index < 12; index++)
                {
                    if (scoreDto.OpponentTeamScoreCard.Count == index)
                    {
                        scoreDto.OpponentTeamScoreCard.Add(new MatchSummarydto
                        {
                            MatchId = matchId.Value,
                        });
                    }
                }



            scoreDto.TeamScoreCard = _context.TeamScores
           .AsNoTracking()
           .Where(m => m.MatchId == matchId)
           .Select(i => new TeamScoredto
           {
               TeamScoreId = i.TeamScoreId,
               TotalScore = i.TotalScore,
               Byes = i.Byes,
               LegByes = i.LegByes,
               Wideballs = i.Wideballs,
               NoBalls = i.NoBalls,
               MatchId = i.MatchId

           })
           .ToList();

            if (matchId.HasValue)
                for (var index = 0; index < 2; index++)
                {
                    if (scoreDto.TeamScoreCard.Count == index)
                    {
                        scoreDto.TeamScoreCard.Add(new TeamScoredto
                        {
                            MatchId = matchId.Value,
                        });
                    }
                }




            return View(scoreDto);
        }

        // POST: PlayerScores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin , ClubUser")]
        public async Task<IActionResult> Edit([FromForm] TeamMatchScoredto Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {
                _context.UpdateRange(Matchplayers.MatchScore.Select(i => new PlayerScore
                {
                    PlayerScoreId = i.PlayerScoreId,
                    Position = i.Position,
                    IsPlayedInning = i.IsPlayedInning,
                    PlayerId = i.PlayerId,
                    HowOutId = i.HowOutId,
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

        private bool PlayerScoreExists(int id)
        {
            return _context.PlayerScores.Any(e => e.PlayerScoreId == id);
        }
    }
}
