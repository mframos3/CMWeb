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
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MealController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Meal
        public async Task<IActionResult> Index()
        {
            var meals = _context.Events.OfType<Meal>();
            var applicationDbContext = meals.Include(m => m.Menu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Meal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meals = _context.Events.OfType<Meal>();
            var meal = await meals
                .Include(m => m.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // GET: Meal/Create
        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Id");
            return View();
        }

        // POST: Meal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,Id,Name,StartDate,EndDate,Track")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Id", meal.MenuId);
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
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Id", meal.MenuId);
            return View(meal);
        }

        // POST: Meal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,Id,Name,StartDate,EndDate,Track")] Meal meal)
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
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Id", meal.MenuId);
            return View(meal);
        }

        // GET: Meal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meals = _context.Events.OfType<Meal>();
            var meal = await meals
                .Include(m => m.Menu)
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
    }
}
