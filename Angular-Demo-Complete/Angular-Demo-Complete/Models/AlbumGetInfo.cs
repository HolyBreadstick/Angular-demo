using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Models
{
    public class AlbumGetInfo
    {
        public Album album { get; set; }
    }

    

    public class Album
    {
        public string name { get; set; }
        public string artist { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public Image[] image { get; set; }
        public string listeners { get; set; }
        public string playcount { get; set; }
        public Tracks tracks { get; set; }
        public Tags tags { get; set; }
        public Wiki wiki { get; set; }
    }

    public class Tracks
    {
        public Track[] track { get; set; }
    }

    public class Track
    {
        public string name { get; set; }
        public string url { get; set; }
        public string duration { get; set; }
        [JsonProperty("@attr")]
        public Attr attr { get; set; }
        public Streamable streamable { get; set; }
        public Artist artist { get; set; }
    }

    public class Attr
    {
        public string rank { get; set; }
    }

    public class Streamable
    {
        [JsonProperty("#text")]
        public string text { get; set; }
        public string fulltrack { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
    }

    public class Tags
    {
        public Tag[] tag { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Wiki
    {
        public string published { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
    }

    public class Image
    {
        public string text { get; set; }
        public string size { get; set; }
    }

}