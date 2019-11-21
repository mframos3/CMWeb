using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using CMWeb.Models;
using Microsoft.Extensions.Logging;

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
        public IActionResult Create(int cid, EventType eventType)
        {
            switch (eventType)
            {   
                case EventType.Chat:
                    return RedirectToAction("Create", "Chat", new {conferenceId = cid}); 
                case EventType.Meal:
                    return RedirectToAction("Create", "Meal", new {conferenceId = cid}); 
                case EventType.Party:
                    return RedirectToAction("Create", "Party", new {conferenceId = cid}); 
                case EventType.Talk:
                    return RedirectToAction("Create", "Talk", new {conferenceId = cid}); 
                case EventType.Workshop:
                    return RedirectToAction("Create", "Workshop", new {conferenceId = cid});
                default:
                    return NotFound();
            }
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
        public  async Task<IActionResult> ConflictChecker(DateTime startDate, DateTime endDate, int eventCenterRoomId)
        {
            var room = await _context.EventCenterRooms
                .Include(r => r.Events)
                .FirstOrDefaultAsync(r => r.Id == eventCenterRoomId);
            foreach (var @event in room.Events.Where(@event =>
                @event.EndDate > startDate && endDate > @event.EndDate
                || @event.StartDate < endDate && @event.StartDate > startDate))
            {
                return Json($"The event {@event.Name} is being held on this room in between " +
                            $"dates {@event.StartDate}, {@event.EndDate} \nPlease change the room or dates.");
            }
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DateChecker(DateTime startDate, DateTime endDate, int conferenceId)
        {
            var conference = await _context.Conferences.FirstOrDefaultAsync(r => r.Id == conferenceId);
            if (startDate > endDate)
            {
                return Json("End Date must be after Start Date.");
            }

            if (startDate < conference.StartDate || endDate > conference.EndDate)
            {
                return Json($"The event must be held during the conference it belongs.");
            }
            return Json(true);
        }
    }
}
