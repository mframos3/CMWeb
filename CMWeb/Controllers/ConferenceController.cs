using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using CMWeb.Models;

namespace CMWeb.Controllers
{
    public class ConferenceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConferenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Conference
        public async Task<IActionResult> Index()
        {
            return View(await _context.Conferences.ToListAsync());
        }

        // GET: Conference/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            
            var conference = await _context.Conferences
                .Include(c => c.SuperConference)
                .Include(c => c.Events)
                .ThenInclude(e => e.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }
            ViewData["EventCenterName"] = await _context.EventCenters.FindAsync(conference.EventCenterId);

            // ViewData["Events"] = conference.Events;
            return View(conference);
        }

        // GET: Conference/Create
        public IActionResult Create(int superConferenceId)
        {
            ViewData["superConferenceId"] = superConferenceId;
            ViewData["EventCenterId"] = new SelectList(_context.EventCenters, "Id", "Name");
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["CurrentUserId"] = currentUserId;
            return View();
        }

        // POST: Conference/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Edition,Description,StartDate,EndDate,SuperConferenceId,Sponsor,EventCenterId")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conference);
                await _context.SaveChangesAsync();
                var superConference = await _context.SuperConferences.FindAsync(conference.SuperConferenceId);
                superConference.Conferences.Add(conference);
                return RedirectToAction("Index", "SuperConference");
            }
            return View(conference);
        }

        // GET: Conference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences.FindAsync(id);
            if (conference == null)
            {
                return NotFound();
            }
            return View(conference);
        }

        // POST: Conference/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,SuperConferenceId")] Conference conference)
        {
            if (id != conference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConferenceExists(conference.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conference);
        }

        // GET: Conference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conference = await _context.Conferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // POST: Conference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conference = await _context.Conferences.FindAsync(id);
            _context.Conferences.Remove(conference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConferenceExists(int id)
        {
            return _context.Conferences.Any(e => e.Id == id);
        }
        
        [AcceptVerbs("Get", "Post")]
        public  IActionResult DateChecker(DateTime startDate, DateTime endDate)
        {
            return startDate > endDate ? Json("End Date must be after Start Date.") : Json(true);
        }
    }
}
