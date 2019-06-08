using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.IServices;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class MatchSummary : IMatchSummary
    {
        private readonly CricketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MatchSummary(CricketContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<MatchDetails> FirstInning(int matchId, int hometeamId, int oppTeamId)
        {
            var model = await _context.Matches
                .AsNoTracking()
                .Where(i => i.MatchId == matchId)
                .Select(i => new MatchDetails
                {
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppTeam = i.OppponentTeam.Team_Name,
                    Type = i.MatchType.MatchTypeName,
                    Ground = i.GroundName,
                    Stage = i.TournamentStage.Name,
                    Tournament = i.Tournament.TournamentName,
                    HomeTeamLogo = i.HomeTeam.FileName ?? "noLogo.png",
                    OppTeamLogo = i.OppponentTeam.FileName ?? "noLogo.png",
                    matchBatsman = i.PlayerScores
                    .Where(o => o.MatchId == matchId && o.TeamId == hometeamId && o.PlayerId != null)
                    .Select(o => new MatchBatsman
                    {
                        PlayerName = o.Player.Player_Name,
                        Runs = o.Bat_Runs,
                        Balls = o.Bat_Balls,
                        Six = o.Six,
                        Four = o.Four,
                        Bowler = o.Bowler.Player_Name,
                        HowOut = o.HowOut.Normalize,

                    }).ToList(),
                    matchBowler = i.PlayerScores
                    .Where(o => o.MatchId == matchId && o.TeamId == oppTeamId && o.Overs != 0 && o.Overs != null)
                    .Select(o => new MatchBowler
                    {
                        PlayerName = o.Player.Player_Name,
                        Runs = o.Ball_Runs,
                        Wickets = o.Wickets,
                        Overs = o.Overs,
                        Maiden = o.Maiden,

                    }).ToList()

                }).SingleOrDefaultAsync();
            return model;
        }

        public async Task<MatchDetails> SecondInning(int matchId, int hometeamId, int oppTeamId)
        {
            var model = await _context.Matches
                .AsNoTracking()
                .Where(i => i.MatchId == matchId)
                .Select(i => new MatchDetails
                {
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppTeam = i.OppponentTeam.Team_Name,
                    Type = i.MatchType.MatchTypeName,
                    Ground = i.GroundName,
                    Stage =i.TournamentStage.Name,
                    Tournament = i.Tournament.TournamentName,
                    HomeTeamLogo = i.HomeTeam.FileName ?? "noLogo.png",
                    OppTeamLogo = i.OppponentTeam.FileName ?? "noLogo.png",
                    matchBatsman = i.PlayerScores
                    .Where(o => o.MatchId == matchId && o.TeamId == oppTeamId && o.PlayerId != null)
                    .Select(o => new MatchBatsman
                    {
                        PlayerName = o.Player.Player_Name,
                        Runs = o.Bat_Runs,
                        Balls = o.Bat_Balls,
                        Six = o.Six,
                        Four = o.Four,
                        Bowler = o.Bowler.Player_Name,
                        HowOut = o.HowOut.Normalize,

                    }).ToList(),
                    matchBowler = i.PlayerScores
                    .Where(o => o.MatchId == matchId && o.TeamId == hometeamId && o.Overs != 0 && o.Overs != null)
                    .Select(o => new MatchBowler
                    {
                        PlayerName = o.Player.Player_Name,
                        Runs = o.Ball_Runs,
                        Wickets = o.Wickets,
                        Overs = o.Overs,
                        Maiden = o.Maiden,

                    }).ToList()

                }).SingleOrDefaultAsync();
            return model;
        }

        public async Task<MatchDetails> GetBatsman(int matchId, int teamId)
        {
            var model = await _context.Matches
                .AsNoTracking()
                .Where(i => i.MatchId == matchId && i.HomeTeamId == teamId)
                .Select(i => new MatchDetails
                {
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppTeam = i.OppponentTeam.Team_Name,
                    Type = i.MatchType.MatchTypeName,
                    Ground = i.GroundName,
                    Tournament = i.Tournament.TournamentName,
                    matchBatsman = i.PlayerScores
                    .Where(o => o.MatchId == matchId && o.TeamId == teamId)
                    .Select(o => new MatchBatsman
                    {
                        PlayerName = o.Player.Player_Name,
                        Runs = o.Bat_Balls,
                        Balls = o.Bat_Balls,
                        Six = o.Six,
                        Four = o.Four,
                        Bowler = o.Bowler.Player_Name,
                        HowOut = o.HowOut.Name,

                    }).ToList()

                }).SingleOrDefaultAsync();
            return model;
        }

        public async Task<MatchDetails> GetBowler(int matchId, int teamId)
        {
            var model = await _context.Matches
                .AsNoTracking()
                .Where(i => i.MatchId == matchId && i.HomeTeamId == teamId)
                .Select(i => new MatchDetails
                {
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppTeam = i.OppponentTeam.Team_Name,
                    Type = i.MatchType.MatchTypeName,
                    Ground = i.GroundName,
                    Tournament = i.Tournament.TournamentName,
                    matchBowler = i.PlayerScores
                    .Where(o => o.MatchId == matchId && o.TeamId == teamId && o.Overs != 0 && o.Overs != null)
                    .Select(o => new MatchBowler
                    {
                        PlayerName = o.Player.Player_Name,
                        Runs = o.Ball_Runs,
                        Wickets = o.Wickets,
                        Overs = o.Overs,
                        Maiden = o.Maiden,

                    }).ToList()

                }).SingleOrDefaultAsync();
            return model;
        }

    }
}
