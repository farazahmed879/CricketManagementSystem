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

        public async Task<List<Tournamentdto>> GetAllTournaments(int? page, int? userId)
        {
            int pageSize = 20;

            var model = await PaginatedList<Tournamentdto>.CreateAsync(
            _context.Tournaments
           .Where(i => !userId.HasValue || i.UserId == userId)
            .Select(i => new ViewModels.Tournamentdto
            {
                TournamentId = i.TournamentId,
                TournamentName = i.TournamentName,
                Organizor = i.Organizor,
                StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToString("dddd, dd MMMM yyyy") : "",

            })
            .OrderByDescending(i => i.TournamentId)
            , page ?? 1, pageSize);

            return model;
        }

    }
}
