using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace CricketApp.Domain
{
    public class Login : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string contact { get; set; }
    }
}
