using Microsoft.AspNetCore.Identity;

namespace CricketApp.Domain
{
    public class ClubAdmin
    {
        public int ClubAdminId { get; set; }
        public Team Team { get; set; }
        public int TeamId { get; set; }
        public IdentityUser<int> User { get; set; }
        public int? UserId { get; set; }

    }
}
