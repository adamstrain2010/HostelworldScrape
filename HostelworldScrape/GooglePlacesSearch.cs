using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using Newtonsoft;
using System.Windows.Forms;

namespace HostelworldScrape
{
    class GooglePlacesSearch
    {
        public static string findID(string hostelName, string city)
        {
            string placeID = null;

            hostelName = hostelName.Replace(" ", @"%20");
            Clipboard.SetText("https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + hostelName + @"%20in%20" + city + "%20United%20Kingdom&key=AIzaSyBp2eZFy4gao6Bn_yUcYzWOzM-Deews6m8");
            //Logger.writeLog(Form1.txtConsole,@"https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + hostelName + @"%20in%20" + city + "%20United%20Kingdom&key=AIzaSyBp2eZFy4gao6Bn_yUcYzWOzM-Deews6m8");
            var client = new RestClient(@"https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + hostelName + @"%20in%20" + city + "%20United%20Kingdom&key=AIzaSyBp2eZFy4gao6Bn_yUcYzWOzM-Deews6m8");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "095beb5c-903d-423c-5bea-f83153ed0ff7");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            

            var jObject = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
            if(jObject.Count > 0)
            {
                foreach(var it in jObject["results"])
                {
                    foreach(Newtonsoft.Json.Linq.JProperty thing in it.Children())
                    {
                        if (thing.Name == "place_id")
                        {
                            placeID = thing.Value.ToString();
                            ////Logger.writeLog(Form1.txtConsole,hostelName + ": " + placeID);
                        }
                        if(thing.Name == null){
                            return "nada";
                        }    
                    }
                }
           }

            //List<string> res =  Parse(response.Content, @"Newtonsoft.Json.Linq.JObject", @"results");
            ////Logger.writeLog(Form1.txtConsole,"res: " + jObject);
            ////Logger.writeLog(Form1.txtConsole,response.Content);

            return placeID;
        }
        
        public static IList<string> things = new List<string>();

        public static string findSite(string placeId)
        {
            

            string websiteUrl = null;
            var client = new RestClient(@"https://maps.googleapis.com/maps/api/place/details/json?placeid=" + placeId + "&key=AIzaSyBp2eZFy4gao6Bn_yUcYzWOzM-Deews6m8");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "b1a45646-61df-ea84-af51-63ac8b25cd51");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            List<Newtonsoft.Json.Linq.JToken> jprops = new List<Newtonsoft.Json.Linq.JToken>();

            var jObject = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
            if(jObject.Count > 0)
            {
                if(jObject["result"] != null)
                {
                    foreach(var it in jObject["result"])
                {
                    foreach(var item in it)
                    {
                        if(item.ToString().Substring(0,2) == "ht")
                        {
                            things.Add(item.ToString());
                        }
                        else
                        {
                                things.Add("NO WEBSITE");
                        }
                    }
                        ////Logger.writeLog(Form1.txtConsole,jprops[jprops.Count() - 1]);
                        //foreach (Newtonsoft.Json.Linq.JProperty thing in it.Children())
                        //{
                        //    if (thing.Name == "website")
                        //        websiteUrl = thing.Value.ToString();
                        //}

                    }
                }
                else{
                        things.Add("NO WEBSITE");
                }

           }
            websiteUrl = things[things.Count() - 1];
            ////Logger.writeLog(Form1.txtConsole,websiteUrl);
            return websiteUrl;
        }

        public static void listThings()
        {
            //Logger.writeLog(Form1.txtConsole,"Things Length: " + things.Count());


            //Logger.writeLog(Form1.txtConsole,things[things.Count() - 1]);
            
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
                    //Logger.writeLog(Form1.txtConsole,ex.Message);
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
