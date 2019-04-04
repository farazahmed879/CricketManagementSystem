using AutoMapper;
using AutoMapper.QueryableExtensions;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helper;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly CricketContext _context;
        private readonly IMapper _mapper;

        public ScheduleController(CricketContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

  
        [HttpGet]
        public async Task<IActionResult> Index(int? day, string month, int? year, int? page, int? teamId)
        {
            ViewBag.Name = "Schedule";
            ViewBag.TeamId = teamId;
            ViewBag.Year = new SelectList(_context.MatchSchedule
                .AsNoTracking()
                .Select(i => i.Year)
                .ToList().Distinct(), "Year");

            int pageSize = 20;
            return View(await PaginatedList<ViewModels.MatchScheduledto>.CreateAsync(
            _context.MatchSchedule
            .AsNoTracking()
            .Where(i => (!day.HasValue || i.Day >= day)
                    && (string.IsNullOrEmpty(month) || i.Month == month)
                    && (!year.HasValue || i.Year == year)
                    && (!teamId.HasValue || i.TeamId == teamId)
            )
            .Select(i => new MatchScheduledto
            {
                TeamId = i.TeamId,
                GroundName = i.GroundName,
                MatchScheduleId = i.MatchScheduleId,
                Day = i.Day,
                Month = i.Month,
                Year = i.Year,
                OpponentTeam = i.OpponentTeam
            })
            .OrderByDescending(i => i.TeamId)
            , page ?? 1, pageSize));

        }


        // Post: Schedule/CreateSchedule
        [HttpPost]
        [Route("Schedule/CreateSchedule")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> CreateSchedule([FromBody]MatchScheduledto matchSchedule)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<MatchSchedule>(matchSchedule);
                _context.MatchSchedule.Update(model);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }

        // GET: Schedule/Edit/5
        [HttpGet]
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult GetEdit(int matchScheduleId)
        {
            var matchSchedule = _context.MatchSchedule
                .AsNoTracking()
                .Select(i => new MatchScheduledto
                {
                    MatchScheduleId = i.MatchScheduleId,
                    GroundName = i.GroundName,
                    OpponentTeam = i.OpponentTeam,
                    Day = i.Day,
                    Month = i.Month,
                    Year = i.Year,
                    TeamId = i.TeamId
                })
                .SingleOrDefault(m => m.MatchScheduleId == matchScheduleId);
            if (matchSchedule == null)
            {
                return NotFound();
            }
            return Json(matchSchedule);
        }

        [HttpDelete]
        [Route("Schedule/DeleteConfirmed")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int matchScheduleId)
        {
            var model = await _context.MatchSchedule.SingleOrDefaultAsync(m => m.MatchScheduleId == matchScheduleId);
            _context.MatchSchedule.Remove(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
