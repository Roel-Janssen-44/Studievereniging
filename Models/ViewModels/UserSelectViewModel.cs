namespace Studievereniging.Models.ViewModels
{
    public class UserSelectViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
} 