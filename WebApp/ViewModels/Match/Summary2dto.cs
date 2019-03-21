using System;

namespace WebApp.ViewModels
{
    public class Summary2dto
    {
        public string HomeTeam { get; set; }
        public string OppponentTeam { get; set; }
        public int HomeTeamScore { get; set; }
        public int OpponentsTeamScore { get; set; }
        public string Result { get; set; }
        public string Stage { get; set; }
        public string TournamentName { get; set; }
        public string Type { get; set; }
        public string Place { get; set; }
        public float HomeTeamOvers { get; set; }
        public float OppTeamOvers { get; set; }
        public DateTime DateOfMatch { get; set; }
        public string GroundName { get; set; }
        public string ManOfTheMatch { get; set; }
        public int HomeTeamWickets { get; set; }
        public int OpponentTeamWickets { get; set; }
        public byte[] OpponentTeamLogo { get; set; }
        public byte[] HomeTeamTeamLogo { get; set; }

    }
}
