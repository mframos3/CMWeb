using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CMWeb.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Start Date"), DataType(DataType.Date)]
        [Remote(action: "DateChecker", controller: "Event", AdditionalFields = "EndDate,ConferenceId")]
        //[Remote(action: "ConflictChecker", controller: "Event", AdditionalFields = "EndDate,EventCenterRoomId")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Display(Name = "End Date"),DataType(DataType.Date)]
        [Remote(action: "DateChecker", controller: "Event", AdditionalFields = "StartDate,ConferenceId")]
        //[Remote(action: "ConflictChecker", controller: "Event", AdditionalFields = "StartDate,EventCenterRoomId")]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Track { get; set; }
        
        
        public int ConferenceId { get; set; }
        public Conference Conference { get; set; }
        [Remote(action: "ConflictChecker", controller: "Event", AdditionalFields = "StartDate,EndDate")]
        public int EventCenterRoomId { get; set; }

        public EventCenterRoom EventCenterRoom { get; set; }
        public ICollection<EventUser> EventUsers { get; set; }
        
        public ICollection<EventRating> EventRatings { get; set; }
        
        public List<FileDetails> Files { get; set; }
        
    }
}