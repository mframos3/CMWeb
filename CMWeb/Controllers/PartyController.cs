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
using Microsoft.AspNetCore.Identity;

namespace CMWeb.Controllers
{
    public class PartyController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public PartyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Party
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.OfType<Party>()
                .Include(p => p.Conference)
                .Include(p => p.EventCenterRoom);
            return View(await applicationDbContext.ToListAsync());
        }
        
        public async Task<IActionResult> Attend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var party = (Party) await _context.Events
                .Include(p => p.Conference)
                .Include(p => p.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }
            
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var eventUser = await _context.EventUsers.FirstOrDefaultAsync(m => m.UserId == currentUserId & m.EventId == party.Id);
            if (eventUser != null)
            {
                return RedirectToAction("Details", routeValues: new {id = party.Id});
            }
            
            var newEventUser = new EventUser();
            newEventUser.UserId = currentUserId;
            newEventUser.EventId = party.Id;
            _context.Add(newEventUser);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", routeValues: new {id = party.Id});
        }
        
        // GET: Party/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = (Party) await _context.Events
                .Include(p => p.Conference)
                .Include(p => p.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var eventUser = await _context.EventUsers.FirstOrDefaultAsync(m => m.UserId == currentUserId & m.EventId == party.Id);
            if (eventUser != null)
            {
                ViewData["Attendance"] = true;
            }
            else
            {
                ViewData["Attendance"] = false;
            }
            
            
            return View(party);
        }

        // GET: Party/Create
        public IActionResult Create(int conferenceId)
        {
            ViewData["ConferenceId"] = conferenceId;
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id");
            return View();
        }

        // POST: Party/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Party party)
        {
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Conference", new {id = party.ConferenceId});
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", party.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", party.EventCenterRoomId);
            return View(party);
        }

        // GET: Party/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = (Party) await _context.Events.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", party.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", party.EventCenterRoomId);
            return View(party);
        }

        // POST: Party/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", party.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", party.EventCenterRoomId);
            return View(party);
        }

        // GET: Party/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = (Party) await _context.Events
                .Include(p => p.Conference)
                .Include(p => p.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Events.FindAsync(id);
            _context.Events.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
