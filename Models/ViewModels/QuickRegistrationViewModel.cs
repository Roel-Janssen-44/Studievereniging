namespace Studievereniging.Models.ViewModels
{
    public class QuickRegistrationViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ActivityId { get; set; }
        public string GeneratedPassword { get; set; } = string.Empty;
    }
} 