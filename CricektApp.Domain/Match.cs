using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CricketApp.Domain
{
    public class Match
    {
        public Match()
        {
            PlayerScores = new List<PlayerScore>();
            TeamScores = new List<TeamScore>();
            FallOfWickets = new List<FallOfWicket>();
        }

        public int MatchId { get; set; }

        public string GroundName { get; set; }

        public string Place { get; set; }
        [Required]
        public int MatchOvers { get; set; }
        [Required]
        public string Result { get; set; }
        public string MatchDescription { get; set; }
        public int? Season { get; set; }
        public int? TournamentId { get; set; }
        public int? TossWinningTeam { get; set; }
        public DateTime? DateOfMatch { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [Required]
        public int OppponentTeamId { get; set; }
        public float? HomeTeamOvers { get; set; }
        public float? OppTeamOvers { get; set; }    
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
        public List<FallOfWicket> FallOfWickets { get; set; }
        public int UserId { get; set; }
        public MatchType MatchType { get; set; }
        public int MatchTypeId { get; set; }
        public TournamentStage TournamentStage { get; set; }
        public int? TournamentStageId { get; set; }
        public MatchSeries MatchSeries { get; set; }
        public int? MatchSeriesId { get; set; }
        public int? PlayerOTM { get; set; }
        public Player Player { get; set; }

    }
}
