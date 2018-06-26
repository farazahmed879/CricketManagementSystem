
using CricektApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CricketApp.Domain
{
    public class Match
    {
        public Match()
        {
            PlayerScores = new List<PlayerScore>();
            TeamScores = new List<TeamScore>();
        }

        public int MatchId { get; set; }
        [Required]
        public string GroundName { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public int MatchOvers { get; set; }
        [Required]
        public string Result { get; set; }
        public int? Season { get; set; }
        [Required]
        public string Status { get; set; }
        public int? TournamentId { get; set; }
        public DateTime? DateOfMatch { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [Required]
        public int OppponentTeamId { get; set; }
        public Team OppponentTeam { get; set; }
        public Team HomeTeam { get; set; }        
        public Tournament Tournament { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[] MatchLogo { get; set; }
        [NotMapped]
        [Display(Name = "Match Image")]
        public IFormFile MatchImage { get; set; }
        public List<PlayerScore> PlayerScores { get; set; }
        public List<TeamScore> TeamScores { get; set; }

    }
}
