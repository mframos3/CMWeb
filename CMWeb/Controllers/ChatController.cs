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
using Microsoft.EntityFrameworkCore.Internal;

namespace CMWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chat
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.OfType<Chat>().Include(c => c.Conference).Include(c => c.EventCenterRoom);
            return View(await applicationDbContext.ToListAsync());
        }
        
        public async Task<IActionResult> Attend(int? id, UserType type)
        {
            if (id == null)
            {
                return NotFound();
            }
            var chat = (Chat) await _context.Events
                .Include(p => p.Conference).ThenInclude(c => c.SuperConference)
                .Include(p => p.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }
            
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var eventUser = await _context.EventUsers.FirstOrDefaultAsync(m => m.UserId == currentUserId & m.EventId == chat.Id);
            if (eventUser != null)
            {
                return RedirectToAction("Details", routeValues: new {id = chat.Id});
            }
            
            var newEventUser = new EventUser();
            newEventUser.UserId = currentUserId;
            newEventUser.EventId = chat.Id;
            newEventUser.Type = type;
            _context.Add(newEventUser);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", routeValues: new {id = chat.Id});
        }
        
        
        
        // GET: Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = (Chat) await _context.Events
                .Include(c => c.Conference)
                .Include(c => c.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["CurrentUserId"] = currentUserId;

            var eventUser = await _context.EventUsers.FirstOrDefaultAsync(m => m.UserId == currentUserId & m.EventId == chat.Id);
            if (eventUser != null)
            {
                ViewData["Attendance"] = true;
            }
            else
            {   
                ViewData["Attendance"] = false;
            }

            var speaker = _context.EventUsers.Where(eu => eu.Type == UserType.Speaker).FirstOrDefault(eu => eu.EventId == id);
            ViewData["Speaker"] = speaker != null;
            return View(chat);
        }

        // GET: Chat/Create
        public IActionResult Create(int conferenceId)
        {
            ViewData["ConferenceId"] = conferenceId;
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Name");
            return View();
        }

        // POST: Chat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Chat chat)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Conference", new {id = chat.ConferenceId});
            }
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", chat.EventCenterRoomId);
            return View(chat);
        }

        // GET: Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = (Chat) await _context.Events.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", chat.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", chat.EventCenterRoomId);
            return View(chat);
        }

        // POST: Chat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", chat.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", chat.EventCenterRoomId);
            return View(chat);
        }

        // GET: Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = (Chat) await _context.Events
                .Include(c => c.Conference)
                .Include(c => c.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Events.FindAsync(id);
            _context.Events.Remove(chat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
