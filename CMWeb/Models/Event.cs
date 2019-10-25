using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMWeb.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        public string Track { get; set; }
        
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
        
        public int EventCenterRoomId { get; set; }
        
        public EventCenterRoom EventCenterRoom { get; set; }
        public ICollection<EventUser> EventUsers { get; set; }
        
        public ICollection<EventRating> EventRatings { get; set; }
        
        
    }
}