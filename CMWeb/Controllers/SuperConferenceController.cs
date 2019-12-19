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
    public class SuperConferenceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuperConferenceController(ApplicationDbContext context)
        {
            _context = context;
        }  
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.SuperConferences.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (superConference == null)
            {
                return NotFound();
            }
            
            return View(superConference);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Rating")] SuperConference superConference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(superConference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(superConference);
        }

        // GET: Conference/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superConference = await _context.SuperConferences.FindAsync(id);
            if (superConference == null)
            {
                return NotFound();
            }
            return View(superConference);
        }

        // POST: Conference/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsPeriodic")] SuperConference superConference)
        {
            if (id != superConference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(superConference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperConferenceExists(superConference.Id))
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
            return View(superConference);
        }

        // GET: Conference/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var superConference = await _context.SuperConferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (superConference == null)
            {
                return NotFound();
            }

            return View(superConference);
        }

        // POST: Conference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var superConference = await _context.SuperConferences.FindAsync(id);
            _context.SuperConferences.Remove(superConference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuperConferenceExists(int id)
        {
            return _context.SuperConferences.Any(e => e.Id == id);
        }

      

    }
}