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
    public class Tournaments : ITournaments
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public Tournaments(CricketContext context,
            UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PaginatedList<Tournamentdto>> GetAllTournaments(DataTableAjaxPostModel model, int? page)
        {

            var result = await PaginatedList<Tournamentdto>.CreateAsync(
            _context.Tournaments
            .Select(i => new ViewModels.Tournamentdto
            {
                TournamentId = i.TournamentId,
                TournamentName = i.TournamentName,
                Organizor = i.Organizor,
                StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToString("dddd, dd MMMM yyyy") : "",

            })
            .OrderByDescending(i => i.TournamentId)
            , model.Start, model.Length);

            return result;
        }

    }
}
