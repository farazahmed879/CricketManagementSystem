using CricketApp.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class HomeScreendto
    {
        public int Tournaments { get; set; }
        public int Players { get; set; }
        public int Teams { get; set; }
        public int Matches { get; set; }
        public int Records { get; set; }
        public int Result { get; set; }
    }
}
