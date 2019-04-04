using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Helper;
using WebApp.ViewModels.RoleManagement;

namespace WebApp.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly CricketContext _context;
        private readonly IMapper _mapper;
        public RoleManagementController(CricketContext cricketContext, IMapper mapper)
        {
            _context = cricketContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Role()
        {
            // Clear the existing external cookie to ensure a clean login process
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var model = await _context.ClubAdmins
                 .Select(i => new ClubAdmindto
                 {
                     UserId = i.User.Id,
                     UserName = i.User.UserName,
                     TeamId = i.Team.TeamId,
                     TeamName = i.Team.Team_Name
                 }).ToListAsync();

            ViewBag.Team = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Name = "RoleManagement";
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int userId, string UserName)
        {

            ViewBag.User = UserName;
            var model = await _context.ClubAdmins
                .Where(i => i.UserId == userId)
                 .Select(i => new ClubAdmindto
                 {
                     UserName = i.User.UserName,
                     TeamId = i.TeamId,
                     TeamName = i.Team.Team_Name
                 }).ToListAsync();

            ViewBag.Team = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Name = "RoleManagement";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]ClubAdmindto role)
        {
            if (ModelState.IsValid)
            {
                var item = _mapper.Map<ClubAdmin>(role);
                _context.ClubAdmins.Add(item);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }


        [HttpGet]
        public IActionResult Create()
        {


            ViewBag.Users = new SelectList( _context.User
                .Select(i => new { i.Id, i.UserName })
                , "Id", "UserName");
            ViewBag.Team = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Name = "Assign Role";
            return View();
        }

    }
}