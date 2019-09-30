using CMWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CMWeb.Models;

namespace CMWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<CMWebUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<CMWeb.Models.Conference> Conference { get; set; }

        public DbSet<CMWeb.Models.EventCenter> EventCenter { get; set; }

        public DbSet<CMWeb.Models.EventCenterRoom> EventCenterRoom { get; set; }

        public DbSet<CMWeb.Models.StatManager> StatManager { get; set; }

        public DbSet<CMWeb.Models.Event> Event { get; set; }

        public DbSet<CMWeb.Models.Notification> Notification { get; set; }
    }
}