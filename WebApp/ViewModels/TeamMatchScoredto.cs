using CricketApp.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class TeamMatchScoredto    {
        public TeamMatchScoredto()
        {
            MatchScore = new List<MatchScoreDto>(12);
            TeamScore = new List<TeamScoredto>(2);
        }

        public List<MatchScoreDto> MatchScore { get; set; }
        public List<TeamScoredto> TeamScore { get; set; }
    }
}
