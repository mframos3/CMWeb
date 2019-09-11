using System;
using System.ComponentModel.DataAnnotations;


namespace CMWeb.Models
{
    public class EventCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }

    }
}
