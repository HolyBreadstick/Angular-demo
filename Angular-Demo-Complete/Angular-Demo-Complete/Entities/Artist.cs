using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Entities
{
    public class Artist
    {
        [Key]
        public int ID { get; set; }
        
        [Index(IsUnique = true)]
        [MaxLength(999)]
        public String firstName { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public String FilePath { get; set; }

        public virtual List<Album> Albums { get; set; } = new List<Album>();
    }
}