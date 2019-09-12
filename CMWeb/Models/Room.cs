using System;
using System.Collections.Generic;
namespace CMWeb.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Capacity { get; set; }
        public string Location { get; set; }
        public List<String> Equipment { get; set; }
    }
}
