using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using System.Windows.Forms;

namespace HostelworldScrape
{
    class domainGrab
    {
       

        

        public static string getDomain(string property)
        {
            string result = null;

            property.Replace(" ", "%20");
            var client = new RestClient("https://api.cognitive.microsoft.com/bing/v5.0/search?q=" + property + "&count=1&offset=0&mkt=en-us&safesearch=Moderate");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "445c2e32-337b-f346-bb55-c3da94318279");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("ocp-apim-subscription-key", "1ed58e2f0acb47f9a5cefd29000e1cfa");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\n    \"Ocp-Apim-Subscription-Key\" : \"AXMPcxlXPA2M7GCa3uJeTB/vwMCWNdYAT3CHkbWyt/I\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            

            string secondParseString = null;
            for (int i = 0; i < 1; i++) 
            {
                secondParseString = Parse(response.Content, @"Newtonsoft.Json.Linq.JArray", @"webPages").First();
            }
            


            var jobj = Newtonsoft.Json.Linq.JArray.Parse(secondParseString);
            for(int i2 = 0; i2 < jobj.Count(); i2++)
            {
                if(i2 == 0)
                {
                     foreach (Newtonsoft.Json.Linq.JObject thing in jobj.Children())
                    {
                        foreach(KeyValuePair<string, Newtonsoft.Json.Linq.JToken> jp in thing)
                        {
                            if (jp.Key == "displayUrl")
                            {
                                if(jp.Value.ToString().Substring(0,2) == "ht")
                                {
                                    System.Uri URI = new System.Uri(jp.Value.ToString());
                                    string uriDomain = URI.GetLeftPart(UriPartial.Authority);
                                    uriDomain = uriDomain.Replace(@"https://", "");
                                    uriDomain = uriDomain.Replace(@"http://", "");
                                    uriDomain = uriDomain.Replace("www", "");
                                    result = uriDomain;
                                }
                                else
                                {
                                    result = jp.Value.ToString();
                                }
                                
                            }
                        }
                        
                    }
                }
            }

            if(result != null)
            {
                return result;
            }
            else
            {
                return "null error. Contact Adam";
            }
            
        }

        

        public static List<string> Parse(string json, string type, string objectName)
        {
            
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            List<string> result = new List<string>();
            if (jObject.Count > 0)
            {
                try
                {
                    foreach (var item in jObject[objectName])
                    {
                        foreach (var thing in item.Children())
                        { 
                            if(thing.GetType().ToString() == type) {
                                result.Add(thing.ToString());
                            };
                        }
                    }
                }
                catch(Exception ex)
                {
                    
                }
                
            }
            else
            {
                result = null;
            }

            appVars.sitesScanned++;
            return result;
        }
    }
}
