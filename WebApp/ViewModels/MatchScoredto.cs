using CricketApp.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class MatchScoreDto
    {
        public int PlayerScoreId { get; set; }
        public int? PlayerId { get; set; }
        public int Position { get; set; }
        public int MatchId { get; set; }
        public string Bowler { get; set; }
        public int Bat_Runs { get; set; }
        public int Bat_Balls { get; set; }
        public int? HowOutId { get; set; }
        public int Ball_Runs { get; set; }
        public int Four { get; set; }
        public int Six { get; set; }
        public int Overs { get; set; }
        public int Wickets { get; set; }
        public int Stump { get; set; }
        public int Catch { get; set; }
        public int Maiden { get; set; }
        public int RunOut { get; set; }
        public bool IsPlayedInning { get; set; }
        public SelectList PlayerList { get; set; }
        public Match Match { get; set; }
    }
}
