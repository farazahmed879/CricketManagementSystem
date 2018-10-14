using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using WebApp.ViewModels;
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
                .Include(i => i.HowOut)
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
                    Player = i.Player,
                    HowOutName = i.HowOut.Name



                })
                .OrderBy(i => i.Position)
                .ToList();


            scoreDto.OpponentTeamScoreCard = _context.PlayerScores
               .AsNoTracking()
               .Where(i => i.Player.TeamId == oppTeamId && i.MatchId == matchId)
               .Include(i => i.HowOut)
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
                   Player = i.Player,
                   HowOutName = i.HowOut.Name


               })
               .OrderBy(i => i.Position)
               .ToList();

            ViewBag.HomeTeamScore = _context.TeamScores
                .AsNoTracking()
                .Where(i => i.TeamId == homeTeamId && i.MatchId == matchId)
                .Select(i => i.TotalScore).Single();

            ViewBag.OppTeamScore = _context.TeamScores
                .AsNoTracking()
                .Where(i => i.TeamId == oppTeamId && i.MatchId == matchId)
                .Select(i => i.TotalScore).Single();

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
                    LegByes = i.LegByes,
                    TeamName = i.Team.Team_Name


                })
                .OrderBy(i => i.TeamScoreId)
                .ToList();

            scoreDto.FallOfWicket = _context.FallOFWickets
              .AsNoTracking()
              .Include(i => i.Team)
              .Where(i => i.MatchId == matchId)
              .Select(i => new FallOfWicketdto
              {
                  TeamId = i.TeamId,
                  MatchId = i.MatchId,
                  First = i.First,
                  Second = i.Second,
                  Third = i.Third,
                  Fourth = i.Fourth,
                  Fifth = i.Fifth,
                  Sixth = i.Sixth,
                  Seventh = i.Seventh,
                  Eight = i.Eight,
                  Ninth = i.Ninth,
                  Tenth = i.Tenth


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
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create(int homeTeamId, int oppTeamId, int matchId)
        {
            ViewBag.Name = "Add Match Players";
            ViewBag.HowOut = new SelectList(_context.HowOut
                .AsNoTracking()
                .Select(i => new { i.HowOutId, i.Name })
                , "HowOutId", "Name");

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

            ViewBag.homeTeamId = homeTeamId;
            ViewBag.opponentTeamId = oppTeamId;

            ViewBag.HomePlayerId = new SelectList(_context.Players
                .AsNoTracking()
                .Where(i => i.TeamId == homeTeamId)
                .Select(i => new { i.PlayerId, i.Player_Name })
                , "PlayerId", "Player_Name");

            ViewBag.OpponentPlayerId = new SelectList(_context.Players
             .AsNoTracking()
             .Where(i => i.TeamId == oppTeamId)
             .Select(i => new { i.PlayerId, i.Player_Name })
             , "PlayerId", "Player_Name");

            var model = new TeamMatchScoredto();
            for (int i = 0; i < 2; i++)

                model.TeamScore.Add(new TeamScoredto
                {
                    MatchId = matchId


                });
            for (int i = 0; i < 2; i++)

                model.FallOfWicket.Add(new FallOfWicketdto
                {
                    MatchId = matchId


                });


            for (int i = 0; i < 12; i++)
            {
                model.MatchScore.Add(new MatchScoreDto
                {
                    MatchId = matchId,

                });
            }



            return View(model);
        }

        // POST: PlayerScores/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
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
                    TeamId = i.TeamId
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
        [Authorize(Roles = "Club Admin,Administrator")]
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> FallOfWicketSave([FromForm] TeamMatchScoredto Model)
        {
            if (ModelState.IsValid)
            {

                _context.AddRange(Model.FallOfWicket.Select(i => new FallOfWicket
                {
                    TeamId = i.TeamId,
                    MatchId = i.MatchId,
                    First = i.First,
                    Second = i.Second,
                    Third = i.Third,
                    Fourth = i.Fourth,
                    Fifth = i.Fifth,
                    Sixth = i.Sixth,
                    Seventh = i.Seventh,
                    Eight = i.Eight,
                    Ninth = i.Ninth,
                    Tenth = i.Tenth
                }
                ));

                await _context.SaveChangesAsync();
                return Ok();
                // return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> UpdateTeamScore([FromForm] TeamMatchScoredto model)
        {
            if (ModelState.IsValid)
            {
                foreach (var teamScore in model.TeamScore)
                {
                    var teamScoreDbModel = new TeamScore
                    {
                        TeamScoreId = teamScore.TeamScoreId
                    };
                    _context.TeamScores.Attach(teamScoreDbModel);

                    teamScoreDbModel.TeamScoreId = teamScore.TeamScoreId;
                    teamScoreDbModel.TeamId = teamScore.TeamId;
                    teamScoreDbModel.MatchId = teamScore.MatchId;
                    teamScoreDbModel.TotalScore = teamScore.TotalScore;
                    teamScoreDbModel.Wideballs = teamScore.Wideballs;
                    teamScoreDbModel.NoBalls = teamScore.NoBalls;
                    teamScoreDbModel.Byes = teamScore.Byes;
                    teamScoreDbModel.LegByes = teamScore.LegByes;

                    _context.TeamScores.Update(teamScoreDbModel);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest(ModelState);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> UpdateFallOfWicket([FromForm] TeamMatchScoredto model)
        {
            if (ModelState.IsValid)
            {
                foreach (var fallOfWicket in model.FallOfWicket)
                {
                    var fallOfWicketDbModel = new FallOfWicket
                    {
                        FallOfWicketId = fallOfWicket.FallOfWicketId
                    };
                    _context.FallOFWickets.Attach(fallOfWicketDbModel);

                    fallOfWicketDbModel.FallOfWicketId = fallOfWicket.FallOfWicketId;
                    fallOfWicketDbModel.First = fallOfWicket.First;
                    fallOfWicketDbModel.Second = fallOfWicket.Second;
                    fallOfWicketDbModel.Third = fallOfWicket.Third;
                    fallOfWicketDbModel.Fourth = fallOfWicket.Fourth;
                    fallOfWicketDbModel.Fifth = fallOfWicket.Fifth;
                    fallOfWicketDbModel.Sixth = fallOfWicket.Sixth;
                    fallOfWicketDbModel.Seventh = fallOfWicket.Seventh;
                    fallOfWicketDbModel.Eight = fallOfWicket.Eight;
                    fallOfWicketDbModel.Ninth = fallOfWicket.Ninth;
                    fallOfWicketDbModel.Tenth = fallOfWicket.Tenth;
                    fallOfWicketDbModel.TeamId = fallOfWicket.TeamId;
                    fallOfWicketDbModel.MatchId = fallOfWicket.MatchId;

                    _context.FallOFWickets.Update(fallOfWicketDbModel);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest(ModelState);

        }

        [HttpPost]
        [Route("PlayerScores/PlayerScoreModal")]
        [Authorize(Roles = "Club Admin,Administrator")]
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
        [Authorize(Roles = "Club Admin,Administrator")]
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
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Edit(int? matchId, int? homeTeamId, int? oppTeamId, int? playerScoreId)
        {
            ViewBag.Name = "Edit Match Players";
            ViewBag.HowOut = new SelectList(_context.HowOut
                .AsNoTracking()
                .Select(i => new { i.HowOutId, i.Name })
                , "HowOutId", "Name");
            var scoreDto = new ScoreCarddto();
            //ViewBag.matchId = matchId;
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

            ViewBag.homeTeamId = homeTeamId;
            ViewBag.opponentTeamId = oppTeamId;

            ViewBag.HomePlayerId = new SelectList(_context.Players
                 .AsNoTracking()
                 .Where(i => i.TeamId == homeTeamId)
                 .Select(i => new { i.PlayerId, i.Player_Name })
                 , "PlayerId", "Player_Name");

            ViewBag.OpponentPlayerId = new SelectList(_context.Players
             .AsNoTracking()
             .Where(i => i.TeamId == oppTeamId), "PlayerId", "Player_Name");

            scoreDto.HomeTeamScoreCard = _context.PlayerScores
                .AsNoTracking()
                .Where(m => m.MatchId == matchId && m.TeamId == homeTeamId)
                .Select(i => new MatchSummarydto
                {
                    PlayerScoreId = i.PlayerScoreId,
                    IsPlayedInning = i.IsPlayedInning,
                    PlayerId = i.PlayerId,
                    HowOutId = i.HowOutId,
                    Bowler = i.Bowler,
                    MatchId = i.MatchId,
                    TeamId = i.TeamId
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
             .Where(m => m.MatchId == matchId && m.TeamId == oppTeamId)
             .Select(i => new MatchSummarydto
             {
                 PlayerScoreId = i.PlayerScoreId,
                 IsPlayedInning = i.IsPlayedInning,
                 PlayerId = i.PlayerId,
                 HowOutId = i.HowOutId,
                 Bowler = i.Bowler,
                 MatchId = i.MatchId,
                 TeamId = i.TeamId
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

            scoreDto.FallOfWicket = _context.FallOFWickets
          .AsNoTracking()
          .Where(m => m.MatchId == matchId)
          .Select(i => new FallOfWicketdto
          {
              FallOfWicketId = i.FallOfWicketId,
              TeamId = i.TeamId,
              MatchId = i.MatchId,
              First = i.First,
              Second = i.Second,
              Third = i.Third,
              Fourth = i.Fourth,
              Fifth = i.Fifth,
              Sixth = i.Sixth,
              Seventh = i.Seventh,
              Eight = i.Eight,
              Ninth = i.Ninth,
              Tenth = i.Tenth

          })
          .ToList();

            if (matchId.HasValue)
                for (var index = 0; index < 2; index++)
                {
                    if (scoreDto.FallOfWicket.Count == index)
                    {
                        scoreDto.FallOfWicket.Add(new FallOfWicketdto
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
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit([FromForm] TeamMatchScoredto Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {
                foreach (var mp in Matchplayers.MatchScore)
                {
                    var matchScore = new PlayerScore { PlayerScoreId = mp.PlayerScoreId };

                    _context.PlayerScores.Attach(matchScore);
                    matchScore.PlayerScoreId = mp.PlayerScoreId;
                    matchScore.Position = mp.Position;
                    matchScore.IsPlayedInning = mp.IsPlayedInning;
                    matchScore.PlayerId = mp.PlayerId;
                    matchScore.HowOutId = mp.HowOutId;
                    matchScore.Bowler = mp.Bowler;
                    matchScore.MatchId = mp.MatchId;
                    matchScore.TeamId = mp.TeamId;
                    await _context.SaveChangesAsync();

                }
                return Ok();
                // return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return BadRequest(ModelState);
        }


        // POST: PlayerScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerScore = await _context.PlayerScores.SingleOrDefaultAsync(m => m.PlayerId == null);
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
