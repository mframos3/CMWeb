using CMWeb.Areas.Identity.Data;

namespace CMWeb.Models
{
    public class EventUser
    {
        
        public string UserId { get; set; }
        public CMWebUser User { get; set; }
        
        public int EventId { get; set; }
        public Event Event { get; set; }
        
        public UserType Type { get; set; }
    }
}