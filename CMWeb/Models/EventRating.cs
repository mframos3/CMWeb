using System.ComponentModel.DataAnnotations.Schema;
using CMWeb.Areas.Identity.Data;

namespace CMWeb.Models
{
    public class EventRating
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        
        public string UserId { get; set; }
        public CMWebUser User { get; set; }
        
        public int EventId { get; set; }
        public Event Event { get; set; }

        [NotMapped] public int SpeakerRating { get; set;}
    }
}