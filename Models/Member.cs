namespace Studievereniging.Models
{
    public class Member : User
    {
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
