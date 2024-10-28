using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Studievereniging.Models.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Display(Name = "Maximum Participants")]
        public int? MaxParticipants { get; set; }
        
        public double? Price { get; set; }
        
        [Display(Name = "Registration Deadline")]
        public DateTime? RegistrationDeadline { get; set; }
        
        public string? Category { get; set; }
        
        public string? Image { get; set; }
        
        [Display(Name = "Is Public")]
        public bool IsPublic { get; set; } = true;  // Changed from bool? to bool with default value

        // For organizer selection
        [Display(Name = "Organizers")]
        public List<int> SelectedOrganizerIds { get; set; } = new List<int>();
        
        // For displaying available organizers in dropdown
        public List<UserSelectViewModel> AvailableOrganizers { get; set; } = new List<UserSelectViewModel>();
        
        // For displaying current organizers
        public List<UserSelectViewModel> CurrentOrganizers { get; set; } = new List<UserSelectViewModel>();
        
        // For displaying participants
        public List<UserSelectViewModel> CurrentParticipants { get; set; } = new List<UserSelectViewModel>();
    }

    public class UserSelectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
