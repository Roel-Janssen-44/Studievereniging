using Microsoft.AspNetCore.Identity;

namespace Studievereniging.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add Name property since we're using it in controllers
        public string Name 
        { 
            get => UserName; 
            set => UserName = value; 
        }

        // Add Role property that works with Identity roles
        public string Role { get; set; } = Models.Role.Guest;

        // Existing properties
        public List<Activity> OrganiserActivities { get; set; } = new List<Activity>();
        public List<Activity> ParticipantActivities { get; set; } = new List<Activity>();
        public ICollection<Activity>? Activities { get; } = new List<Activity>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 