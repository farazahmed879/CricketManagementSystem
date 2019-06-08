using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CricketApp.Domain
{
    public class ApplicationUserRole : IdentityRole<int>
    {
        public List<ApplicationUser> User { get; set; }
    }
}
