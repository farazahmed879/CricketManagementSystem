using Microsoft.AspNetCore.Identity;

namespace CricketApp.Domain
{
    public  class UserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; }
        public ApplicationUserRole Role { get; set; }
    }
}
