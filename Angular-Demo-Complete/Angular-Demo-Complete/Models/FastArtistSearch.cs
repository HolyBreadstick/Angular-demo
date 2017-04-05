using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Models
{
    public class FastArtistSearch
    {
        public int ID { get; set; }
        
        public String firstName { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public  List<Models.FastAlbumSearch> Albums { get; set; }
    }
}