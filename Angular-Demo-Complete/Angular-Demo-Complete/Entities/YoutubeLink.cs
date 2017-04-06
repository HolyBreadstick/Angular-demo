using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Entities
{
    public class YoutubeLink
    {
        public int ID { get; set; }

        public virtual Song Owner { get; set; }

        public String Link { get; set; }

        public String BaseLink { get; set; } = "";
    }
}