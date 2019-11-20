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
using WebApp.Helper;
using Dapper;
using System.Data;
using System.Collections.Generic;

namespace WebApp.Controllers
{

    public class PlayerScoresController : Controller
    {
        private readonly CricketContext _context;



        public PlayerScoresController(CricketContext context)
        {
            _context = context;
        }

        [HttpGet]
        // [Route("PlayerScores/Index/matchId{matchId}/homeTeamId{homeTeamId}/oppTeamId{oppTeamId}/playerScoreId{playerScoreId}")]
        public IActionResult Index(int? matchId, int? homeTeamId, int? oppTeamId, bool api)
        {
            ViewBag.Name = "Score Card";
            var scoreDto = new ScoreCarddto();
            ViewBag.matchId = matchId;
            ViewBag.homeTeamId = homeTeamId;
            ViewBag.oppTeamId = oppTeamId;

            ViewBag.Overs = _context.Matches
              .AsNoTracking()
              .Where(i => i.MatchId == matchId)
              .Select(i => i.MatchOvers)
              .Single();

            var connection = _context.Database.GetDbConnection();
            var s = connection.Query<Summary2dto>(
              "usp_Summary2",
              new
              {
                  paramMatchId = matchId,
                  paramHomeTeamId = homeTeamId,
                  paramOpponentTeamId = oppTeamId
              },
              commandType: CommandType.StoredProcedure) ?? new List<Summary2dto>()
              {
              };


            var matchSummary = _context.PlayerScores
                .AsNoTracking()
                .Where(i => (i.TeamId == homeTeamId || i.TeamId == oppTeamId) && i.MatchId == matchId)
                             .Select(i => new MatchSummarydto
                             {
                                 PlayerScoreId = i.PlayerScoreId,
                                 PlayerId = i.PlayerId,
                                 Position = i.Position,
                                 MatchId = i.MatchId,
                                 Bowler = i.BowlerId,
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
                                 PlayerName = i.Player.Player_Name,
                                 HowOutName = i.HowOut.Name,
                                 TeamId = i.TeamId,
                                 Fielder = i.Fielder
                             })
                .OrderBy(i => i.Position)
                .ToList();

            scoreDto.HomeTeamScoreCard = matchSummary.Where(i => i.TeamId == homeTeamId).ToList();

            scoreDto.OpponentTeamScoreCard = matchSummary.Where(i => i.TeamId == oppTeamId).ToList();
            scoreDto.Summary2dto = s.SingleOrDefault();
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
                    TeamName = i.Team.Team_Name,
                    Wickets = i.Wickets


                })
                .OrderBy(i => i.TeamScoreId)
                .ToList();

            var graph = _context.FallOFWickets
               .AsNoTracking()
               .Where(i => i.MatchId == matchId && (i.TeamId == homeTeamId || i.TeamId == oppTeamId))
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


            scoreDto.HomeTeamFOW = graph.Where(i => i.TeamId == homeTeamId).ToList();
            scoreDto.OpponentTeamFOW = graph.Where(i => i.TeamId == oppTeamId).ToList();

