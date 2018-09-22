using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class Playersdto
    {

        public int PlayerId { get; set; }
        [Required]
        public string Player_Name { get; set; }
        public string Contact { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Address { get; set; }
        public string CNIC { get; set; }
        public int? BattingStyleId { get; set; }
        public int? BowlingStyleId { get; set; }
        public int? PlayerRoleId { get; set; }
        public string DOB { get; set; }
        public bool IsGuestPlayer { get; set; }
        public bool IsDeactivated { get; set; }
        public int TeamId { get; set; }
        public string BattingStyle { get; set; }
        public string BowlingStyle { get; set; }
        public string PlayerRole { get; set; }
        public string Team { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[] PlayerLogo { get; set; }
        [NotMapped]
        [Display(Name = "Player Image")]
        public IFormFile PlayerImage { get; set; }

    }
}
