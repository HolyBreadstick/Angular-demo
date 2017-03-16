using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Entities
{
    public class Artist
    {
        [Key]
        public int ID { get; set; }

        public String firstName { get; set; }
        public String lastName { get; set; }
        public DateTime birthdate { get; set; }

        public virtual List<Album> Albums { get; set; } = new List<Album>();
    }
}