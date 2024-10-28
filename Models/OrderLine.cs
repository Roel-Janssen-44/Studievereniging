using System.ComponentModel.DataAnnotations;

namespace Studievereniging.Models

{
    public class OrderLine
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Amount { get; set; }
    
        public virtual Product? Product { get; set; }


    }
}