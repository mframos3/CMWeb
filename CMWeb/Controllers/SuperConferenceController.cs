using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMWeb.Data;
using CMWeb.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        
        // GET: SuperConference/Stats/5
        public async Task<IActionResult> Stats(int id)
        {
            ViewData["ConferencesEventsAttendance"] = await GetConferencesEventsAttendance(id);
            ViewData["EventsAttendance"] = await GetEventsAttendance(id);
            ViewData["ConferencesTrackAttendance"] = await GetConferencesTrackAttendance(id);
            ViewData["TracksAttendance"] = await GetTracksAttendance(id);
            return View(await _context.SuperConferences.ToListAsync());
        }
        
        // GET: SuperConference/Stats
        public async Task<IActionResult> StatIndex()
        {
            return View(await _context.SuperConferences.ToListAsync());
        }
        
        
        private async Task<List<string>> GetConferencesEventsAttendance(int superConferenceId)
        {
            var JObjEvents = new List<JObject>();
            var eEvents = await _context.Events
                .Where(e => e.Conference.SuperConferenceId == superConferenceId)
                .ToListAsync();
            var jsonEvents = eEvents.ConvertAll(JsonConvert.SerializeObject).ToList();

            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new Amount()
            {
                Id = @group.Key,
                Attendance = @group.Count()
            });

            foreach (var jObj in jsonEvents.Select(JObject.Parse))
            {
                var attendance = eventAttendance.FirstOrDefault(eu => eu.Id.ToString() == (string) jObj["Id"]);
                if (attendance == null)
                {
                    jObj["Attendance"] = 0;
                }
                else
                {
                    jObj["Attendance"] = attendance.Attendance;
                }
                
                JObjEvents.Add(jObj);
            }
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();

            return conferences
                .Select(conference => 
                    new
                    {
                        name = conference.Edition, 
                        data = JObjEvents.Where(e => int.Parse(e["ConferenceId"].ToString()) == conference.Id)
                    })
                .Select(JsonConvert.SerializeObject).ToList();
        }
        
        private async Task<List<string>> GetEventsAttendance(int superConferenceId)
        {
            var output = new List<object>();
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();
            var eEvents = await _context.Events
                .Where(e => e.Conference.SuperConferenceId == superConferenceId)
                .ToListAsync();
            
            foreach (var eEvent in eEvents)
            {
                var data = new List<object>{};
                var eventConferences = conferences.Where(c => c.Id == eEvent.ConferenceId);
                foreach (var conference in eventConferences)
                {
                    var userConference = from ea in _context.EventUsers.Where(eu => eu.EventId == eEvent.Id) 
                        join ce in conference.Events on ea.EventId equals ce.Id
                        where ea.EventId == ce.Id select new
                    {
                        ea.UserId, 
                        ce.ConferenceId
                    };
                    
                    var conferenceAttendance = userConference.GroupBy(uc => uc.ConferenceId).ToList().Select(group => new Amount()
                    {
                        Id = @group.Key,
                        Attendance = @group.Count()
                    });
                    var attendance = conferenceAttendance.FirstOrDefault(ca => ca.Id == conference.Id);
                    data.Add(attendance == null
                        ? new {name = conference.Edition, attendance = 0}
                        : new {name = conference.Edition, attendance = attendance.Attendance});
                }
                var tuple = new {name = eEvent.Name, data};
                output.Add(tuple);
            }

            return output.ConvertAll(JsonConvert.SerializeObject);

        }
        
        private async Task<List<string>> GetConferencesTrackAttendance(int superConferenceId)
        {
            var output = new List<string>();
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();
            
            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new Amount()
            {
                Id = @group.Key,
                Attendance = @group.Count()
            });
            
            foreach (var conference in conferences)
            {
                var trackAttendances = new List<TrackAttendance>();
                var conferenceEvents = conference.Events;
                foreach (var eEvent in conferenceEvents)
                {
                    foreach (var track in eEvent.Track.Split(";"))
                    {
                        var attendance = eventAttendance.FirstOrDefault(ea => ea.Id == eEvent.Id);
                        if (attendance == null)
                        {
                            trackAttendances.Add(
                                new TrackAttendance() {Track = track, Attendance = 0});
                            continue;
                        };
                        if (trackAttendances.All(ta => ta.Track != track))
                        {
                            trackAttendances.Add(
                            new TrackAttendance(){Track = track, Attendance = 0}
                            );
                            
                        }

                        trackAttendances.Find(ta => ta.Track == track).Attendance += attendance.Attendance;
                    }
                    
                    
                }
                
                output.Add(JsonConvert.SerializeObject(new
                {
                    name = conference.Edition, 
                    data = trackAttendances
                }));

            }

            return output;
        }
        
        private async Task<List<string>> GetTracksAttendance(int superConferenceId)
        {
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();
            
            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new Amount()
            {
                Id = @group.Key,
                Attendance = @group.Count()
            });

            var trackConferences = new List<TrackConferences>();

            foreach (var conference in conferences)
            {
                var trackAttendances = new List<TrackAttendance>();
                var conferenceEvents = conference.Events;
                foreach (var eEvent in conferenceEvents)
                {
                    foreach (var track in eEvent.Track.Split(";"))
                    {
                        var attendance = eventAttendance.FirstOrDefault(ea => ea.Id == eEvent.Id);
                        if (attendance == null)
                        {
                            trackAttendances.Add(
                                new TrackAttendance() {Track = track, Attendance = 0});
                            continue;
                        };
                        if (trackAttendances.All(ta => ta.Track != track))
                        {
                            trackAttendances.Add(
                                new TrackAttendance(){Track = track, Attendance = 0}
                            );
                            
                        }

                        trackAttendances.Find(ta => ta.Track == track).Attendance += attendance.Attendance;
                    }
                    
                    
                }

                foreach (var trackAttendance in trackAttendances)
                {
                    if (trackConferences.All(tc => tc.Track != trackAttendance.Track))
                    {
                        trackConferences.Add(new TrackConferences()
                        {
                            Track = trackAttendance.Track,
                            Data = new List<TrackConferenceAttendance>()
                            {
                                new TrackConferenceAttendance()
                                {
                                    Conference = conference.Edition,
                                    Attendance = trackAttendance.Attendance
                                }
                            } 
                        });
                    }
                    else
                    {
                        trackConferences.Find(tc => tc.Track == trackAttendance.Track).Data.Add(
                            new TrackConferenceAttendance()
                            {
                                Conference = conference.Edition,
                                Attendance = trackAttendance.Attendance
                            });
                    }
                }
            }

            return trackConferences.ConvertAll(JsonConvert.SerializeObject);
        }
        
        
        
        
        
        
        
        
        

        private class Amount
        {
            public int Id { get; set; }
            public int Attendance { get; set; }
        }

        private class TrackAttendance
        {
            public string Track { get; set; }
            public int Attendance { get; set; }
        }

        private class TrackConferenceAttendance
        {
            public string Conference { get; set; }
            public int Attendance { get; set; }
        }
        
        private class TrackConferences
        {
            public string Track { get; set; }
            public List<TrackConferenceAttendance> Data { get; set; }
        }



    }
}