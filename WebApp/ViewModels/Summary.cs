using CricketApp.Domain;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class Summary
    {
        public Summary()
        {
            MatchSummaryPlayerList = new List<MatchSummaryPlayerList>();
        }
        public List<MatchSummaryPlayerList> MatchSummaryPlayerList { get; set; }
        public Summary2dto Summary2dto { get; set; }
    }
}
