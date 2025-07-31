using System.Collections.Generic;
using System.Text;

namespace CSharpTools.HTTP.Utility
{
    public static class HttpUtility
    {
        private static readonly Dictionary<char, string> UrlEncodings = new()
        {
            { '%', "%25" },
            { ' ', "%20" },
            { '!', "%21" },
            { '"', "%22" },
            { '#', "%23" },
            { '$', "%24" },
            { '&', "%26" },
            { '\'', "%27" },
            { '(', "%28" },
            { ')', "%29" },
            { '*', "%2A" },
            { '+', "%2B" },
            { ',', "%2C" },
            { '-', "%2D" },
            { '.', "%2E" },
            { '/', "%2F" },
            { ':', "%3A" },
            { ';', "%3B" },
            { '<', "%3C" },
            { '=', "%3D" },
            { '>', "%3E" },
            { '?', "%3F" },
            { '@', "%40" },
            { '[', "%5B" },
            { '\\', "%5C" },
            { ']', "%5D" },
            { '^', "%5E" },
            { '_', "%5F" },
            { '`', "%60" },
            { '{', "%7B" },
            { '|', "%7C" },
            { '}', "%7D" },
            { '~', "%7E" },
            { '€', "%E2%82%AC" },
        };
    
        public static string ReplaceUrlReservedCharacters(string input)
        {
            StringBuilder sb = new();

            foreach (char c in input)
            {
                if (UrlEncodings.TryGetValue(c, out string? encoding))
                    sb.Append(encoding);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}