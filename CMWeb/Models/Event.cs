using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMWeb.Areas.Identity.Data;

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
        
        public ICollection<EventUser> EventUsers { get; set; }
    }
}