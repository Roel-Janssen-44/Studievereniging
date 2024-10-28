using Microsoft.AspNetCore.Mvc.Rendering;

namespace Studievereniging.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Category { get; set; }
        public double? Price { get; set; }
        public int? MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public string? AdminName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFullyBooked => MaxParticipants.HasValue && CurrentParticipants >= MaxParticipants.Value;
    }
}
