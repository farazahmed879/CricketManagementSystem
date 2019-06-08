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
    public class Series : ISeries
    {
        private readonly CricketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Series(CricketContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PaginatedList<MatchSeriesdto>> GetAllSeries(DataTableAjaxPostModel model, int? page)
        {

            var result = await PaginatedList<MatchSeriesdto>.CreateAsync(
               _context.MatchSeries
                 .Select(i => new MatchSeriesdto
                 {
                     MatchSeriesId = i.MatchSeriesId,
                     Name = i.Name,
                     Organizor = i.Organizor,
                     FileName = i.FileName ?? "noImage.jpg",
                     StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToString("dddd, dd MMMM yyyy") : "",

                 })
                 .OrderByDescending(i => i.MatchSeriesId)
                , model.Start, model.Length);


            return result;
        }
    }
}
