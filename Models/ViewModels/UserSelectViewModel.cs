namespace Studievereniging.Models.ViewModels
{
    public class UserSelectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
} 