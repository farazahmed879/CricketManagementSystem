using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CricketApp.Domain
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ApplicationUserRoleId { get; set; }
        public ApplicationUserRole Role { get; set; }
        public Team Team { get; set; }
    }
}
