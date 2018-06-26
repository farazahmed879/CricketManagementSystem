using CricketApp.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class TeamScoredto
    {
        public int TeamScoreId { get; set; }
        public int TotalScore { get; set; }
        public int Wideballs { get; set; }
        public int NoBalls { get; set; }
        public int Buys { get; set; }
        public int LegBuys { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
    }
}
