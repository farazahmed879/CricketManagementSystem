namespace CricketApp.Domain
{
    public class MatchSchedule
    {
        public long MatchScheduleId { get; set; }
        public string GroundName { get; set; }
        public string OpponentTeam { get; set; }
        public string Month { get; set; }
        public int? Day { get; set; }
        public int? Year { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
