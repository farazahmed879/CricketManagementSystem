using CricketApp.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CricketApp.Domain
{
    public class Tournament
    {
        public Tournament() {
            Matches = new List<Match>();
        }
        public int TournamentId { get; set; }
        [Required]
        public string TournamentName { get; set; }
        public string Organizor { get; set; }
        public DateTime StartingDate { get; set; }
        public List<Match> Matches { get; set; }
        public int UserId { get; set; }
        public IdentityUser<int> TenantUser { get; set; }
 
    }
}
