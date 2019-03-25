namespace WebApp.ViewModels
{
    public class HomeScreendto
    {
        public int Tournaments { get; set; }
        public int Players { get; set; }
        public int Teams { get; set; }
        public int Matches { get; set; }
        public string DateOfMatch { get; set; }
        public string UserName { get; set; }
        public int Series { get; set; }
        public int Records { get; set; }
        public string Summary { get; set; }
        public string HomeTeam { get; set; }
        public string OppponentTeam { get; set; }
        public int HomeTeamScore { get; set; }
        public int OpponentsTeamScore { get; set; }
        public int HomeTeamWickets { get; set; }
        public int OppenentTeamWickets { get; set; }
        public byte[] OpponentTeamLogo { get; set; }
        public byte[] HomeTeamTeamLogo { get; set; }
    }
}
