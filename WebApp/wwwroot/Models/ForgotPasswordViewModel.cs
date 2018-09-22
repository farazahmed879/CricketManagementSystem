using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}