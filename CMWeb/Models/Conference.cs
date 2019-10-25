using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMWeb.Models
{
    public class Conference
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        public ICollection<Sponsor> Sponsors { get; set; }
        
        public ICollection<Event> Events { get; set; }

    }
}