﻿using CricketApp.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class ScoreCarddto
    {
        public ScoreCarddto()
        {
            HomeTeamScoreCard = new List<MatchSummarydto>(12);
            OpponentTeamScoreCard = new List<MatchSummarydto>(12);
            TeamScoreCard = new List<TeamScoredto>(2);
        }

        public List<MatchSummarydto> HomeTeamScoreCard { get; set; }
        public List<MatchSummarydto> OpponentTeamScoreCard { get; set; }
        public List<TeamScoredto> TeamScoreCard { get; set; }
    }
}