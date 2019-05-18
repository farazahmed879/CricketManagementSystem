using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class MatchDetails
    {
        public MatchDetails()
        {
            matchBatsman = new List<MatchBatsman>();
            matchBowler = new List<MatchBowler>();
        }
        public string HomeTeam { get; set; }
        public string HomeTeamLogo { get; set; }
        public string OppTeam { get; set; }
        public string OppTeamLogo { get; set; }
        public string Ground { get; set; }
        public string Tournament { get; set; }
        public string Stage { get; set; }
        public string Type { get; set; }
        public List<MatchBatsman> matchBatsman { get; set; }
        public List<MatchBowler> matchBowler { get; set; }

    }
}
