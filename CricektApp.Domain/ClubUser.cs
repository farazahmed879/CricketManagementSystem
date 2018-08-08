using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CricketApp.Domain
{
    public class ClubUser
    {
        public int ClubUserId { get; set; }
        public Team Team { get; set; }
        public int TeamId { get; set; }
        public IdentityUser<int> User { get; set; }
        public int? UserId { get; set; }

    }
}
