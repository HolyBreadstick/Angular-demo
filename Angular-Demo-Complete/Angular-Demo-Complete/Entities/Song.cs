using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Entities
{
    public class Song
    {
        [Key]
        public int ID { get; set; }

        public virtual Album Owner { get; set; }

        public String title { get; set; }

        public double price { get; set; }

        public bool onSale { get; set; }

        public double discount { get; set; }

        [NotMapped]
        public String formatPrice {
            get {
                return toFormat();
            }
            set {
                this.formatPrice = value;
            }
        }
        

        private string toFormat()
        {
            return "";
        }
    }
}