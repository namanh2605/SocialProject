using System.ComponentModel.DataAnnotations;

namespace SocialProject.ViewModals.Authentication
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
