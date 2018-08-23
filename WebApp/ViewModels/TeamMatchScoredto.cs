using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class TeamMatchScoredto    {
        public TeamMatchScoredto()
        {
            MatchScore = new List<MatchScoreDto>(12);
            TeamScore = new List<TeamScoredto>(2);
            FallOfWicket = new List<FallOfWicketdto>(2);
        }

        public List<MatchScoreDto> MatchScore { get; set; }
        public List<TeamScoredto> TeamScore { get; set; }
        public List<FallOfWicketdto> FallOfWicket { get; set; }
    }
}
