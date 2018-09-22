using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Data.Models
{
    public class League
    {
        [Key]
        public int Id { get; set; }
        public string LeagueName { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
