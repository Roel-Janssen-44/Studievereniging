namespace Studievereniging.Models
{
    public class Guest : User
    {
        public virtual ICollection<Order> Orders { get; set; }
    }
}
