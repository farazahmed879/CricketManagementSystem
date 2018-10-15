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

namespace WebApp.Controllers
{

    public class OthersController : Controller
    {
        private readonly CricketContext _context;
        public OthersController(CricketContext context)
        {
            _context = context;
        }




        // View: Others/Developers
        public IActionResult Developers()
        {
            ViewBag.Name = "Developers";
            return View();
        }
        // View: Others/Partners
        public IActionResult Partners()
        {
            ViewBag.Name = "Partners";
            return View();
        }

        // View: Others/AboutUs
        public IActionResult AboutUs()
        {
            ViewBag.Name = "AboutUs";
            return View();
        }
        // View: Others/TestMatch
        public IActionResult TestMatch()
        {
            ViewBag.Name = "AboutUs";
            return View();
        }
        // View: Others/Records
        public IActionResult Records()
        {
            ViewBag.Name = "Records";
            return View();
        }

    }
}
