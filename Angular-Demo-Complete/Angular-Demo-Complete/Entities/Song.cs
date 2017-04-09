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

        public double storedPrice { get; set; } = 1.29;
        
        public bool onSale { get; set; } = false;

        public String FilePath { get; set; }

        public Boolean canDownload { get; set; }

        public virtual List<YoutubeLink> YoutubeLinks { get; set; } = new List<YoutubeLink>();

        [NotMapped]
        public double price
        {
            get
            {
                return calculatePrice();
            }
            set
            {
                this.price = value;
            }
        }
        
        [Range(0.0,1.0)]
        public double discount { get; set; } = 0.0;

        [NotMapped]
        public String formatPrice {
            get {
                return toFormat();
            }
            set {
                this.formatPrice = value;
            }
        }

        private double calculatePrice()
        {
            if (storedPrice == 0)
            {
                return 0;
            }
            else if (onSale == false)
            {
                return storedPrice;
            }
            else if (onSale)
            {
                if (discount != 0.0)
                {
                    var price = (storedPrice - (discount * storedPrice));
                    return Math.Round(price, 2);
                }
            }


            throw new Exception("Should never hit me!");
        }

        private string toFormat()
        {
            var p = price;
            if (p == 0) {
                return "FREE!";
            }
            else
                return String.Format("{0:$##,###,###,##0.00}", p);
        }
        
    }
}