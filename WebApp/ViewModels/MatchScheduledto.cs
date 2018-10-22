using CricketApp.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class MatchScheduledto
    {

        public long MatchScheduleId { get; set; }
        public string GroundName { get; set; }
        public string OpponentTeam { get; set; }
        public string Month { get; set; }
        public int? Day { get; set; }
        public int? Year { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
