using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
} 