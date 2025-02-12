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
    public class EventRatingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventRatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int eventId)
        {
            var filteredrate = new List<EventRating>();
            var rates = await _context.EventRatings.ToListAsync();
            foreach (var rate in rates)
            {
                if (rate.EventId == eventId)
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
        public async Task<IActionResult> Create([Bind("Id,Rating,Comment,UserId,EventId,SpeakerRating")] EventRating eventRating)
        {
            var user = _context.EventRatings.FirstOrDefault(u => u.UserId == eventRating.UserId);
            var speaker = _context.EventUsers.Where(s =>s.Type == UserType.Speaker).First(s => s.EventId == eventRating.EventId);
            var ratingCount = _context.EventRatings.Count(e => e.EventId == eventRating.EventId);
            
            Console.WriteLine(speaker.Rating);
            
            speaker.Rating = ((speaker.Rating*ratingCount)+(eventRating.SpeakerRating))/(ratingCount+1);
            
            Console.WriteLine(ratingCount);
            var ev = _context.EventRatings.FirstOrDefault(u => u.EventId == eventRating.EventId);
            if (user == null || ev == null) 
            {
                _context.Add(eventRating);
                await _context.SaveChangesAsync();
                
            }

            return Redirect("/SuperConference");

        }

       
        
    }
}