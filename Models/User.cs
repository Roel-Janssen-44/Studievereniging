using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        // Add Role property that uses the static values from Role class
        public required string Role { get; set; } = Models.Role.Guest; // Default to Guest role
        
        public List<Activity> OrganiserActivities { get; set; } = new List<Activity>();
        public List<Activity> ParticipantActivities { get; set; } = new List<Activity>();

        public ICollection<Activity>? Activities { get; } = new List<Activity>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
