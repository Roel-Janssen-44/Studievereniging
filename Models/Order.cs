using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Studievereniging.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public ICollection<OrderLine>? OrderLines { get; set; }

        [AllowNull]
        public string? CustomerId { get; set; }

        [AllowNull]
        public virtual ApplicationUser? user { get; set; }

    }
}