using System;
using System.Collections.Generic;
using System.Text;

namespace CricketApp.Domain
{
   public class MatchType
    {
        public MatchType()
        {
            Matches = new List<Match>();
        }
        public int MatchTypeId { get; set; }
        public string MatchTypeName { get; set; }
        public List<Match> Matches { get; set; }
    }
}
