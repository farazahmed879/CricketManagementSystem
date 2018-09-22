using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool isPersistent { get; set; }
    }
}