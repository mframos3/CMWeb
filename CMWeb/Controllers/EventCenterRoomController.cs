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
    public class EventCenterRoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventCenterRoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventCenterRoom
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCenterRooms.ToListAsync());
        }

        // GET: EventCenterRoom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCenterRoom = await _context.EventCenterRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCenterRoom == null)
            {
                return NotFound();
            }

            return View(eventCenterRoom);
        }

        // GET: EventCenterRoom/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventCenterRoom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Capacity,Location,Equipment")] EventCenterRoom eventCenterRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventCenterRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCenterRoom);
        }

        // GET: EventCenterRoom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCenterRoom = await _context.EventCenterRooms.FindAsync(id);
            if (eventCenterRoom == null)
            {
                return NotFound();
            }
            return View(eventCenterRoom);
        }

        // POST: EventCenterRoom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Capacity,Location,Equipment")] EventCenterRoom eventCenterRoom)
        {
            if (id != eventCenterRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCenterRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCenterRoomExists(eventCenterRoom.Id))
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
            return View(eventCenterRoom);
        }

        // GET: EventCenterRoom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCenterRoom = await _context.EventCenterRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCenterRoom == null)
            {
                return NotFound();
            }

            return View(eventCenterRoom);
        }

        // POST: EventCenterRoom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCenterRoom = await _context.EventCenterRooms.FindAsync(id);
            _context.EventCenterRooms.Remove(eventCenterRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCenterRoomExists(int id)
        {
            return _context.EventCenterRooms.Any(e => e.Id == id);
        }
    }
}
