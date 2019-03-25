﻿using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CricketApp.Domain
{
    public class Player
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
        public DateTime? DOB { get; set; }
        public string IsGuestorRegistered { get; set; }
        public bool IsDeactivated { get; set; }
        public int TeamId { get; set; }
        public BattingStyle BattingStyle { get; set; }
        public BowlingStyle BowlingStyle { get; set; }
        public PlayerRole PlayerRole { get; set; }
        public Team Team { get; set; }
        public string FileName { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[] PlayerLogo { get; set; }
        [NotMapped]
        [Display(Name = "Player Image")]
        public IFormFile PlayerImage { get; set; }
    }
}
