using System;
using System.Collections.Generic;
using WebApp.ViewModels.Home;

namespace WebApp.ViewModels
{
    public class HomeScreendto
    {
        public HomeScreendto()
        {
            RecentMatches = new List<RecentMatchesdto>();
        }

        public TotalAchievedto TotalAchievedto { get; set; }
        public List<RecentMatchesdto> RecentMatches { get; set; }
    }
}
