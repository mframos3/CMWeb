using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Constraints;

namespace CMWeb.Models
{
    public class SuperConference
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
        
        public float Rating { get; set; }

        public ICollection<Conference> Conferences { get; set; }
    }
}