using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CMWeb.Models
{
    public class Conference
    {
        public int Id { get; set; }

        public string Edition { get; set; }
        public string Description { get; set; }

        [DataType(DataType.DateTime)] 
        [Remote(action: "DateChecker", controller: "Conference", AdditionalFields = "EndDate")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)] 
        [Remote(action: "DateChecker", controller: "Conference", AdditionalFields = "StartDate")]
        public DateTime EndDate { get; set; }

        public ICollection<Event> Events { get; set; }

        public int SuperConferenceId { get; set; }

        public SuperConference SuperConference { get; set; }

        public EventCenter EventCenter { get; set; }

        public int EventCenterId { get; set; }

        public string Sponsor { get; set; }

    }
}