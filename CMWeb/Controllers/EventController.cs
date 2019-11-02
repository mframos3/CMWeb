using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using CMWeb.Models;
using System.Text.Encodings.Web;

namespace CMWeb.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(@event => @event.Conference)
                .Include(@event => @event.EventCenterRoom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Track
        public async Task<IActionResult> Track(string track)
        {
            ViewData["Track"] = track;
            var applicationDbContext = _context.Events
                .Include(@event => @event.Conference)
                .Include(@event => @event.EventCenterRoom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(ev => ev.Conference)
                .Include(ev => ev.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id");
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id");
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", @event.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", @event.EventCenterRoomId);
            return View(@event);
        }

        // GET: /HelloWorld/Welcome/ 
        // Requires using System.Text.Encodings.Web;
        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", @event.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", @event.EventCenterRoomId);
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", @event.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", @event.EventCenterRoomId);
            return View(@event);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(ev => ev.Conference)
                .Include(ev => ev.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult ConflictChecker(DateTime startDate, DateTime endDate, EventCenterRoom room)
        {
            foreach (var @event in room.Events)
            {
                if (@event.EndDate > startDate)
                {
                    return Json($"{room.Location} is busy with event {@event.Name} at this moment.");
                }

                if (@event.StartDate < endDate)
                {
                    return Json($"{room.Location} is busy with event {@event.Name} at this moment.");
                }
            }

            return Json(true);
        }
    }
}
