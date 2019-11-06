using System.Collections.Generic;
namespace CMWeb.Models
{
    public class EventCenterRoom
    {
        public int Id { get; set; }
        public string Capacity { get; set; }
        public string Location { get; set; }
        public List<string> Equipment { get; set; }
        
        public ICollection<Event> Events { get; set; }
    }
}