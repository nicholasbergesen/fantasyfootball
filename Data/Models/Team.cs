using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Data.Models
{
    public class Team
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }

        [Required]
        public int LeagueID { get; set; }

        [ForeignKey("LeagueID")]
        public virtual League League { get; set; }
    }
}
