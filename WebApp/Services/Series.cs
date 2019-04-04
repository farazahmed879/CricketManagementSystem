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
    public class Series : ISeries
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;

        public Series(CricketContext context,
            UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<MatchSeriesdto>> GetAllSeries(int? page, int? userId)
        {
            int pageSize = 20;

            var model = await PaginatedList<MatchSeriesdto>.CreateAsync(
               _context.MatchSeries
                .Where(i => !userId.HasValue || i.UserId == userId)
                 .Select(i => new MatchSeriesdto
                 {
                     MatchSeriesId = i.MatchSeriesId,
                     Name = i.Name,
                     Organizor = i.Organizor,
                     StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToString("dddd, dd MMMM yyyy") : "",

                 })
                 .OrderByDescending(i => i.MatchSeriesId)
                , page ?? 1, pageSize);


            return model;
        }
    }
}
