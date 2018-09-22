using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Data.Models
{
    public class UserPremierScore
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public int ExactPredictions { get; set; }
    }
}