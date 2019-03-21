using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using System.IO;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApp.ViewModels;
using AutoMapper.QueryableExtensions;
using WebApp.Helper;
using WebApp.IServices;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class TeamsController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly ITeams _teams;

        public TeamsController(CricketContext context,
            UserManager<IdentityUser<int>> userManager,
            IMapper mapper, ITeams teams
            )
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _teams = teams;
        }

        [Route("Team/GetAllTeams")]
        public List<TeamDropDowndto> GetAllTeams()
        {
            var teams = _teams.GetAllTeams();
            return teams;
        }

        // GET: Teams

        public async Task<IActionResult> Index(string zone, string location, string name, int? page, int? userId)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;
            ViewBag.Name = "Team";
            var model = await _teams.GetAllTeams(zone, location, name, page, userId);
            return View(model);
        }

        [Route("Team/List")]
        public async Task<IActionResult> List(string zone, string location, string name, int? page, int? userId)
        {
            var users = await _userManager.GetUserAsync(HttpContext.User);
            if (users != null)
                userId = users.Id;
            ViewBag.Name = "Team";
            var model = await _teams.GetAllTeams(zone, location, name, page, userId);
            return Json(model);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? teamId)
        {
            var team = await _context.Teams
                .Select(i => new Teamdto
                {
                    TeamId = i.TeamId,
                    Team_Name = i.Team_Name,
                    TeamLogo = i.TeamLogo,
                    Place = i.Place,
                    Zone = i.Zone,
                    Contact = i.Contact,
                    IsRegistered = i.IsRegistered,
                    City = i.City,
                })
            .SingleOrDefaultAsync(m => m.TeamId == teamId);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add Team";
            return View();
        }

        // Post: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Teamdto team)
        {
            if (ModelState.IsValid)
            {
                var form = Request.Form;
                byte[] fileBytes = null;
                if (team.TeamImage != null)
                {
                    using (var stream = team.TeamImage.OpenReadStream())
                    {
                        fileBytes = ReadStream(stream);

                    }
                }


                team.TeamLogo = fileBytes ?? null;
                var teamModel = _mapper.Map<Team>(team);
                _context.Teams.Add(teamModel);
                var users = await _userManager.GetUserAsync(HttpContext.User);
                _context.ClubAdmins.Add(new ClubAdmin
                {
                    TeamId = teamModel.TeamId,
                    UserId = users.Id

                });
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }
        public static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        // GET: Teams/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {

            ViewBag.Name = "Edit Team";
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .AsNoTracking()
                .ProjectTo<Teamdto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(Teamdto team)
        {
            if (ModelState.IsValid)
            {

                var form = Request.Form;
                byte[] fileBytes = null;
                if (team.TeamImage != null)
                {
                    using (var stream = team.TeamImage.OpenReadStream())
                    {
                        fileBytes = ReadStream(stream);
                    }
                }
                team.TeamLogo = fileBytes ?? null;
                _context.Teams.Update(_mapper.Map<Team>(team));
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UnSuccess());
        }


        [Route("Team/DeleteConfirmed")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int teamId)
        {
            var team = await _context.Teams.SingleOrDefaultAsync(m => m.TeamId == teamId);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
