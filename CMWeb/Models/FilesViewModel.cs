using System.Collections.Generic;

namespace CMWeb.Models
{
    public class FileDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        
        public int EventId { get; set; }
        public Event Event { get; set; }
    }

    public class FilesViewModel
    {
        public List<FileDetails> Files { get; set; } = new List<FileDetails>();
    }
}