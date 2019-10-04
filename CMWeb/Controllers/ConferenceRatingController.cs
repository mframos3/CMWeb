using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using CMWeb.Models;

namespace CMWeb.Controllers
{
    public class ConferenceRatingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConferenceRatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notification/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rating,Comment")] ConferenceRating conferenceRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conferenceRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(conferenceRating);
        }

       
        
    }
}
