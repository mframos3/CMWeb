using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using CMWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Stats()
        {
           
            
            ViewData["Events"] = await GetJsonEventsWithAttendance();
            return View();
        }

        private async Task<List<string>> GetJsonEventsWithAttendance()
        {
            var output = new List<string>();
            var eEvents = await _context.Events.ToListAsync();
            var jsonEvents = eEvents.ConvertAll(JsonConvert.SerializeObject).ToList();

            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new Amount()
            {
                EventId = @group.Key,
                Attendance = @group.Count()
            });

            foreach (var jObj in jsonEvents.Select(JObject.Parse))
            {
                var attendance = eventAttendance.FirstOrDefault(eu => eu.EventId.ToString() == (string) jObj["Id"]);
                if (attendance == null)
                {
                    jObj["Attendance"] = 0;
                }
                else
                {
                    jObj["Attendance"] = attendance.Attendance;
                }
                
                output.Add(jObj.ToString());
            }

            return output;
        }

        private class Amount
        {
            public int EventId { get; set; }
            public int Attendance { get; set; }
        }
    }
}
