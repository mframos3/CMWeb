using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMWeb.Models;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;

namespace CMWeb.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomePageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Conferences.Include(conference => conference.Events);
            return View(await applicationDbContext.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}