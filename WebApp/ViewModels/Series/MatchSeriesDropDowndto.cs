using CricketApp.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class MatchSeriesDropDowndto
    {
        
        public int MatchSeriesId { get; set; }
        public string Name { get; set; }
    }
}
