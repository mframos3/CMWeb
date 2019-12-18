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

        public DbSet<EventUser> EventUsers { get; set; }
        public DbSet<Conference> Conferences { get; set; }

        public DbSet<EventCenter> EventCenters { get; set; }

        public DbSet<EventCenterRoom> EventCenterRooms { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        
        public DbSet<Event> Events { get; set; }
        
        public DbSet<ConferenceRating> ConferenceRatings { get; set; }
        
        public DbSet<EventRating> EventRatings { get; set; }
        
        public DbSet<Menu> Menus { get; set; }
        
        public DbSet<FileDetails> Files { get; set; }

        public DbSet<SuperConference> SuperConferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Conference>().ToTable("Conference");
            modelBuilder.Entity<EventCenter>().ToTable("EventCenter");
            modelBuilder.Entity<Notification>().ToTable("Notification");
            modelBuilder.Entity<EventCenterRoom>().ToTable("EventCenterRoom");
            modelBuilder.Entity<ConferenceRating>().ToTable("ConferenceRating");
            modelBuilder.Entity<EventRating>().ToTable("EventRating");
            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<EventUser>().ToTable("EventUser");
            modelBuilder.Entity<Event>().ToTable("Event")
                .HasDiscriminator<EventType>("EventType")
                .HasValue<Chat>(EventType.Chat)
                .HasValue<Meal>(EventType.Meal)
                .HasValue<Party>(EventType.Party)
                .HasValue<Talk>(EventType.Talk)
                .HasValue<Workshop>(EventType.Workshop);

            modelBuilder.Entity<Event>()
                .HasOne(ev => ev.EventCenterRoom)
                .WithMany(ecr => ecr.Events)
                .HasForeignKey(ev => ev.EventCenterRoomId);

            modelBuilder.Entity<Event>()
                .HasOne(ev => ev.Conference)
                .WithMany(c => c.Events)
                .HasForeignKey(ev => ev.ConferenceId);
            
            modelBuilder.Entity<EventRating>().HasKey(er => new {er.EventId, er.UserId});
            
            modelBuilder.Entity<EventRating>().HasOne(er => er.Event)
                .WithMany(e => e.EventRatings)
                .HasForeignKey(er => er.EventId);
            
            modelBuilder.Entity<EventRating>().HasOne(er => er.User)
                .WithMany(u => u.EventRatings)
                .HasForeignKey(er => er.UserId);
            
            modelBuilder.Entity<MealMenu>()
                .HasKey(t => new { t.MealId, t.MenuId });

            modelBuilder.Entity<MealMenu>()
                .HasOne(mm => mm.Meal)
                .WithMany(m => m.MealMenus)
                .HasForeignKey(mm => mm.MealId);

            modelBuilder.Entity<MealMenu>()
                .HasOne(mm => mm.Menu)
                .WithMany(m => m.MealMenus)
                .HasForeignKey(mm => mm.MenuId);
            
            modelBuilder.Entity<EventUser>().HasKey(eu => new {eu.EventId, eu.UserId});
            
            modelBuilder.Entity<EventUser>().HasOne(eu => eu.Event)
                .WithMany(e => e.EventUsers)
                .HasForeignKey(eu => eu.EventId);
            
            modelBuilder.Entity<EventUser>().HasOne(eu => eu.User)
                .WithMany(u => u.EventUsers)
                .HasForeignKey(eu => eu.UserId);

            modelBuilder.Entity<EventCenterRoom>()
                .HasOne(ecr => ecr.EventCenter)
                .WithMany(ec => ec.EventCenterRooms);
            
            modelBuilder.Entity<Conference>()
                .HasOne(ec => ec.EventCenter)
                .WithMany(c => c.Conferences);

            modelBuilder.Entity<FileDetails>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Files)
                .HasForeignKey(f => f.EventId); 
            
            modelBuilder.Entity<Conference>()
                .HasOne(c => c.SuperConference)
                .WithMany(c => c.Conferences);

        }

    }
}