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
            ViewData["ConferencesEventsRatings"] = await GetConferencesEventsRatings(id);
            ViewData["EventsRatings"] = await GetEventsRating(id);
            ViewData["ConferencesTrackRating"] = await GetConferencesTrackRating(id);
            ViewData["TracksRating"] = await GetTracksRating(id);
            ViewData["SpeakersRating"] = await GetSpeakersRating(id);
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

            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new IdAttendance()
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
                    
                    var conferenceAttendance = userConference.GroupBy(uc => uc.ConferenceId).ToList().Select(group => new IdAttendance()
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
            
            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new IdAttendance()
            {
                Id = @group.Key,
                Attendance = @group.Count()
            });
            
            foreach (var conference in conferences)
            {
                var trackAttendances = new List<NameAttendance>();
                var conferenceEvents = conference.Events;
                foreach (var eEvent in conferenceEvents)
                {
                    foreach (var track in eEvent.Track.Split(";"))
                    {
                        var attendance = eventAttendance.FirstOrDefault(ea => ea.Id == eEvent.Id);
                        if (attendance == null)
                        {
                            trackAttendances.Add(
                                new NameAttendance() {Name = track, Attendance = 0});
                            continue;
                        };
                        if (trackAttendances.All(ta => ta.Name != track))
                        {
                            trackAttendances.Add(
                            new NameAttendance(){Name = track, Attendance = 0}
                            );
                            
                        }

                        trackAttendances.Find(ta => ta.Name == track).Attendance += attendance.Attendance;
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
            
            var eventAttendance = _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new IdAttendance()
            {
                Id = @group.Key,
                Attendance = @group.Count()
            });

            var trackConferences = new List<AttendanceData>();

            foreach (var conference in conferences)
            {
                var trackAttendances = new List<NameAttendance>();
                var conferenceEvents = conference.Events;
                foreach (var eEvent in conferenceEvents)
                {
                    foreach (var track in eEvent.Track.Split(";"))
                    {
                        var attendance = eventAttendance.FirstOrDefault(ea => ea.Id == eEvent.Id);
                        if (attendance == null)
                        {
                            trackAttendances.Add(
                                new NameAttendance() {Name = track, Attendance = 0});
                            continue;
                        };
                        if (trackAttendances.All(ta => ta.Name != track))
                        {
                            trackAttendances.Add(
                                new NameAttendance(){Name = track, Attendance = 0}
                            );
                            
                        }

                        trackAttendances.Find(ta => ta.Name == track).Attendance += attendance.Attendance;
                    }
                    
                    
                }

                foreach (var trackAttendance in trackAttendances)
                {
                    if (trackConferences.All(tc => tc.Name != trackAttendance.Name))
                    {
                        trackConferences.Add(new AttendanceData()
                        {
                            Name = trackAttendance.Name,
                            Data = new List<NameAttendance>()
                            {
                                new NameAttendance()
                                {
                                    Name = conference.Edition,
                                    Attendance = trackAttendance.Attendance
                                }
                            } 
                        });
                    }
                    else
                    {
                        trackConferences.Find(tc => tc.Name == trackAttendance.Name).Data.Add(
                            new NameAttendance()
                            {
                                Name = conference.Edition,
                                Attendance = trackAttendance.Attendance
                            });
                    }
                }
            }

            return trackConferences.ConvertAll(JsonConvert.SerializeObject);
        }
        
         private async Task<List<string>> GetConferencesEventsRatings(int superConferenceId)
         {
             var output = new List<RatingData>();
             var conferences = await _context.Conferences.Where(c => c.SuperConferenceId == superConferenceId).ToListAsync();
             var eventRatings =  _context.EventUsers.GroupBy(eu => eu.EventId).ToList().Select(group => new IdRating()
             {
                 Id = @group.Key,
                 Rating = @group.Sum(i => i.Rating) / @group.Count()
             }).ToList();
             foreach (var conference in conferences)
             {
                 var conferenceData = new RatingData() {Name = conference.Edition, Data = new List<NameRating>()};
                 foreach (var eEvent in conference.Events.ToList())
                 {
                     var rating = eventRatings.FirstOrDefault(er => er.Id == eEvent.Id);
                     conferenceData.Data.Add(rating == null
                         ? new NameRating() {Name = eEvent.Name, Rating = 0}
                         : new NameRating() {Name = eEvent.Name, Rating = rating.Rating});
                 }
             }

             return output.ConvertAll(JsonConvert.SerializeObject);

         }
        
        private async Task<List<string>> GetEventsRating(int superConferenceId)
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
                    var userConference = from ea in _context.EventRatings.Where(eu => eu.EventId == eEvent.Id) 
                        join ce in conference.Events on ea.EventId equals ce.Id
                        where ea.EventId == ce.Id select new
                    {
                        ea.UserId, 
                        ea.Rating,
                        ce.ConferenceId
                        
                    };
                    
                    var conferenceRating = userConference.GroupBy(uc => uc.ConferenceId).ToList().Select(group => new IdRating()
                    {
                        Id = @group.Key,
                        Rating = @group.Sum(i => i.Rating) / @group.Count()
                    });
                    var rating = conferenceRating.FirstOrDefault(ca => ca.Id == conference.Id);
                    data.Add(rating == null
                        ? new NameRating() {Name = conference.Edition, Rating = 0}
                        : new NameRating() {Name = conference.Edition, Rating = rating.Rating});
                }
                var tuple = new {name = eEvent.Name, data};
                output.Add(tuple);
            }

            return output.ConvertAll(JsonConvert.SerializeObject);

        }
        
        private async Task<List<string>> GetConferencesTrackRating(int superConferenceId)
        {
            var output = new List<string>();
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();
            
            var eventRating = _context.EventRatings.GroupBy(eu => eu.EventId).ToList().Select(group => new IdRating()
            {
                Id = @group.Key,
                Rating = @group.Sum(i => i.Rating) / @group.Count()
            });
            
            foreach (var conference in conferences)
            {
                var trackRatings = new List<NameRating>();
                var conferenceEvents = conference.Events;
                foreach (var eEvent in conferenceEvents)
                {
                    foreach (var track in eEvent.Track.Split(";"))
                    {
                        var rating = eventRating.FirstOrDefault(ea => ea.Id == eEvent.Id);
                        if (rating == null)
                        {
                            trackRatings.Add(
                                new NameRating() {Name = track, Rating = 0});
                            continue;
                        };
                        if (trackRatings.All(ta => ta.Name != track))
                        {
                            trackRatings.Add(
                            new NameRating(){Name = track, Rating = 0}
                            );
                            
                        }
                        var trackAttendance = trackRatings.Find(ta => ta.Name == track);
                        trackAttendance.Rating += ((rating.Rating - trackAttendance.Rating) / 
                                                        trackAttendance.Count + 1) ;
                        trackAttendance.Count ++;
                    }
                    
                    
                }
                
                output.Add(JsonConvert.SerializeObject(new
                {
                    name = conference.Edition, 
                    data = trackRatings
                }));

            }

            return output;
        }
        
        private async Task<List<string>> GetTracksRating(int superConferenceId)
        {
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();
            
            var eventRating = _context.EventRatings.GroupBy(eu => eu.EventId).ToList().Select(group => new IdRating()
            {
                Id = @group.Key,
                Rating = @group.Sum(i => i.Rating) / @group.Count()
            });

            var trackConferences = new List<RatingData>();

            foreach (var conference in conferences)
            {
                var trackRatings = new List<NameRating>();
                var conferenceEvents = conference.Events;
                foreach (var eEvent in conferenceEvents)
                {
                    foreach (var track in eEvent.Track.Split(";"))
                    {
                        var rating = eventRating.FirstOrDefault(ea => ea.Id == eEvent.Id);
                        if (rating == null)
                        {
                            trackRatings.Add(
                                new NameRating() {Name = track, Rating = 0});
                            continue;
                        };
                        if (trackRatings.All(ta => ta.Name != track))
                        {
                            trackRatings.Add(
                                new NameRating() {Name = track, Rating = 0}
                            );
                            
                        }
                        var trackAttendance = trackRatings.Find(ta => ta.Name == track);
                        trackAttendance.Rating += ((rating.Rating - trackAttendance.Rating) / 
                                                        trackAttendance.Count + 1) ;
                        trackAttendance.Count ++;
                    }
                    
                    
                }

                foreach (var trackRating in trackRatings)
                {
                    if (trackConferences.All(tc => tc.Name != trackRating.Name))
                    {
                        trackConferences.Add(new RatingData()
                        {
                            Name = trackRating.Name,
                            Data = new List<NameRating>()
                            {
                                new NameRating()
                                {
                                    Name = conference.Edition,
                                    Rating = trackRating.Rating
                                }
                            } 
                        });
                    }
                    else
                    {
                        trackConferences.Find(tc => tc.Name == trackRating.Name).Data.Add(
                            new NameRating()
                            {
                                Name = conference.Edition,
                                Rating = trackRating.Rating
                            });
                    }
                }
            }

            return trackConferences.ConvertAll(JsonConvert.SerializeObject);
        }

        private async Task<List<string>> GetSpeakersRating(int superConferenceId)
        {
            var superConference = await _context.SuperConferences
                .Include(c => c.Conferences)
                .FirstOrDefaultAsync(m => m.Id == superConferenceId);
            var conferences = superConference.Conferences.ToList();
            var users = _context.Users.ToList();
            var output = new List<RatingData>();
            foreach (var conference in conferences)
            {
                var speakersRating = new RatingData() {Name = conference.Edition, Data = new List<NameRating>()};
                var eEvents = conference.Events.ToList();
                foreach (var eEvent in eEvents)
                {
                    var speakers = _context.EventUsers.Where(eu => eu.Type == UserType.Speaker).Where(eu => eu.EventId == eEvent.Id).ToList();
                    if (!speakers.Any())
                    {
                        continue;
                    }
                    foreach (var speaker in speakers)
                    {
                        if(speakersRating.Data.Any(d => d.Name == users.Find(u => u.Id == speaker.UserId).Name))
                        {
                            var repeatedspeaker = speakersRating.Data.Find(sr =>
                                sr.Name == users.Find(u => u.Id == speaker.UserId).Name);
                                repeatedspeaker.Rating += ((speaker.Rating - repeatedspeaker.Rating) / 
                                                          repeatedspeaker.Count + 1) ;
                                repeatedspeaker.Count ++;
                        }
                        else
                        {
                            speakersRating.Data.Add(new NameRating()
                            {
                                Name = users.Find(u => u.Id == speaker.UserId).Name,
                                Rating = 0
                            });
                        }
                    }
                }
                output.Add(speakersRating);
            }

            return output.ConvertAll(JsonConvert.SerializeObject);
        }
        
        
        private class IdAttendance
        {
            public int Id { get; set; }
            public int Attendance { get; set; }
        }
        
        private class IdRating
        {
            public int Id { get; set; }
            public float Rating { get; set; }
        }

        private class NameAttendance
        {
            public string Name { get; set; }
            public int Attendance { get; set; }
        }
        
        private class NameRating
        {
            public string Name { get; set; }
            public float Rating { get; set; }
            public int Count { get; set; }
        }

  
        private class AttendanceData
        {
            public string Name { get; set; }
            public List<NameAttendance> Data { get; set; }
        }
        
        private class RatingData
        {
            public string Name { get; set; }
            public List<NameRating> Data { get; set; }
        }
        



    }
}