using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class MatchBowler
    {

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public float? Overs { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public int? Maiden { get; set; }

    }
}
