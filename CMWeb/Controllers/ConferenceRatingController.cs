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
        
        
        public async Task<IActionResult> Index(int conferenceId)
        {
            var filteredrate = new List<ConferenceRating>();
            var rates = await _context.ConferenceRatings.ToListAsync();
            foreach (var rate in rates)
            {
                if (rate.ConferenceId == conferenceId)
                {
                    filteredrate.Add(rate);
                }
            }
            return View(filteredrate);
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
        public async Task<IActionResult> Create([Bind("Id,Rating,Comment,ConferenceId,UserId")] ConferenceRating conferenceRating)
        {
            var user = _context.ConferenceRatings.FirstOrDefault(u => u.UserId == conferenceRating.UserId);
            var conference = _context.ConferenceRatings.FirstOrDefault(u => u.ConferenceId == conferenceRating.ConferenceId);
            if (user == null || conference == null) 
            {
                _context.Add(conferenceRating);
                await _context.SaveChangesAsync();
            }
            return Redirect($"/Conference/Details/{conferenceRating.ConferenceId}");

        }
    }
}
