using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft;
using System.IO;
 

namespace HostelworldScrape
{
    class emailApiRead
    {
        public static List<string> getEmails(string domain)
        {
            List<string> output = new List<string>();
            string url = @"https://api.emailhunter.co/v1/search?domain=" + domain + @"&api_key=" + Properties.Settings.Default.APIkey;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "62a2ee18-26ba-cc00-7a60-1c41848e858b");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            foreach(string thing in Parse(response.Content))
            {
                output.Add(thing);
            }
            return output;
        }



        public static List<String> Parse(string json)
        {
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            List<string> result = new List<string>();
            if (jObject.Count > 0)
            {
                foreach (var item in jObject["emails"])
                {
                    foreach (Newtonsoft.Json.Linq.JProperty thing in item.Children())
                    {
                        if (thing.Name == "value")
                        {
                            result.Add(thing.Value.ToString());
                        }
                    }
                }
            }
            else
            {
                result = null;
            }
            

            return result;
        }
    }
}

