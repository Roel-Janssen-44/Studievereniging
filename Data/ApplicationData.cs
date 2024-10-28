using Microsoft.EntityFrameworkCore;
using Studievereniging.Models;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Studievereniging.Data
{
    public class ApplicationData : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Activity relationships
            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Organisers)
                .WithMany(u => u.OrganiserActivities)
                .UsingEntity(j => j.ToTable("ActivityOrganisers"));

            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Participants)
                .WithMany(u => u.ParticipantActivities)
                .UsingEntity(j => j.ToTable("ActivityParticipants"));

            // Configure Activity-Admin relationship
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Admin)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Order-User relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.user)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}






