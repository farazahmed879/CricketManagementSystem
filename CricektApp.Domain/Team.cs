using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CricketApp.Domain
{
    public class Team
    {
        public Team()
        {
            Players = new List<Player>();
            OpponentTeamMatches = new List<Match>();
            HomeTeamMatches = new List<Match>();
        }

        public int TeamId { get; set; }
        [Required]
        public string Team_Name { get; set; }
        public string Place { get; set; }
        public string Zone { get; set; }
        [Required]
        public string City { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[] TeamLogo { get; set; }
        [NotMapped]
        [Display(Name = "Team Image")]
        public IFormFile TeamImage { get; set; }
        public List<Player> Players { get; set; }
        public List<Match> OpponentTeamMatches { get; set; }
        public List<Match> HomeTeamMatches { get; set; }

    }
}
