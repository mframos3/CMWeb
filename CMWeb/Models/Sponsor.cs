namespace CMWeb.Models
{
    public class Sponsor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        // TODO: Tabla intermedia many-to-many con Conference
    }
}