using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMWeb.Models
{
    public class EventCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public ICollection<EventCenterRoom> EventCenterRooms { get; set; }
        public string Description { get; set; }

        public ICollection<Conference> Conferences { get; set; }

    }
}