using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Models
{
    public class FastAlbumSearch
    {
        public String ArtistName { get; set; }
        public int ID { get; set; }
        public String imageLink { get; set; }
        public double price
        {
            get
            {
                return calculatePrice();
            }
            set
            {
                price = value;
            }
        }
        public List<Models.FastSongSearch> Songs { get; set; }
        public int views { get; set; }
        public String title { get; set; }

        public double calculatePrice()
        {

            if (this.Songs != null)
            {
                var total = 0.0;


                foreach (var s in Songs)
                {
                    total += s.price;
                }

                if (Songs.Count > 1)
                {
                    total = (total * .6);
                }

                return Math.Round(total, 2); 
            }
            return 0.0;
            
        }

    }


    
}