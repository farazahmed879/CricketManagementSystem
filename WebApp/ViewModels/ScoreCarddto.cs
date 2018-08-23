using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class ScoreCarddto
    {
        public ScoreCarddto()
        {
            HomeTeamScoreCard = new List<MatchSummarydto>(12);
            OpponentTeamScoreCard = new List<MatchSummarydto>(12);
            TeamScoreCard = new List<TeamScoredto>(2);
            FallOfWicket = new List<FallOfWicketdto>(2);
        }

        public List<MatchSummarydto> HomeTeamScoreCard { get; set; }
        public List<MatchSummarydto> OpponentTeamScoreCard { get; set; }
        public List<TeamScoredto> TeamScoreCard { get; set; }
        public List<FallOfWicketdto> FallOfWicket { get; set; }
    }
}
