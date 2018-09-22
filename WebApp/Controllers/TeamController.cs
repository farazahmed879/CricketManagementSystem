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

namespace WebApp.Controllers
{
    public class TeamsController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private Task<IdentityUser<int>> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public TeamsController(CricketContext context, UserManager<IdentityUser<int>> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Teams

        public async Task<IActionResult> Index(string zone, string city, int? page, int? userId)
        {

            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Team";
            int pageSize = 20;
            if (users != null)
                userId = users.Id;

            return View(await PaginatedList<ViewModels.Teamdto>.CreateAsync(
            _context.Teams
            .AsNoTracking()
            .Where(i => (string.IsNullOrEmpty(zone) || i.Zone == zone)
                    && (string.IsNullOrEmpty(city) || i.City == city)
                    && (!userId.HasValue || i.clubAdmin.UserId == userId)
            )
            .Select(i => new ViewModels.Teamdto
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

        }
        // GET: RoleManagement/
        [Route("Teams/RoleManagement")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> RoleManagement(int? page)
        {

            ViewBag.ClubUsers = new SelectList(_context.UserRole
              .AsNoTracking()
               .Where(i => i.RoleId == 17)
               .Select(i => new
               {
                   i.User.UserName,
                   i.User.Id
               }), "Id", "UserName");


            //   ViewBag.ClubUsers = new SelectList(_context.User, "Id", "UserName");
            var users = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Name = "Role Management";
            var RoleManagement = _context.Teams
                .Where(i => i.clubAdmin.UserId == users.Id)
                .Select(i => new { i.TeamId, i.Team_Name, i.IsRegistered, i.clubAdmin.UserId })
                .ToList();
            return View(RoleManagement);
        }

        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> RoleManagementUpdate(int id, Team team)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok();
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .SingleOrDefaultAsync(m => m.TeamId == id);
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

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Team team)
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
                _context.Add(team);
                var user = await GetCurrentUserAsync();
                _context.ClubAdmins.Add(new ClubAdmin
                {
                    TeamId = team.TeamId,
                    UserId = user?.Id

                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
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

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.TeamId == id);
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
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
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

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.TeamId == id);
        }
    }
}
