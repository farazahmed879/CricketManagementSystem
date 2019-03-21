using CricketApp.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class TournamentDropDowndto
    {
        
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
    }
}
