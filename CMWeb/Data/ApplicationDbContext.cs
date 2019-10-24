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
        
        public DbSet<Event> Events { get; set; }
        
        public DbSet<ConferenceRating> ConferenceRatings { get; set; }
        
        public DbSet<EventRating> EventRatings { get; set; }
        
        public DbSet<Menu> Menus { get; set; }
        
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

            modelBuilder.Entity<Meal>()
                .HasOne(meal => meal.Menu)
                .WithMany(menu => menu.Meals)
                .HasForeignKey(meal => meal.MenuId);
            
            modelBuilder.Entity<EventUser>().HasKey(eu => new {eu.EventId, eu.UserId});
            
            modelBuilder.Entity<EventUser>().HasOne(eu => eu.Event)
                .WithMany(e => e.EventUsers)
                .HasForeignKey(eu => eu.EventId);
            
            modelBuilder.Entity<EventUser>().HasOne(eu => eu.User)
                .WithMany(u => u.EventUsers)
                .HasForeignKey(eu => eu.UserId);
            
            
        }

        
    }
}