            if (api == true)
                return Json(scoreDto);
            else
                return View(scoreDto);

        }


        [HttpGet("PlayerScores/Create/{homeTeamId}/{oppTeamId}/{matchId}")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create(int homeTeamId, int oppTeamId, int matchId)
        {
            var model = new TeamMatchScoredto();
            ViewBag.Name = "Add Match Players";
            ViewBag.HowOut = new SelectList(_context.HowOut
                .AsNoTracking()
                .Select(i => new { i.HowOutId, i.Name })
                , "HowOutId", "Name");

            var connection = _context.Database.GetDbConnection();
            var s = connection.Query<Summary2dto>(
              "usp_Summary2",
              new
              {
                  paramMatchId = matchId,
                  paramHomeTeamId = homeTeamId,
                  paramOpponentTeamId = oppTeamId
              },
              commandType: CommandType.StoredProcedure) ?? new List<Summary2dto>()
              {
              };



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


            //for (int i = 0; i < 15; i++)
            //{
            //    model.HomeTeamScoreCard.Add(new MatchSummarydto
            //    {
            //        MatchId = matchId,

            //    });
            //}
            //for (int i = 0; i < 15; i++)
            //{
            //    model.OpponentTeamScoreCard.Add(new MatchSummarydto
            //    {
            //        MatchId = matchId,

            //    });
            //}

            model.Summary2dto = s.SingleOrDefault();
            model.HomeTeamScoreCard.MatchId = matchId;
            return View(model);
        }



        [HttpGet("PlayerScores/Edit/{homeTeamId}/{oppTeamId}/{matchId}")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Edit(int? matchId, int? homeTeamId, int? oppTeamId)
        {
            ViewBag.Name = "Edit Match Players";
            ViewBag.HowOut = new SelectList(_context.HowOut
                .AsNoTracking()
                .Select(i => new { i.HowOutId, i.Name })
                , "HowOutId", "Name");
            var scoreDto = new ScoreCarddto();
            var connection = _context.Database.GetDbConnection();
            var s = connection.Query<Summary2dto>(
              "usp_Summary2",
              new
              {
                  paramMatchId = matchId,
                  paramHomeTeamId = homeTeamId,
                  paramOpponentTeamId = oppTeamId
              },
              commandType: CommandType.StoredProcedure) ?? new List<Summary2dto>()
              {
              };
            scoreDto.Summary2dto = s.SingleOrDefault();
            //ViewBag.matchId = matchId;

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

            var matchPlayers = _context.PlayerScores
              .AsNoTracking()
              .Where(m => m.MatchId == matchId && (m.TeamId == homeTeamId || m.TeamId == oppTeamId))
              .Select(i => new MatchSummarydto
              {
                  PlayerScoreId = i.PlayerScoreId,
                  IsPlayedInning = i.IsPlayedInning,
                  PlayerId = i.PlayerId,
                  HowOutId = i.HowOutId,
                  Bowler = i.BowlerId,
                  MatchId = i.MatchId,
                  TeamId = i.TeamId,
                  Wickets = i.Wickets,
                  Fielder = i.Fielder
              })
              .ToList();

            scoreDto.HomeTeamScoreCard = matchPlayers.Where(i => i.TeamId == homeTeamId).ToList();
            scoreDto.OpponentTeamScoreCard = matchPlayers.Where(i => i.TeamId == oppTeamId).ToList();



            if (matchId.HasValue)
                for (var index = 0; index < 15; index++)
                {
                    if (scoreDto.HomeTeamScoreCard.Count == index)
                    {
                        scoreDto.HomeTeamScoreCard.Add(new MatchSummarydto
                        {
                            MatchId = matchId.Value,
                            TeamId = homeTeamId.Value
                        });
                    }
                }


            if (matchId.HasValue)
                for (var index = 0; index < 15; index++)
                {
                    if (scoreDto.OpponentTeamScoreCard.Count == index)
                    {
                        scoreDto.OpponentTeamScoreCard.Add(new MatchSummarydto
                        {
                            MatchId = matchId.Value,
                            TeamId = oppTeamId.Value
                        });
                    }
                }



            scoreDto.TeamScoreCard = _context.TeamScores
           .AsNoTracking()
           .Where(m => m.MatchId == matchId)
           .Select(i => new TeamScoredto
           {
               TeamScoreId = i.TeamScoreId,
               Wickets = i.Wickets,
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



        [HttpPost("PlayerScores/HomeTeamSave")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> HomeTeamSave([FromForm] TeamMatchScoredto HomeTeamplayers, int teamId)
        {
            if (ModelState.IsValid)
            {

                var model = new PlayerScore();
                model.Position = HomeTeamplayers.HomeTeamScoreCard.Position;
                model.IsPlayedInning = HomeTeamplayers.HomeTeamScoreCard.IsPlayedInning;
                model.PlayerId = HomeTeamplayers.HomeTeamScoreCard.PlayerId;
                model.HowOutId = HomeTeamplayers.HomeTeamScoreCard.HowOutId;
                model.BowlerId = HomeTeamplayers.HomeTeamScoreCard.Bowler == -1 ? null : HomeTeamplayers.HomeTeamScoreCard.Bowler;
                model.MatchId = HomeTeamplayers.HomeTeamScoreCard.MatchId;
                model.TeamId = HomeTeamplayers.HomeTeamScoreCard.TeamId;
                model.Fielder = HomeTeamplayers.HomeTeamScoreCard.Fielder;
                model.Bat_Runs = HomeTeamplayers.HomeTeamScoreCard.Bat_Runs;
                model.Bat_Balls = HomeTeamplayers.HomeTeamScoreCard.Bat_Balls;
                model.Four = HomeTeamplayers.HomeTeamScoreCard.Four;
                model.Six = HomeTeamplayers.HomeTeamScoreCard.Six;
                model.Ball_Runs = HomeTeamplayers.HomeTeamScoreCard.Ball_Runs;
                model.Overs = HomeTeamplayers.HomeTeamScoreCard.Overs;
                model.Wickets = HomeTeamplayers.HomeTeamScoreCard.Wickets;
                model.Maiden = HomeTeamplayers.HomeTeamScoreCard.Maiden;
                model.RunOut = HomeTeamplayers.HomeTeamScoreCard.RunOut;
                model.Catches = HomeTeamplayers.HomeTeamScoreCard.Catches;
                model.Stump = HomeTeamplayers.HomeTeamScoreCard.Stump;
                _context.PlayerScores.Add(model);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }


        [HttpPost("PlayerScores/OpponentTeamSave")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> OpponentTeamSave([FromForm] TeamMatchScoredto OpponentTeamplayers, int teamId)
        {
            if (ModelState.IsValid)
            {

                var model = new PlayerScore();
                model.Position = OpponentTeamplayers.OppoTeamScoreCard.Position;
                model.IsPlayedInning = OpponentTeamplayers.OppoTeamScoreCard.IsPlayedInning;
                model.PlayerId = OpponentTeamplayers.OppoTeamScoreCard.PlayerId;
                model.HowOutId = OpponentTeamplayers.OppoTeamScoreCard.HowOutId;
                model.BowlerId = OpponentTeamplayers.OppoTeamScoreCard.Bowler == -1 ? null : OpponentTeamplayers.OppoTeamScoreCard.Bowler;
                model.MatchId = OpponentTeamplayers.OppoTeamScoreCard.MatchId;
                model.TeamId = OpponentTeamplayers.OppoTeamScoreCard.TeamId;
                model.Fielder = OpponentTeamplayers.OppoTeamScoreCard.Fielder;
                model.Bat_Runs = OpponentTeamplayers.OppoTeamScoreCard.Bat_Runs;
                model.Bat_Balls = OpponentTeamplayers.OppoTeamScoreCard.Bat_Balls;
                model.Four = OpponentTeamplayers.OppoTeamScoreCard.Four;
                model.Six = OpponentTeamplayers.OppoTeamScoreCard.Six;
                model.Ball_Runs = OpponentTeamplayers.OppoTeamScoreCard.Ball_Runs;
                model.Overs = OpponentTeamplayers.OppoTeamScoreCard.Overs;
                model.Wickets = OpponentTeamplayers.OppoTeamScoreCard.Wickets;
                model.Maiden = OpponentTeamplayers.OppoTeamScoreCard.Maiden;
                model.RunOut = OpponentTeamplayers.OppoTeamScoreCard.RunOut;
                model.Catches = OpponentTeamplayers.OppoTeamScoreCard.Catches;
                model.Stump = OpponentTeamplayers.OppoTeamScoreCard.Stump;
                _context.PlayerScores.Add(model);
                await _context.SaveChangesAsync();
            }
            return Json(ResponseHelper.UnSuccess());
        }

        [HttpPost("PlayerScores/TeamScoreSave")]
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
                    Wickets = i.Wickets,
                    Wideballs = i.Wideballs,
                    NoBalls = i.NoBalls,
                    Byes = i.Byes,
                    LegByes = i.LegByes
                }
                ));

                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
                // return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return Json(ResponseHelper.UnSuccess());
        }

        [HttpPost("PlayerScores/FallOfWicketSave")]
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
                return Json(ResponseHelper.Success());
                // return RedirectToAction(nameof(Index), new { matchId = Matchplayers.Select(i => i.MatchId).First(), teamId });
            }
            return Json(ResponseHelper.UnSuccess());
        }



        // POST: PlayerScores/OpponentTeamUpdate/5
        [HttpPut("PlayerScores/OpponentTeamUpdate")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> OpponentTeamUpdate([FromForm] TeamMatchScoredto Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.PlayerScores.Where(i => i.PlayerScoreId == Matchplayers.OppoTeamScoreCard.PlayerScoreId).SingleOrDefaultAsync();


                model.Position = Matchplayers.OppoTeamScoreCard.Position;
                model.IsPlayedInning = Matchplayers.OppoTeamScoreCard.IsPlayedInning;
                model.PlayerId = Matchplayers.OppoTeamScoreCard.PlayerId;
                model.HowOutId = Matchplayers.OppoTeamScoreCard.HowOutId;
                model.BowlerId = Matchplayers.OppoTeamScoreCard.Bowler;
                model.MatchId = Matchplayers.OppoTeamScoreCard.MatchId;
                model.TeamId = Matchplayers.OppoTeamScoreCard.TeamId;
                model.Fielder = Matchplayers.OppoTeamScoreCard.Fielder;
                model.Bat_Runs = Matchplayers.OppoTeamScoreCard.Bat_Runs;
                model.Bat_Balls = Matchplayers.OppoTeamScoreCard.Bat_Balls;
                model.Four = Matchplayers.OppoTeamScoreCard.Four;
                model.Six = Matchplayers.OppoTeamScoreCard.Six;
                model.Ball_Runs = Matchplayers.OppoTeamScoreCard.Ball_Runs;
                model.Overs = Matchplayers.OppoTeamScoreCard.Overs;
                model.Wickets = Matchplayers.OppoTeamScoreCard.Wickets;
                model.Maiden = Matchplayers.OppoTeamScoreCard.Maiden;
                model.RunOut = Matchplayers.OppoTeamScoreCard.RunOut;
                model.Catches = Matchplayers.OppoTeamScoreCard.Catches;
                model.Stump = Matchplayers.OppoTeamScoreCard.Stump;
                _context.PlayerScores.Update(model);
                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }

        [HttpPut("PlayerScores/HomeTeamUpdate")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> HomeTeamUpdate([FromForm] TeamMatchScoredto Matchplayers, int teamId)
        {
            if (ModelState.IsValid)
            {
                var model = await _context.PlayerScores.Where(i => i.PlayerScoreId == Matchplayers.HomeTeamScoreCard.PlayerScoreId).SingleOrDefaultAsync();


                model.Position = Matchplayers.HomeTeamScoreCard.Position;
                model.IsPlayedInning = Matchplayers.HomeTeamScoreCard.IsPlayedInning;
                model.PlayerId = Matchplayers.HomeTeamScoreCard.PlayerId;
                model.HowOutId = Matchplayers.HomeTeamScoreCard.HowOutId;
                model.BowlerId = Matchplayers.HomeTeamScoreCard.Bowler;
                model.MatchId = Matchplayers.HomeTeamScoreCard.MatchId;
                model.TeamId = Matchplayers.HomeTeamScoreCard.TeamId;
                model.Fielder = Matchplayers.HomeTeamScoreCard.Fielder;
                model.Bat_Runs = Matchplayers.HomeTeamScoreCard.Bat_Runs;
                model.Bat_Balls = Matchplayers.HomeTeamScoreCard.Bat_Balls;
                model.Four = Matchplayers.HomeTeamScoreCard.Four;
                model.Six = Matchplayers.HomeTeamScoreCard.Six;
                model.Ball_Runs = Matchplayers.HomeTeamScoreCard.Ball_Runs;
                model.Overs = Matchplayers.HomeTeamScoreCard.Overs;
                model.Wickets = Matchplayers.HomeTeamScoreCard.Wickets;
                model.Maiden = Matchplayers.HomeTeamScoreCard.Maiden;
                model.RunOut = Matchplayers.HomeTeamScoreCard.RunOut;
                model.Catches = Matchplayers.HomeTeamScoreCard.Catches;
                model.Stump = Matchplayers.HomeTeamScoreCard.Stump;
                _context.PlayerScores.Update(model);
                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }

        [HttpPut("PlayerScores/UpdateTeamScore")]
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
                    teamScoreDbModel.Wickets = teamScore.Wickets;
                    teamScoreDbModel.Wideballs = teamScore.Wideballs;
                    teamScoreDbModel.NoBalls = teamScore.NoBalls;
                    teamScoreDbModel.Byes = teamScore.Byes;
                    teamScoreDbModel.LegByes = teamScore.LegByes;
                    await _context.SaveChangesAsync();
                    //_context.TeamScores.Update(teamScoreDbModel);
                }


                return Json(ResponseHelper.UpdateSuccess());
            }
            else
                return Json(ResponseHelper.UpdateUnSuccess());

        }

        [HttpPut("PlayerScores/UpdateFallOfWicket")]
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
                    await _context.SaveChangesAsync();
                }


                return Json(ResponseHelper.UpdateSuccess());
            }
            else
                return Json(ResponseHelper.UpdateUnSuccess());

        }

        [HttpPut]
        [Route("PlayerScores/PlayerScoreModal")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public int PlayerScoreModal([FromBody]MatchSummarydto playerScores)
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
                    playercoreCard.Catches = playerScores.Catches;
                    playercoreCard.Stump = playerScores.Stump;
                    _context.Update(playercoreCard);
                    _context.SaveChangesAsync();
                    return playerScores.TeamId;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }

        [HttpGet]
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
          PlayerName = i.Player.Player_Name,
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
          PlayerScoreId = i.PlayerScoreId,
          TeamId= i.TeamId
      })
      .ToList();

            return Json(playerScore);
        }

        [HttpGet]
        public IActionResult MatchScore(int homeTeamId, int oppTeamId, int matchId)
        {
            var model = new TeamMatchScoredto();
            var connection = _context.Database.GetDbConnection();
            ViewBag.HowOut = new SelectList(_context.HowOut
          .AsNoTracking()
          .Select(i => new { i.HowOutId, i.Name })
          , "HowOutId", "Name");

            var s = connection.Query<Summary2dto>(
              "usp_Summary2",
              new
              {
                  paramMatchId = matchId,
                  paramHomeTeamId = homeTeamId,
                  paramOpponentTeamId = oppTeamId
              },
              commandType: CommandType.StoredProcedure) ?? new List<Summary2dto>()
              {
              };
           
            model.Summary2dto = s.SingleOrDefault();
            return View(model);
        }
        [HttpGet("PlayerScore/GetHomeTeamScore/MatchId/{matchId}/HomeTeamId/{homeTeamId}")]
        public List<MatchSummarydto> GetHomeTeamScore(int? matchId, int? homeTeamId,bool api)
        {
            var model = _context.PlayerScores
                .Where(i => i.MatchId == matchId && i.TeamId == homeTeamId && i.PlayerId != null)
                .Select(i => new MatchSummarydto
                {
                    PlayerScoreId = i.PlayerScoreId,
                    PlayerId = i.PlayerId,
                    PlayerName = i.Player.Player_Name,
                    HowOutId = i.HowOutId,
                    HowOutName = i.HowOut.Name,
                    Position = i.Position,
                    IsPlayedInning = i.IsPlayedInning,
                    Bat_Runs = i.Bat_Runs,
                    Bat_Balls = i.Bat_Balls,
                    Four = i.Four,
                    Six = i.Six,
                    Overs = i.Overs,
                    Ball_Runs = i.Ball_Runs,
                    Wickets = i.Wickets,
                    Maiden = i.Maiden,
                    Catches = i.Catches,
                    RunOut = i.RunOut,
                    Stump = i.Stump


                }).OrderBy(i => i.Position).ToList();

            return model;
        }

        [HttpGet("PlayerScore/GetOpponentTeamScore/MatchId/{matchId}/OppTeamId/{oppTeamId}")]
        public List<MatchSummarydto> GetOpponentTeamScore(int? matchId, int? oppTeamId, bool api)
        {
            var model = _context.PlayerScores
                .Where(i => i.MatchId == matchId && i.TeamId == oppTeamId)
                .Select(i => new MatchSummarydto
                {
                    PlayerScoreId = i.PlayerScoreId,
                    PlayerId = i.PlayerId,
                    PlayerName = i.Player.Player_Name,
                    HowOutId = i.HowOutId,
                    HowOutName = i.HowOut.Name,
                    Position = i.Position,
                    IsPlayedInning = i.IsPlayedInning,
                    Bat_Runs = i.Bat_Runs,
                    Bat_Balls = i.Bat_Balls,
                    Four = i.Four,
                    Six = i.Six,
                    Overs = i.Overs,
                    Ball_Runs = i.Ball_Runs,
                    Wickets = i.Wickets,
                    Maiden = i.Maiden,
                    Catches = i.Catches,
                    RunOut = i.RunOut,
                    Stump = i.Stump


                }).OrderBy(i => i.Position).ToList();

            return model;
        }

    }
}
