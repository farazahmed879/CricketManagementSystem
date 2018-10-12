using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class TeamPlayersdto
    {

        public int PlayerId { get; set; }
        [Required]
        public string Player_Name { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public byte[] PlayerLogo { get; set; }
        public TeamDetailsdto TeamDetails { get; set; }
        public int TeamId { get; set; }

    }
}
