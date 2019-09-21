using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class Matchdto
    {

        public int MatchId { get; set; }
        public int? GroundId { get; set; }
        public string FileName { get; set; }
        [Required]
        public int MatchOvers { get; set; }
        [Required]
        public string Result { get; set; }
        public int? Season { get; set; }
        public int? TournamentId { get; set; }
        public string DateOfMatch { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [Required]
        public int OppponentTeamId { get; set; }
       // public string OppponentTeam { get; set; }
        public float? HomeTeamOvers { get; set; }
        public float? OppTeamOvers { get; set; }
       // public string HomeTeam { get; set; }
       // public string Tournament { get; set; }
      //  public string Series { get; set; }
        [NotMapped]
        [Display(Name = "Match Image")]
        public IFormFile MatchImage { get; set; }
        public int UserId { get; set; }
      //  public string MatchType { get; set; }
        public int MatchTypeId { get; set; }
        public int? MatchSeriesId { get; set; }
        public int? TournamentStageId { get; set; }
        public bool HasFilledHomeTeamData { get; set; }
        public bool HasFilledOpponentTeamData { get; set; }
        public bool HasFilledTeamScoreData { get; set; }
        public int? PlayerOTM { get; set; }
        public string PlayerOfTheMatch { get; set; }

    }
}
