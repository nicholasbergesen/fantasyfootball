using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Data.Models
{
    public class Post 
    {
        [Key]
        public int ID { get; set; }
        public DateTime DatePosted { get; set; }
        public string Comment { get; set; }
        public string PostedBy { get; set; }
    }
}
