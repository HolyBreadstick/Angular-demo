using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Angular_Demo_Complete.Helpers
{
    public class RemoveIllegalPathCharacters
    {
        public static string RemoveCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        public static String FormatForBrowser(String path) {
            var newPath = path.Substring(path.IndexOf("ArtistData"));
            return newPath.Replace("\\", "/");
        }

        public static String RemoveSpaces(String path) {
            return path.Replace(" ", "_");
        }
    }
}