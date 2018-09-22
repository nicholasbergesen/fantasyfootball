using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class PremierFixture
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Fixture Date")]
        public DateTime FixtureDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public virtual ICollection<PremierPrediction> PremierPedictions { get; set; }
    }
}