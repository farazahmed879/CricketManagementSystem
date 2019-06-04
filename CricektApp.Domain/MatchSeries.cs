using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CricketApp.Domain
{
    public class MatchSeries
    {
        public MatchSeries() {
            Matches = new List<Match>();
        }
        public int MatchSeriesId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Organizor { get; set; }
        public string FileName { get; set; }
        public DateTime? StartingDate { get; set; }
        public List<Match> Matches { get; set; }
        public int UserId { get; set; }
        public IdentityUser<int> TenantUser { get; set; }
 
    }
}
