using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HostelworldScrape
{
    class readPageEmails
    {
        public static List<string> ExtractEmails(string textToScrape)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;

            List<string> results = new List<string>();
            for(match = reg.Match(textToScrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                {
                    results.Add(match.Value);
                }
            }

            return results;
        }
    }
}
