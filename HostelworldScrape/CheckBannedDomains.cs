using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;

namespace HostelworldScrape
{
    class CheckBannedDomains
    {
        public static List<string> domainList = new List<string>();

        public static void getBannedDomains()
        {
            string domains = File.ReadAllText(@"BannedExtensions.json");
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(domains);
            foreach (KeyValuePair<string, Newtonsoft.Json.Linq.JToken> item in jObject)
                {

                    string key = item.Key;
                    var val = item.Value;
                    foreach (string thing in val)
                    {
                        domainList.Add(thing);
                    }
                }
            }

        public static bool check(string domain)
        {
            bool validDomain = true;
            foreach(string bannedDomain in domainList)
            {
                ////Logger.writeLog(Form1.txtConsole,"DOMAIN: " + domain);
                ////Logger.writeLog(Form1.txtConsole,"BANNED DOMAIN: " + bannedDomain);
                //if (domain.Contains(bannedDomain))
                if (domain.Contains(bannedDomain))
                {
                    return false;
                }
                
            }
            return validDomain;
        }
    }
}
