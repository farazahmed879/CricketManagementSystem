using CricketApp.Domain;
using System;

namespace WebApp.ViewModels
{
    public class PlayerStatisticsdto
    {
        public int playerId { get; set;}
        public string PlayerName { get; set; }
        public string Role { get; set; }
        public string BattingStyle { get; set; }
        public string BowlingStyle { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public DateTime DOB { get; set; }
        public int TotalMatch { get; set; }
        public int TotalInnings { get; set; }
        public int TotalBatRuns { get; set; }
        public int TotalBatBalls { get; set; }
        public int TotalFours { get; set; }
        public int TotalSixes { get; set; }
        public int NumberOf50s { get; set; }
        public int NumberOf100s { get; set; }
        public float StrikeRate { get; set; }
        public string BattingAverage { get; set; }
        public int TotalOvers { get; set; }
        public int TotalBallRuns { get; set; }
        public int TotalWickets { get; set; }
        public int TotalMaidens { get; set; }
        public string BowlingAvg { get; set; }
        public float Economy { get; set; }
        public int FiveWickets { get; set; }
        public int TotalCatches { get; set; }
        public int TotalRunOuts { get; set; }
        public int TotalStumps { get; set; }
        public int TotalNotOut { get; set; }
        public string TotalBowled { get; set; }
        public float TotalCatch { get; set; }
        public int TotalStump { get; set; }
        public int TotalRunOut { get; set; }
        public int TotalHitWicket { get; set; }
        public int TotalLBW { get; set; }
        public byte[] PlayerImage { get; set; }
        public string BestBowling { get; set; }
        public int BestScore { get; set; }
        public int MostWickets { get; set; }
        public Team Team { get; set; }
    }
}
