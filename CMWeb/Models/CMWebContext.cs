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
    }
}
