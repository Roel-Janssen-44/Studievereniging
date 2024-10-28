using Microsoft.AspNetCore.Mvc.Rendering;

namespace Studievereniging.ViewModels
{
    public class ActivityFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int? MaxParticipants { get; set; }
        public double? Price { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public string? Category { get; set; }
        public string? Image { get; set; }
        public bool? IsPublic { get; set; }
        public int? AdminId { get; set; }
        public List<int> OrganiserIds { get; set; } = new List<int>();
        
        // For dropdown lists in the view
        public List<SelectListItem> AvailableAdmins { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableOrganisers { get; set; } = new List<SelectListItem>();
    }
}
