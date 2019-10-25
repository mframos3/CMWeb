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
    public class TalkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TalkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Talk
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.OfType<Talk>().Include(t => t.Conference).Include(t => t.EventCenterRoom);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Talk/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talk = (Talk) await _context.Events
                .Include(t => t.Conference)
                .Include(t => t.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talk == null)
            {
                return NotFound();
            }

            return View(talk);
        }

        // GET: Talk/Create
        public IActionResult Create()
        {
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id");
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id");
            return View();
        }

        // POST: Talk/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Topic,Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Talk talk)
        {
            if (ModelState.IsValid)
            {
                _context.Add(talk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", talk.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", talk.EventCenterRoomId);
            return View(talk);
        }

        // GET: Talk/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talk = (Talk) await _context.Events.FindAsync(id);
            if (talk == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", talk.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", talk.EventCenterRoomId);
            return View(talk);
        }

        // POST: Talk/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Topic,Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Talk talk)
        {
            if (id != talk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(talk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TalkExists(talk.Id))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", talk.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", talk.EventCenterRoomId);
            return View(talk);
        }

        // GET: Talk/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talk = (Talk) await _context.Events
                .Include(t => t.Conference)
                .Include(t => t.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talk == null)
            {
                return NotFound();
            }

            return View(talk);
        }

        // POST: Talk/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var talk = await _context.Events.FindAsync(id);
            _context.Events.Remove(talk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TalkExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
