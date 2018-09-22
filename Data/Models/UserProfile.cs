using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int FixtureCount { get; set; }
        public int ScoreCount { get; set; }
        public int PostCount { get; set; }
        public int LoginCount { get; set; }
        public int FunCount { get; set; }
    }
}