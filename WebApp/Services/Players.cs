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

        public async Task<List<Playersdto>> GetAllPlayers(int? teamId, int? playerRoleId, int? battingStyleId, int? bowlingStyleId, string name, int? userId, int? page)
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
                                      && (!userId.HasValue || i.Team.clubAdmin.UserId == userId)
                                     )
                      .Select(i => new Playersdto
                      {
                          PlayerId = i.PlayerId,
                          Player_Name = i.Player_Name,
                          BattingStyle = i.BattingStyle.Name,
                          BowlingStyle = i.BowlingStyle.Name,
                          PlayerRole = i.PlayerRole.Name,
                          PlayerLogo = i.PlayerLogo,
                          DOB = i.DOB.HasValue ? i.DOB.Value.ToShortDateString() : "",
                          Team = i.Team.Team_Name,

                      })
                        .OrderByDescending(i => i.PlayerId)
                          , page ?? 1, pageSize);
            //if (partialView)
            //    return PartialView("_PlayerPartial", model);
            //else
            return model;
        }
    }
}
