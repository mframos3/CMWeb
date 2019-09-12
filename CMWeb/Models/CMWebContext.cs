using Microsoft.EntityFrameworkCore;


namespace CMWeb.Models
{
    public class CMWebContext : DbContext
    {
        public CMWebContext(DbContextOptions<CMWebContext> options)
            : base(options) 
        {
        }

        public DbSet<EventCenter> EventCenter { get; set; }

        public DbSet<Conference> Conference { get; set; }

        public DbSet<Room> Room { get; set; }
    }
}
