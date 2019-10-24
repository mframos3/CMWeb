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
            return View(await _context.Events.OfType<Party>().ToListAsync());
        }

        // GET: Party/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var party = (Party) await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // GET: Party/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Party/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Track")] Party party)
        {
            if (!ModelState.IsValid) return View(party);
            _context.Add(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            return View(party);
        }

        // POST: Party/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Track")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }
            
            if (!ModelState.IsValid) return View(party);
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
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Party/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var party = (Party) await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
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
