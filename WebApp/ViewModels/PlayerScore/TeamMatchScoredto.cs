using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class TeamMatchScoredto
    {
        public TeamMatchScoredto()
        {
           // HomeTeamScoreCard = new List<MatchSummarydto>(15);
           // OpponentTeamScoreCard = new List<MatchSummarydto>(15);
            TeamScore = new List<TeamScoredto>(2);
            FallOfWicket = new List<FallOfWicketdto>(2);
        }

       // public List<MatchSummarydto> HomeTeamScoreCard { get; set; }
        //public List<MatchSummarydto> OpponentTeamScoreCard { get; set; }
        public List<TeamScoredto> TeamScore { get; set; }
        public List<FallOfWicketdto> FallOfWicket { get; set; }
        public Summary2dto Summary2dto { get; set; }
        public MatchSummarydto HomeTeamScoreCard { get; set; }
        public MatchSummarydto OppoTeamScoreCard { get; set; }
    }
}
