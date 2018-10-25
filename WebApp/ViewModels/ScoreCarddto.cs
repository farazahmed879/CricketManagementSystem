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
            HomeTeamFOW = new List<FallOfWicketdto>(2);
            OpponentTeamFOW = new List<FallOfWicketdto>(2);
            FallOfWicket = new List<FallOfWicketdto>(2);
            //    ViewBagHomeTeam = new List<ViewBagHomeTeamdto>();
            //    ViewBagOpponentTeam = new List<ViewBagOpponentTeamdto>();

        }

        public List<MatchSummarydto> HomeTeamScoreCard { get; set; }
        public List<MatchSummarydto> OpponentTeamScoreCard { get; set; }
        public List<TeamScoredto> TeamScoreCard { get; set; }
        public List<FallOfWicketdto> HomeTeamFOW { get; set; }
        public List<FallOfWicketdto> OpponentTeamFOW { get; set; }
        public List<FallOfWicketdto> FallOfWicket { get; set; }
        //  public List<ViewBagOpponentTeamdto> ViewBagOpponentTeam { get; set; }
        //  public List<ViewBagHomeTeamdto> ViewBagHomeTeam { get; set; }

    }
}
