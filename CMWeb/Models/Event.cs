using System;
using System.ComponentModel.DataAnnotations;

namespace CMWeb.Models
{
    public abstract class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        public string Track { get; set; }
    }
}