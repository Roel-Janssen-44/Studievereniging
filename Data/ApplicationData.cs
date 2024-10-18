using Microsoft.EntityFrameworkCore;
using Studievereniging.Models;
using System.Data;

namespace Studievereniging.Data
{
    public class ApplicationData : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Activity> Activities { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=Studievereniging;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
     
            modelBuilder.Entity<Activity>()
            .HasMany(a => a.Organisers)
            .WithMany(u => u.OrganiserActivities)
            .UsingEntity(j => j.ToTable("ActivityOrganisers"));

            modelBuilder.Entity<Activity>()
                .HasMany(a => a.Participants)
                .WithMany(u => u.ParticipantActivities)
                .UsingEntity(j => j.ToTable("ActivityParticipants"));

            modelBuilder.Entity<Activity>()
                .HasOne(u => u.Admin)
                .WithMany(a => a.Activities)
                .OnDelete(DeleteBehavior.SetNull);




        }


    }
}





