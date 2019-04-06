using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class Teamdto
    {

        public int TeamId { get; set; }
        [Required]
        public string Team_Name { get; set; }
        public string FileName { get; set; }
        public string Place { get; set; }
        public string Zone { get; set; }
        public string Contact { get; set; }
        public bool IsRegistered { get; set; }
        [Required]
        public string City { get; set; }
        [Column(TypeName = "varbinary(max)")]
        [NotMapped]
        [Display(Name = "Team Image")]
        public IFormFile TeamImage { get; set; }

    }
}
