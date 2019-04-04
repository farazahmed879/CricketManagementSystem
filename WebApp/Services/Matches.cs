using CricketApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.IServices;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class Matches : IMatches
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public Matches(CricketContext context,
            UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Matchdto>> GetAllMatches(int? teamId, int? matchTypeId,
                                               int? tournamentId, int? matchSeriesId,
                                                int? season, int? matchOvers, int? userId, int? page)
        {
            int pageSize = 20;
            var model = await PaginatedList<Matchdto>.CreateAsync(
                _context.Matches
                .AsNoTracking()
               .Where(i => (!matchTypeId.HasValue || i.MatchTypeId == matchTypeId) &&
                           (!teamId.HasValue || i.HomeTeamId == teamId || i.OppponentTeamId == teamId) &&
                           (!tournamentId.HasValue || i.TournamentId == tournamentId) && (!season.HasValue || i.Season == season) &&
                           (!matchSeriesId.HasValue || i.MatchSeriesId == matchSeriesId) && (!matchOvers.HasValue || i.MatchOvers == matchOvers))
                .Select(i => new ViewModels.Matchdto
                {
                    MatchId = i.MatchId,
                    GroundName = i.GroundName,
                    DateOfMatch = i.DateOfMatch.HasValue ? i.DateOfMatch.Value.ToShortDateString() : "",
                    MatchOvers = i.MatchOvers,
                    Result = i.Result,
                    MatchType = i.MatchType.MatchTypeName,
                    //TournamentId = i.TournamentId,
                    MatchTypeId = i.MatchTypeId,
                    //Tournament = i.Tournament.TournamentName,
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppponentTeam = i.OppponentTeam.Team_Name,
                    HomeTeamId = i.HomeTeamId,
                    OppponentTeamId = i.OppponentTeamId,
                    //MatchLogo = i.MatchLogo,
                    HasFilledHomeTeamData = i.PlayerScores.Any() && i.PlayerScores.Any(o => o.Player != null && o.Player.TeamId == i.HomeTeamId),
                    HasFilledOpponentTeamData = i.PlayerScores.Any() && i.PlayerScores.Any(o => o.Player != null && o.Player.TeamId == i.OppponentTeamId),
                    HasFilledTeamScoreData = i.TeamScores.Any() && i.TeamScores.Any(o => i.MatchId == i.MatchId)
                })
               .OrderByDescending(i => i.MatchId)
                                 , page ?? 1, pageSize);
            return model;
        }

    }
}
