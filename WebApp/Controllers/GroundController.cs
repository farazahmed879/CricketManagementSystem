using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using WebApp.ViewModels;
using WebApp.Helper;
using WebApp.IServices;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Controllers
{

    public class GroundController : Controller
    {
        private readonly CricketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IGround _grounds;
        private readonly IHostingEnvironment _hosting;

        public GroundController(CricketContext context,
            UserManager<ApplicationUser> userManager, IGround grounds, IHostingEnvironment hosting,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _grounds = grounds;
            _hosting = hosting;
        }

        // GET: Grounds
        [HttpGet]
        public async Task<IActionResult> Index(DataTableAjaxPostModel model, int? page, bool isApi)
        {
            ViewBag.Name = "Grounds";

            var result = await _grounds.GetAllGround(model.Init(), page);

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

        [HttpGet("Ground/List")]
        public async Task<IActionResult> List(DataTableAjaxPostModel model, int? page)
        {
            ViewBag.Name = "Grounds";
            var result = await _grounds.GetAllGround(model.Init(), page);

            return Json(result);
        }

        [HttpGet("Ground/Details/{id}")]
        // GET: Grounds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ground = await _context.Ground
                .SingleOrDefaultAsync(m => m.GroundId == id);
            if (ground == null)
            {
                return NotFound();
            }

            return View(ground);
        }
        [AllowAnonymous]
        [HttpGet("Ground/Create")]
        // GET: Ground/Create
        [Authorize(Roles = "Club Admin,Administrator")]
        public IActionResult Create()
        {
            ViewBag.Name = "Add Ground";
            return View();
        }

        // POST: Ground/Create
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("Ground/Create")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Create(Grounddto ground)
        {
            if (ModelState.IsValid)
            {
                //var directory = Path.Combine(_hosting.WebRootPath, "Home", "images", "Ground");
                //if (!Directory.Exists(directory))
                //    Directory.CreateDirectory(directory);
                //if (ground.GroundImage != null)
                //{
                //    ground.FileName = ground.GroundImage.FileName;
                //    using (var stream = new FileStream(Path.Combine(directory, ground.FileName), FileMode.Create))
                //    {
                //        await ground.GroundImage.CopyToAsync(stream);
                //    }
                //}

                var users = await _userManager.GetUserAsync(HttpContext.User);
                var groundModel = _mapper.Map<Ground>(ground);
                //groundModel.UserId = users.Id;
                _context.Ground.Add(groundModel);
                await _context.SaveChangesAsync();
                return Json(ResponseHelper.Success());
            }
            return Json(ResponseHelper.UnSuccess());
        }
        [AllowAnonymous]
        [HttpGet("Ground/Edit/{id}")]
        // GET: Grounds/Edit/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Name = "Edit Ground";
            if (id == null)
            {
                return NotFound();
            }

            var ground = await _context.Ground
                .AsNoTracking()
                .Select(i => new ViewModels.Grounddto
                {
                    GroundId = i.GroundId,
                    Name = i.Name,
                    Location = i.Location,

                })
                .SingleOrDefaultAsync(m => m.GroundId == id);
            if (ground == null)
            {
                return NotFound();
            }
            return View(ground);
        }
        [AllowAnonymous]
        // POST: Grounds/Edit/5
        [HttpPut("Ground/Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> Edit(Grounddto ground)
        {

            if (ModelState.IsValid)
            {
                //var directory = Path.Combine(_hosting.WebRootPath, "Home", "images", "Ground");
                //if (!Directory.Exists(directory))
                //    Directory.CreateDirectory(directory);
                //if (ground.GroundImage != null)
                //{
                //    ground.FileName = ground.GroundImage.FileName;
                //    using (var stream = new FileStream(Path.Combine(directory, ground.FileName), FileMode.Create))
                //    {
                //        await ground.GroundImage.CopyToAsync(stream);
                //    }
                //}
                var users = await _userManager.GetUserAsync(HttpContext.User);
                var groundModal = _mapper.Map<Ground>(ground);
                //groundModal.UserId = users.Id;
                _context.Ground.Update(groundModal);
                await _context.SaveChangesAsync();

                return Json(ResponseHelper.UpdateSuccess());
            }
            return Json(ResponseHelper.UpdateUnSuccess());
        }
        [AllowAnonymous]
        [HttpDelete("Ground/DeleteConfirmed/{groundId}")]
        // POST: Grounds/Delete/5
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int groundId)
        {
            var ground = await _context.Ground.SingleOrDefaultAsync(m => m.GroundId == groundId);
            _context.Ground.Remove(ground);
            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpPut]
        [Route("Ground/Add")]
        [Authorize(Roles = "Club Admin,Administrator")]
        public async Task<int> AddGroundAsync([FromBody]Grounddto ground)
        {
            if (ModelState.IsValid)
            {
                var groundModel = _mapper.Map<Ground>(ground);
                await _context.Ground.AddAsync(groundModel);
                await _context.SaveChangesAsync();
                return groundModel.GroundId;
            }
            return 0;

        }
    }
}
