using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleManagementController(CricketContext cricketContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = cricketContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("RoleManagement/Role")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Role()
        {
            // Clear the existing external cookie to ensure a clean login process
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            var model = await _context.Users
                 .Select(i => new AspUserdto
                 {
                     Id = i.Id,
                     UserName = i.UserName,
                     Email = i.Email,
                     PhoneNumber = i.PhoneNumber,
                     Role = i.Role.Name,
                     Team = i.Team.Team_Name

                 }).ToListAsync();

            ViewBag.Team = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");

            //ViewBag.Roles = new SelectList(_context.Role
            //    .Select(i => new { i.Id, i.Name })
            //    , "Id", "Name");

            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Name = "RoleManagement";
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {

            var model = await _context.Users
                .Where(i => i.Id == id)
                 .Select(i => new AspUserdto
                 {
                     Id = i.Id,
                     UserName = i.UserName,
                     Email = i.Email,
                     PhoneNumber = i.PhoneNumber,
                     RoleId = i.Role.Id,
                     TeamId = i.Team.TeamId
                 }).SingleOrDefaultAsync();

            ViewBag.Team = new SelectList(_context.Teams
                .Select(i => new { i.TeamId, i.Team_Name })
                , "TeamId", "Team_Name");


            ViewBag.Roles = new SelectList(_context.Roles
               .Select(i => new { i.Id, i.Name })
               , "Id", "Name");

            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.Name = "RoleManagement";
            return View(model);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(AspUserdto model)
        {




            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (model.ConfirmPassword != model.Password)
                        ModelState.AddModelError(nameof(model.ConfirmPassword), "Confirm password does not match ");
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName;

                await _userManager.UpdateAsync(user);

                var team = new Team { TeamId = model.TeamId.Value };
                _context.Attach(team);

                team.UserId = model.Id;

                await _context.SaveChangesAsync();



                return Json(ResponseHelper.UpdateSuccess());
            }
            return BadRequest(ModelState);
        }


        [HttpPost("RoleManagement/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]ClubAdmindto role)
        {
            if (ModelState.IsValid)
            {
                //var item = _mapper.Map<ClubAdmin>(role);
                //_context.ClubAdmins.Add(item);
                //await _context.SaveChangesAsync();
                //return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }


        [HttpGet]
        public IActionResult Create()
        {


            ViewBag.Users = new SelectList(_context.Users
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