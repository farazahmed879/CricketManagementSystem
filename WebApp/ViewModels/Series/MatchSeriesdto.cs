using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class MatchSeriesdto
    {
        
        public int MatchSeriesId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Organizor { get; set; }
        public string StartingDate { get; set; }
        public string FileName { get; set; }
        [Display(Name = "Series Image")]
        public IFormFile SeriesImage { get; set; }
        public int UserId { get; set; }
        public ApplicationUser TenantUser { get; set; }
    }
}
