using CricketApp.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.ViewModels
{
    public class ViewBagHomeTeamdto
    {

        public int TeamId { get; set; }
        public string Team_Name { get; set; }

    }
}
