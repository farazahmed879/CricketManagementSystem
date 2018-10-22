using AutoMapper;
using AutoMapper.QueryableExtensions;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
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

        // GET: Teams

        public async Task<IActionResult> Index(int? day, string month, int? page, int? teamId)
        {
            ViewBag.Name = "Schedule";
            ViewBag.TeamId = teamId;
            int pageSize = 20;
            return View(await PaginatedList<ViewModels.MatchScheduledto>.CreateAsync(
            _context.MatchSchedule
            .AsNoTracking()
            .Where(i => (!day.HasValue || i.Day >= day)
                    && (string.IsNullOrEmpty(month) || i.Month == month)
                    && (!teamId.HasValue || i.TeamId == teamId)
            )
            .Select(i => new ViewModels.MatchScheduledto
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
        [Route("Schedule/CreateSchedule")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> CreateSchedule(MatchScheduledto matchSchedule)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<MatchSchedule>(matchSchedule);
                _context.MatchSchedule.Add(model);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }

        // GET: Schedule/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult GetEdit(int matchScheduleId)
        {
            var matchSchedule = _context.MatchSchedule
                .AsNoTracking()
                .ProjectTo<MatchScheduledto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(m => m.MatchScheduleId == matchScheduleId);
            if (matchSchedule == null)
            {
                return NotFound();
            }
            return Json(matchSchedule);
        }

        // POST: Schedule/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Update(int id, MatchScheduledto dto)
        {
            if (ModelState.IsValid)
            {

                _context.MatchSchedule.Update(_mapper.Map<MatchSchedule>(dto));
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }


        [Route("Schedule/DeleteConfirmed")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(long matchScheduleId)
        {
            var model = await _context.MatchSchedule.SingleOrDefaultAsync(m => m.MatchScheduleId == matchScheduleId);
            _context.MatchSchedule.Remove(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
