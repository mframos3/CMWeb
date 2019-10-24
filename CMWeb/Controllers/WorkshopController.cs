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
    public class WorkshopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkshopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Workshop
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.OfType<Workshop>().ToListAsync());
        }

        // GET: Workshop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = (Workshop) await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // GET: Workshop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workshop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Track")] Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workshop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workshop);
        }

        // GET: Workshop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = (Workshop) await _context.Events.FindAsync(id);
            if (workshop == null)
            {
                return NotFound();
            }
            return View(workshop);
        }

        // POST: Workshop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Track")] Workshop workshop)
        {
            if (id != workshop.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(workshop);
            try
            {
                _context.Update(workshop);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkshopExists(workshop.Id))
                {
                    return NotFound();
                }
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Workshop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = (Workshop) await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // POST: Workshop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workshop = await _context.Events.FindAsync(id);
            _context.Events.Remove(workshop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkshopExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
