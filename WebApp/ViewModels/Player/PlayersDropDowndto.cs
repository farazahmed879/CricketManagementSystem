using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class PlayersDropDowndto
    {

        public int PlayerId { get; set; }
        public string Player_Name { get; set; }
    }
}
