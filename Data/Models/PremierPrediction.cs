using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class PremierPrediction
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }
        public int HomePrediction { get; set; }
        public int AwayPrediction { get; set; }

        [Required]
        public int PremierFixtureID { get; set; }

        [ForeignKey("PremierFixtureID")]
        public virtual PremierFixture PremierFixture { get; set; }

        public string UserName { get; set; }
    }
}