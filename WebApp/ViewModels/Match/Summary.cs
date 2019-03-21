using CricketApp.Domain;
using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class Summary
    {
        public Summary()
        {
            HomeTeamBatting = new List<HomeTeamBatting>();
            OppTeamBatting = new List<OppTeamBatting>();
            HomeTeamBowling = new List<HomeTeamBowling>();
            OppTeamBowling = new List<OppTeamBowling>();
        }
        public List<HomeTeamBowling> HomeTeamBowling { get; set; }
        public List<HomeTeamBatting> HomeTeamBatting { get; set; }
        public List<OppTeamBatting> OppTeamBatting { get; set; }
        public List<OppTeamBowling> OppTeamBowling { get; set; }
        public Summary2dto Summary2dto { get; set; }
    }
}
