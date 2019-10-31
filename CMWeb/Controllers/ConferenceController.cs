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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conference == null)
            {
                return NotFound();
            }

            return View(conference);
        }

        // GET: Conference/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conference/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate")] Conference conference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate")] Conference conference)
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
    }
}
