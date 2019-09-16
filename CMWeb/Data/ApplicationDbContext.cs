using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CMWeb.Models;

namespace CMWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }

        public DbSet<EventCenter> EventCenter { get; set; }

        public DbSet<Conference> Conference { get; set; }

        public DbSet<Room> Room { get; set; }
    }
}
