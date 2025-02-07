using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CMWeb.Models;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using System.Security.Claims;
using CMWeb.Areas.Identity.Data;

namespace CMWeb.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomePageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IList<Notification> notificationList = new List<Notification>();
            foreach (Notification notification in _context.Notifications.Include(n => n.UserNotifications))
            {
                notificationList.Add(notification);
            }
            ViewData["notifications"] = notificationList;
            ClaimsPrincipal currentUser = this.User;
            ViewData["userId"] = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(currentUser.FindFirstValue(ClaimTypes.NameIdentifier));
            var applicationDbContext = _context.Conferences.Include(conference => conference.Events);
            return View(await applicationDbContext.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}