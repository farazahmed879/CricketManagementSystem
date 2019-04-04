using System.Data;
using System.Diagnostics;
using CricketApp.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly CricketContext _context;

        public HomeController(CricketContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var connection = _context.Database.GetDbConnection();
            var model = connection.QuerySingleOrDefault<HomeScreendto>(
                "[usp_HomeScreen]",
                new
                { },
                commandType: CommandType.StoredProcedure) ?? new HomeScreendto
                {

                };
            ViewBag.Name = "Home";

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult DashBoard()
        {
            var connection = _context.Database.GetDbConnection();
            var model = connection.QuerySingleOrDefault<HomeScreendto>(
                "[usp_HomeScreen]",
                new
                { },
                commandType: CommandType.StoredProcedure) ?? new HomeScreendto
                {

                };
            ViewBag.Name = "Home";

            return View(model);
        }
        [Route("home/HomePage")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult HomePage()
        {
            var connection = _context.Database.GetDbConnection();
            var model = connection.QuerySingleOrDefault<HomeScreendto>(
                "[usp_HomeScreen]",
                new
                { },
                commandType: CommandType.StoredProcedure) ?? new HomeScreendto
                {

                };

            return Json(model);
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
