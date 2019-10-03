/*
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
    public class StatManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatManager
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatManager.ToListAsync());
        }

        // GET: StatManager/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statManager = await _context.StatManager
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statManager == null)
            {
                return NotFound();
            }

            return View(statManager);
        }

        // GET: StatManager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] StatManager statManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statManager);
        }

        // GET: StatManager/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statManager = await _context.StatManager.FindAsync(id);
            if (statManager == null)
            {
                return NotFound();
            }
            return View(statManager);
        }

        // POST: StatManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id")] StatManager statManager)
        {
            if (id != statManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatManagerExists(statManager.Id))
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
            return View(statManager);
        }

        // GET: StatManager/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statManager = await _context.StatManager
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statManager == null)
            {
                return NotFound();
            }

            return View(statManager);
        }

        // POST: StatManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var statManager = await _context.StatManager.FindAsync(id);
            _context.StatManager.Remove(statManager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatManagerExists(string id)
        {
            return _context.StatManager.Any(e => e.Id == id);
        }
    }
}
*/