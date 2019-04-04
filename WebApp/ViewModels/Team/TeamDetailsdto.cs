using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class TeamDetailsdto
    {
        public TeamDetailsdto()
        {
            TeamPlayers = new List<TeamPlayersdto>();
        }


        public int TeamId { get; set; }
        public byte[] TeamLogo { get; set; }
        [NotMapped]
        [Display(Name = "Team Image")]
        public IFormFile TeamImage { get; set; }
        public string Team_Name { get; set; }
        public string FileName { get; set; }
        public string City { get; set; }
        public string Place { get; set; }
        public string Zone { get; set; }
        public List<TeamPlayersdto> TeamPlayers { get; set; }
    }
}
