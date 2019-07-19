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
    public class Ground : IGround
    {
        private readonly CricketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Ground(CricketContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<PaginatedList<Grounddto>> GetAllGround(DataTableAjaxPostModel model, int? page)
        {

            var result = await PaginatedList<Grounddto>.CreateAsync(
            _context.Ground
            .Select(i => new ViewModels.Grounddto
            {
                GroundId = i.GroundId,
                Name = i.Name,
                Location = i.Location,

            })
            .OrderByDescending(i => i.GroundId)
            , model.Start, model.Length);

            return result;
        }

    }
}
