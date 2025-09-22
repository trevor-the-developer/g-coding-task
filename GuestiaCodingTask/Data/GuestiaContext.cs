using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GuestiaCodingTask.Data
{
    public class GuestiaContext : DbContext
    {
        public GuestiaContext() { }

        public GuestiaContext(DbContextOptions<GuestiaContext> options) : base(options)
        {
        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<GuestGroup> GuestGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(
                        @"Server=localhost,1433;Database=GuestiaDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;")
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();
        }
    }
}