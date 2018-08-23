using Microsoft.AspNetCore.Identity;

namespace CricketApp.Domain
{
    public  class UserRole : IdentityUserRole<int>
    {
        public IdentityUser<int> User { get; set; }
        public IdentityRole<int> Role { get; set; }
    }
}
