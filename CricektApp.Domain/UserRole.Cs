using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricketApp.Domain
{
   public  class UserRole : IdentityUserRole<int>
    {
        public IdentityUser<int> User { get; set; }
        public IdentityRole<int> Role { get; set; }
    }
}
