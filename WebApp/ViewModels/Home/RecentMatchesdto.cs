using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.Home
{
    public class RecentMatchesdto
    {

        public DateTime DateOfMatch { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string Summary { get; set; }
        public string HomeTeam { get; set; }
        public string OppponentTeam { get; set; }
        public int HomeTeamScore { get; set; }
        public int OpponentsTeamScore { get; set; }
        public int HomeTeamWickets { get; set; }
        public int OppenentTeamWickets { get; set; }
        public string OpponentTeamLogo { get; set; }
        public string HomeTeamTeamLogo { get; set; }
    }
}
