using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular_Demo_Complete.Helpers
{
    public class YoutubeLinkExtractor
    {
        public static int FindVideoID(String link) {
            var parsedLink = link.Substring(link.IndexOf("v=")+2);


            using (var db = new MusicContext()) {
                var info = (from dt in db.YoutubeLinks where dt.Link == parsedLink select dt.Owner.ID).FirstOrDefault();
                return info;
            }
        }
    }
}