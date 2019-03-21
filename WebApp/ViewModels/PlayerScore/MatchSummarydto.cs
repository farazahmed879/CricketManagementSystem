using CricketApp.Domain;

namespace WebApp.ViewModels
{
    public class MatchSummarydto
    {
        public int PlayerScoreId { get; set; }
        public int? PlayerId { get; set; }
        public int Position { get; set; }
        public int MatchId { get; set; }
        public string Bowler { get; set; }
        public int? Bat_Runs { get; set; }
        public int? Bat_Balls { get; set; }
        public int? HowOutId { get; set; }
        public string HowOutName { get; set; }
        public int? Ball_Runs { get; set; }
        public float? Overs { get; set; }
        public int? Wickets { get; set; }
        public int? Stump { get; set; }
        public int? Catches { get; set; }
        public int? Maiden { get; set; }
        public int? RunOut { get; set; }
        public int? Four { get; set; }
        public int? Six { get; set; }
        public int TeamId { get; set; }
        public bool IsPlayedInning { get; set; }
        public Player Player { get; set; }
        public Match Match { get; set; }
        public HowOut HowOut { get; set; }
        public Team Team { get; set; }
    }
}
