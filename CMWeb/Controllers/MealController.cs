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
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MealController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Attend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var meal = (Meal) await _context.Events
                .Include(p => p.Conference)
                .Include(p => p.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }
            
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var eventUser = await _context.EventUsers.FirstOrDefaultAsync(m => m.UserId == currentUserId & m.EventId == meal.Id);
            if (eventUser != null)
            {
                return RedirectToAction("Details", routeValues: new {id = meal.Id});
            }

            var newEventUser = new EventUser {UserId = currentUserId, EventId = meal.Id};
            _context.Add(newEventUser);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", routeValues: new {id = meal.Id});
        }

        // GET: Meal
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.OfType<Meal>().Include(m => m.Conference).Include(m => m.EventCenterRoom).Include(m => m.MealMenus);
            return View(await applicationDbContext.ToListAsync());
        }
        
        

        // GET: Meal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Events.OfType<Meal>()
                .Include(m => m.Conference).ThenInclude(c => c.SuperConference)
                .Include(m => m.EventCenterRoom)
                .Include(m => m.MealMenus).ThenInclude(m => m.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var eventUser = await _context.EventUsers.FirstOrDefaultAsync(m => m.UserId == currentUserId & m.EventId == meal.Id);
            if (eventUser != null)
            {
                ViewData["Attendance"] = true;
            }
            else
            {
                ViewData["Attendance"] = false;
            }

            return View(meal);
        }

        // GET: Meal/Create
        public IActionResult Create(int conferenceId)
        {
            ViewData["ConferenceId"] = conferenceId;
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Name");
            ViewBag.Menus = new MultiSelectList(_context.Menus.ToList(), "Id", "Name");
            return View();
        }

        // POST: Meal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction("SelectMenus", "Meal", new {id = meal.Id});
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", meal.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", meal.EventCenterRoomId);
            return View(meal);
        }

        // GET: Meal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = (Meal) await _context.Events.FindAsync(id);
            if (meal == null)
            {
                return NotFound();
            }
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", meal.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", meal.EventCenterRoomId);
            return View(meal);
        }

        // POST: Meal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Track,ConferenceId,EventCenterRoomId")] Meal meal)
        {
            if (id != meal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.Id))
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
            ViewData["ConferenceId"] = new SelectList(_context.Conferences, "Id", "Id", meal.ConferenceId);
            ViewData["EventCenterRoomId"] = new SelectList(_context.EventCenterRooms, "Id", "Id", meal.EventCenterRoomId);
            return View(meal);
        }

        // GET: Meal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Events.OfType<Meal>()
                .Include(m => m.Conference)
                .Include(m => m.EventCenterRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Meal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Events.FindAsync(id);
            _context.Events.Remove(meal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
        
        // GET: Meal/AddMenus/1
        public async Task<IActionResult> SelectMenus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Events.OfType<Meal>()
                .Include(m => m.Conference).ThenInclude(c => c.SuperConference)
                .Include(m => m.EventCenterRoom)
                .Include(m => m.MealMenus).ThenInclude(m => m.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (meal == null)
            {
                return NotFound();
            }

            ViewBag.Menus = _context.Menus.ToList();
            return View(meal);
        }
        
        // POST: Meal/AddMenus/1

        public async Task<IActionResult> AddMenus(int[] areChecked, int mealId)
        {
            var meal = (Meal) await _context.Events.FirstOrDefaultAsync(e => e.Id == mealId);
            foreach (var menuId in areChecked)
            {
                var menu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == menuId);
                _context.Add(new MealMenu {Menu = menu, Meal = meal});
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Details", "Conference", new {id = meal.ConferenceId});
        }
    }
}
