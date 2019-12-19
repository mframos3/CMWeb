using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMWeb.Models
{
    public class Conference
    {
        public int Id { get; set; }

        public string Edition { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [DataType(DataType.Date)] public DateTime EndDate { get; set; }

        public ICollection<Event> Events { get; set; }

        public float Rating { get; set; }

        public int SuperConferenceId { get; set; }

        public SuperConference SuperConference { get; set; }

        public EventCenter EventCenter { get; set; }

        public int EventCenterId { get; set; }

        public string Sponsor { get; set; }

    }
}