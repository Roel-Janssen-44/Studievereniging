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
        public DbSet<Suggestions> Suggestions { get; set; }
        public DbSet<Category> Categories { get; set; }

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

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Lidmaatschap",
                    Price = 5.99,
                    Description = "Victuz lidmaatschap",
                    Image = "/IMG/wordlid.png"
                },
                new Product
                {
                    Id = 2,
                    Name = "T-Shirt",
                    Price = 15.99,
                    Description = "Comfortabel katoenen T-shirt",
                    Image = "/IMG/shirt.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Mok",
                    Price = 9.99,
                    Description = "Koffiemok met logo",
                    Image = "/IMG/mok.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Sticker",
                    Price = 4.95,
                    Description = "Sticker met verenigingslogo",
                    Image = "/IMG/sticker.png"
                }
            );

            modelBuilder.Entity<Activity>().HasData(
                new Activity
                {
                    Id = 1,
                    Name = "Spellen middag",
                    StartDate = DateTime.Now.AddDays(10),
                    EndDate = DateTime.Now.AddDays(12),
                    Location = "B2.104",
                    MaxParticipants = 100,
                    Price = 0,
                    RegistrationDeadline = DateTime.Now.AddDays(8),
                    Category = "Social",
                    IsPublic = true,
                    Image = "/IMG/spellenmiddag.jpg"
                },
                new Activity
                {
                    Id = 2,
                    Name = "Workshop coderen",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(5),
                    Location = "B3.305",
                    MaxParticipants = 50,
                    Price = 5.00,
                    RegistrationDeadline = DateTime.Now.AddDays(2),
                    Category = "Education",
                    IsPublic = true,
                    Image = "/IMG/workshopcoderen.jpg"
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Social" },
                new Category { Id = 2, Name = "Education" },
                new Category { Id = 3, Name = "Sport" },
                new Category { Id = 4, Name = "Culture" }
            );
        }
    }
}

