using System;
using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models
{
    public class Suggestions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "De suggestie mag niet langer zijn dan 500 karakters.")]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}