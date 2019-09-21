﻿using CricketApp.Domain;

namespace WebApp.ViewModels
{
    public class TeamScoredto
    {
        public int TeamScoreId { get; set; }
        public int TotalScore { get; set; }
        public int Wickets { get; set; }
        public int Wideballs { get; set; }
        public int NoBalls { get; set; }
        public int Byes { get; set; }
        public int LegByes { get; set; }
        public int TeamId { get; set; }
       // public Team Team { get; set; }
        public string TeamName { get; set; }
        public int MatchId { get; set; }
        //public Match Match { get; set; }
    }
}
