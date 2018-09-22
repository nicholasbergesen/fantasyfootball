﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Data.Models
{
    public class ChampionPrediction
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }
        public int HomePrediction { get; set; }
        public int AwayPrediction { get; set; }

        [Required]
        public int ChampionFixtureID { get; set; }

        [ForeignKey("ChampionFixtureID")]
        public virtual ChampionFixture ChampionFixture { get; set; }

        public string UserName { get; set; }
    }
}
