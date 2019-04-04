using CricketApp.Data;
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
    public class Players : IPlayers
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public Players(CricketContext context,
            UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Playersdto>> GetAllPlayersList(int? teamId, int? playerRoleId, int? battingStyleId, int? bowlingStyleId, string name, int? userId, int? page)
        {
            int pageSize = 20;
            var model = await PaginatedList<Playersdto>.CreateAsync(
                          _context.Players
                        .AsNoTracking()
                        .Where(i => (!teamId.HasValue || i.TeamId == teamId)
                                      && (!playerRoleId.HasValue || i.PlayerRoleId == playerRoleId)
                                      && (!battingStyleId.HasValue || i.BattingStyleId == battingStyleId)
                                      && (!bowlingStyleId.HasValue || i.BowlingStyleId == bowlingStyleId)
                                      && (string.IsNullOrEmpty(name) || EF.Functions.Like(i.Player_Name, '%' + name + '%'))
                                     )
                      .Select(i => new Playersdto
                      {
                          PlayerId = i.PlayerId,
                          Player_Name = i.Player_Name,
                          BattingStyle = i.BattingStyle.Name,
                          BowlingStyle = i.BowlingStyle.Name,
                          PlayerRole = i.PlayerRole.Name,
                          DOB = i.DOB.HasValue ? i.DOB.Value.ToShortDateString() : "",
                          Team = i.Team.Team_Name,

                      })
                        .OrderByDescending(i => i.PlayerId)
                          , page ?? 1, pageSize);
            return model;
        }
        
        public List<PlayersDropDowndto> GetAllPlayers()
        {
            var model = _context.Players
                .AsNoTracking()
                .Select(i => new PlayersDropDowndto
                {
                    PlayerId = i.PlayerId,
                    Player_Name = i.Player_Name
                }).ToList();
            return model;
        }

        public async Task<PlayerPastRecorddto> GetPlayerPastRecordByPlayerId(int? playerId)
        {
            var playerPastRecord =  await _context.PlayerPastRecord
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

            return playerPastRecord;
        }



    }
}
