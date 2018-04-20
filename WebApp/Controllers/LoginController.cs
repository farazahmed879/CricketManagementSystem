using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CricketApp.Data;
using CricketApp.Domain;
using System.IO;
using WebApp.Utilities;
using Dapper;
using WebApp.ViewModels;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly CricketContext _context;

        public LoginController(CricketContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            return View();

        }
        // GET: Matches/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: SignUp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Login SignUp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(SignUp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(SignUp);
        }
        [HttpGet]
        public IActionResult IsEmailMatch(string email, string password)
        {
            return Json(_context.Login
                .AsNoTracking()
                .Any(i => i.Email == email && i.Password == password));
        }

        [HttpGet]
        public IActionResult IsEmailAvailable(string email)
        {
            return Json( _context.Login
                .AsNoTracking()
                .Any(i => i.Email == email));
        }

    }
}
