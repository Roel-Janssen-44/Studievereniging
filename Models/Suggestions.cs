using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models
{
    public class Suggestions
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "De suggestie mag niet langer zijn dan 500 karakters.")]
        public string Text { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "De naam mag niet langer zijn dan 100 karakters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; } 
    }
}