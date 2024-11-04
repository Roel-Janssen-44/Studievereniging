namespace Studievereniging.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }
    }
} 