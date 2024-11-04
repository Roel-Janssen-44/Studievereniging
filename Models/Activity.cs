using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Studievereniging.Models
{
    public class Activity
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        public int? MaxParticipants { get; set; }
        public double? Price { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public string? Category { get; set; }
        public DateTime? Calendar { get; set; }
        public string? Image { get; set; }
        public bool IsPublic { get; set; } = true; 

        [AllowNull]
        public string? AdminId { get; set; }
        
        [AllowNull]
        public ApplicationUser? Admin { get; set; }

        public List<ApplicationUser> Organisers { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> Participants { get; set; } = new List<ApplicationUser>();
    }
}
