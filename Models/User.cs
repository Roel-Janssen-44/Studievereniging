using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; } = Models.Role.Guest;
        
        public List<Activity> OrganiserActivities { get; set; } = new List<Activity>();
        public List<Activity> ParticipantActivities { get; set; } = new List<Activity>();
        public ICollection<Activity>? Activities { get; } = new List<Activity>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
