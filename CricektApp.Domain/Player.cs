using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


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
        public string Status { get; set; }
        public string CNIC { get; set; }
        public string Role { get; set; }
        public string BattingStyle { get; set; }
        public string BowlingStyle { get; set; }
        public DateTime? DOB { get; set; }
        public bool IsGuestPlayer { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[] PlayerLogo { get; set; }
        [NotMapped]
        [Display(Name = "Player Image")]
        public IFormFile PlayerImage { get; set; }
    }
}
