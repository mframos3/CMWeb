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

        
        public DbSet<Conference> Conferences { get; set; }

        public DbSet<EventCenter> EventCenters { get; set; }

        public DbSet<EventCenterRoom> EventCenterRooms { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        
        public DbSet<Chat> Chats { get; set; }
        
        public DbSet<Meal> Meals { get; set; }
        
        public DbSet<Party> Parties { get; set; }
        
        public DbSet<Talk> Talk { get; set; }
        
        public DbSet<Workshop> Workshops { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Conference>().ToTable("Conference");
            modelBuilder.Entity<EventCenter>().ToTable("EventCenter");
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<EventCenterRoom>().ToTable("EventCenterRoom");
            modelBuilder.Entity<Event>().ToTable("Event")
                .HasDiscriminator<EventType>("EventType")
                .HasValue<Chat>(EventType.Chat)
                .HasValue<Meal>(EventType.Meal)
                .HasValue<Party>(EventType.Party)
                .HasValue<Talk>(EventType.Talk)
                .HasValue<Workshop>(EventType.Workshop);
        }

        
    }
}