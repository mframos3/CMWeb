using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CMWeb.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        [DataType(DataType.Date)]
        //[Remote(action: "ConflictChecker", controller: "Event", AdditionalFields = "EndDate,EventCenterRoom")]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        //[Remote(action: "ConflictChecker", controller: "Event")]
        public DateTime EndDate { get; set; }
        
        public string Track { get; set; }
        
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
        
        public int EventCenterRoomId { get; set; }
        //[Remote(action: "ConflictChecker", controller: "Event")]
        public EventCenterRoom EventCenterRoom { get; set; }
        public ICollection<EventUser> EventUsers { get; set; }
        
        public ICollection<EventRating> EventRatings { get; set; }
        
        public List<FileDetails> Files { get; set; }
        
    }
}