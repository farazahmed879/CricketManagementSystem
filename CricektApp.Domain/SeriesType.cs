using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CricketApp.Domain
{
    public class SeriesType
    {
        public SeriesType() {
            Matches = new List<Match>();
        }
        public int SeriesTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Organizor { get; set; }
        public DateTime? StartingDate { get; set; }
        public List<Match> Matches { get; set; }
        public int UserId { get; set; }
        public IdentityUser<int> TenantUser { get; set; }
 
    }
}
