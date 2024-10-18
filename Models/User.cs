using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models
{

    public abstract class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Activity> OrganiserActivities { get; set; } = new List<Activity>();
        public List<Activity> ParticipantActivities { get; set; } = new List<Activity>();

        
    }
}
    