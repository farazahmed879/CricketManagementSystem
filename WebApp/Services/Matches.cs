using CricketApp.Data;
using CricketApp.Domain;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public Matches(CricketContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PaginatedList<MatchListdto>> GetAllMatchesList(DataTableAjaxPostModel model, int? teamId, int? matchTypeId,
                                               int? tournamentId, int? matchSeriesId,
                                                int? season, int? matchOvers)
        {
            var result = await PaginatedList<MatchListdto>.CreateAsync(
                _context.Matches
                .AsNoTracking()
               .Where(i => (!matchTypeId.HasValue || i.MatchTypeId == matchTypeId) &&
                           (!teamId.HasValue || i.HomeTeamId == teamId || i.OppponentTeamId == teamId) &&
                           (!tournamentId.HasValue || i.TournamentId == tournamentId) && (!season.HasValue || i.Season == season) &&
                           (!matchSeriesId.HasValue || i.MatchSeriesId == matchSeriesId) && (!matchOvers.HasValue || i.MatchOvers == matchOvers))
                .Select(i => new ViewModels.MatchListdto
                {
                    MatchId = i.MatchId,
                    GroundName = i.Ground.Name,
                    DateOfMatch = i.DateOfMatch.HasValue ? i.DateOfMatch.Value.ToString("dddd, dd MMMM yyyy") : "",
                    MatchOvers = i.MatchOvers,
                    Result = i.Result,
                    MatchType = i.MatchType.MatchTypeName,
                    MatchTypeId = i.MatchTypeId,
                    HomeTeam = i.HomeTeam.Team_Name,
                    OppponentTeam = i.OppponentTeam.Team_Name,
                    HomeTeamId = i.HomeTeamId,
                    OppponentTeamId = i.OppponentTeamId,
                    //MatchLogo = i.MatchLogo,
                    TeamDataCout = i.PlayerScores.Count(o => o.Player.TeamId == i.HomeTeamId),
                    HasFilledHomeTeamData = i.PlayerScores.Any() && i.PlayerScores.Any(o => o.Player != null && o.Player.TeamId == i.HomeTeamId),
                    HasFilledOpponentTeamData = i.PlayerScores.Any() && i.PlayerScores.Any(o => o.Player != null && o.Player.TeamId == i.OppponentTeamId),
                    HasFilledTeamScoreData = i.TeamScores.Any() && i.TeamScores.Any(o => i.MatchId == i.MatchId)
                })
               .OrderByDescending(i => i.DateOfMatch)
                                 , model.Start, model.Length);
            return result;
        }

    }
}
