using CricketApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CricketApp.Domain
{
    public class TeamScore
    {


        public int TeamScoreId
        {
            get; set;
        }
        public int TotalScore { get; set; }
        public int Wideballs { get; set; }
        public int NoBalls { get; set; }
        public int Byes { get; set; }
        public int LegByes { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }

    }
}
