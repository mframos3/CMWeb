using System.Linq;
using System.Threading.Tasks;
using CMWeb.Data;
using CMWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMWeb.Controllers
{
    public class FileDbController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FileDbController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> AddFile(int eventId, string name, string route, string type)
        {
            var eEvent = await _context.Events.FirstOrDefaultAsync(m => m.Id == eventId);
            var fileDb = new FileDetails() { EventId = eventId, Name = name, Path = route , Event = eEvent};
            if (!ModelState.IsValid) 
                return RedirectToAction("Details", type, new {Id = eventId});
            _context.Files.Add(fileDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", type, new {Id = eventId});
        }
    }
}