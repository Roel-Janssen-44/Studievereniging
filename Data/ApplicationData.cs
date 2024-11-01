using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Models;
using Microsoft.AspNetCore.Identity;

namespace Studievereniging.Data
{
    public class ApplicationData : IdentityDbContext<ApplicationUser>
    {
        public ApplicationData(DbContextOptions<ApplicationData> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This is crucial for Identity tables

            // Configure Activity-ApplicationUser relationships
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

            // Configure Order-ApplicationUser relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.user)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Customize Identity table names (optional)
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}






