using System.Security.Permissions;
using CMWeb.Areas.Identity.Data;

namespace CMWeb.Models
{
    public class ConferenceRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        

        public int ConferenceId { get; set;}

        public int UserId { get; set; }

    }
}