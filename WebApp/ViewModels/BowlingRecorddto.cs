using CricketApp.Domain;
using System;

namespace WebApp.ViewModels
{
    public class BowlingRecorddto
    {
        public int playerId { get; set;}
        public string PlayerName { get; set; }      
        public int TeamId { get; set; }      
        public int TotalMatch { get; set; }     
        public int TotalOvers { get; set; }
        public int TotalBallRuns { get; set; }
        public int TotalWickets { get; set; }
        public int TotalMaidens { get; set; }
        public string BowlingAvg { get; set; }
        public string Economy { get; set; }
        public int FiveWickets { get; set; }
        public int TotalCatches { get; set; }
        public int TotalRunOuts { get; set; }
        public int TotalStumps { get; set; }
    }
}
