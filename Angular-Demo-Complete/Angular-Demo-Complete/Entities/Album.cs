using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Entities
{
    public class Album
    {
        [Key]
        public int ID { get; set; }

        public virtual Artist Owner { get; set; }

        public String title { get; set; }

        public int views { get; set; }

        public virtual List<Song> Songs { get; set; } = new List<Song>();

        public byte[] image { get; set; }

        [NotMapped]
        public double price { get; set; }

        public bool downloadImage() {
            return false;
        }

        public double calculatePrice() {
            return 0.0;
        }

    }
}