using CricketApp.Data;
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
    public class Teams : ITeams
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public Teams(CricketContext context,
            UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Teamdto>> GetAllTeams(string zone, string location, string name, int? page, int? userId)
        {
            int pageSize = 20;

            var model = (await PaginatedList<Teamdto>.CreateAsync(
            _context.Teams
            .AsNoTracking()
            .Where(i => (string.IsNullOrEmpty(zone) || i.Zone == zone)
                    && (string.IsNullOrEmpty(location) || EF.Functions.Like(i.Place, '%' + location + '%'))
                    && (string.IsNullOrEmpty(name) || EF.Functions.Like(i.Team_Name, '%' + name + '%'))
                    && (!userId.HasValue || i.clubAdmin.UserId == userId)
            )
            .Select(i => new Teamdto
            {
                TeamId = i.TeamId,
                Team_Name = i.Team_Name,
                Place = i.Place,
                Zone = i.Zone,
                City = i.City,
                Contact = i.Contact,
                TeamLogo = i.TeamLogo
            })
            .OrderByDescending(i => i.TeamId)
            , page ?? 1, pageSize));

            return model;
        }

        public List<TeamDropDowndto> GetAllTeams()
        {
            var model = _context.Teams
                .AsNoTracking()
                .Select(i => new TeamDropDowndto
                {
                    TeamId = i.TeamId,
                    Team_Name = i.Team_Name
                }).ToList();
            return model;
        }

    }
}
