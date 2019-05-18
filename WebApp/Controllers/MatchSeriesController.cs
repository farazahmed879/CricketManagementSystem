using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApp.ViewModels;
using AutoMapper.QueryableExtensions;
using WebApp.Helper;
using WebApp.IServices;

namespace WebApp.Controllers
{

    public class MatchSeriesController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly IMapper _mapper;
        private readonly ISeries _series;

        public MatchSeriesController(CricketContext context,
            UserManager<IdentityUser<int>> userManager, ISeries series,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _series = series;

        }

        // GET: MatchSeries
        [HttpGet]
        public async Task<IActionResult> Index(DataTableAjaxPostModel model, int? page, bool isApi)
        {
            ViewBag.Name = "Series";
            var result = await _series.GetAllSeries(model.Init(),page);
            if (isApi == true)
                return Json(new
                {
                    data = result,
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                });
            else
                return View(result);

        }

        [HttpGet]
        [Route("MatchSeries/List")]
        public async Task<IActionResult> List(DataTableAjaxPostModel model, int? page)
        {
            ViewBag.Name = "Series";
            var result = await _series.GetAllSeries(model.Init(),page);
            return Json(result);

        }

        // GET: MatchSeries/Details/5
        [HttpGet("MatchSeries/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matchSeries = await _context.MatchSeries
                .SingleOrDefaultAsync(m => m.MatchSeriesId == id);
            if (matchSeries == null)
            {
                return NotFound();
            }

            return View(matchSeries);
        }

        // GET: MatchSeries/Create
        [HttpGet("MatchSeries/Create")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add MatchSeries";
            return View();
        }

        // POST: MatchSeries/Create
        [HttpPost("MatchSeries/Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(MatchSeriesdto matchSeries)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.GetUserAsync(HttpContext.User);
                var matchSeriesModal = _mapper.Map<MatchSeries>(matchSeries);
                matchSeriesModal.UserId = users.Id;
                _context.MatchSeries.Add(matchSeriesModal);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return View(matchSeries);
        }

        // GET: MatchSeries/Edit/5
        [HttpGet("MatchSeries/Edit/{id}")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Name = "Edit MatchSeries";
            if (id == null)
            {
                return NotFound();
            }

            var matchSeries = await _context.MatchSeries
                .AsNoTracking()
                .Select(i => new MatchSeriesdto
                {
                    MatchSeriesId = i.MatchSeriesId,
                    Name = i.Name,
                    Organizor = i.Organizor,
                    StartingDate = i.StartingDate.HasValue ? i.StartingDate.Value.ToShortDateString() : "",

                })
                .SingleOrDefaultAsync(m => m.MatchSeriesId == id);
            if (matchSeries == null)
            {
                return NotFound();
            }
            return View(matchSeries);
        }


        [HttpPut("MatchSeries/Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(MatchSeriesdto matchSeries)
        {

            if (ModelState.IsValid)
            {

                var users = await _userManager.GetUserAsync(HttpContext.User);
                var matchSeriesModal = _mapper.Map<MatchSeries>(matchSeries);
                matchSeriesModal.UserId = users.Id;
                _context.MatchSeries.Update(matchSeriesModal);
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }

            return Json(ResponseHelper.UpdateUnSuccess());
        }


        // POST: MatchSeries/Delete/5
        [HttpDelete("MatchSeries/DeleteConfirmed/{matchSeriesId}")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int matchSeriesId)
        {
            var matchSeries = await _context.MatchSeries.SingleOrDefaultAsync(m => m.MatchSeriesId == matchSeriesId);
            _context.MatchSeries.Remove(matchSeries);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
