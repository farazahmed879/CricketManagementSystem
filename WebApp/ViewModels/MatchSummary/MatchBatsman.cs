using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class MatchBatsman
    {

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int? Runs { get; set; }
        public int? Balls { get; set; }
        public int? Six { get; set; }
        public int? Four { get; set; }
        public string HowOut { get; set; }
        public string Bowler { get; set; }
        public string HomeTeam { get; set; }
        public string OppTeam { get; set; }

    }
}
