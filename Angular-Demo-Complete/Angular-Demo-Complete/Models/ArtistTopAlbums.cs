using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Models.ArtistSearch
{
    public class ArtistTopAlbums
    {
        public Topalbums topalbums { get; set; }
    }

    public class Topalbums
    {
        public Album[] album { get; set; }
        [JsonProperty("@attr")]
        public Attr attr { get; set; }
    }

    public class Attr
    {
        public string artist { get; set; }
        public string page { get; set; }
        public string perPage { get; set; }
        public string totalPages { get; set; }
        public string total { get; set; }
    }

    public class Album
    {
        public string name { get; set; }
        public int playcount { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public Artist artist { get; set; }
        public Image[] image { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
    }

    public class Image
    {
        [JsonProperty("#text")]
        public string text { get; set; }
        public string size { get; set; }
    }

}