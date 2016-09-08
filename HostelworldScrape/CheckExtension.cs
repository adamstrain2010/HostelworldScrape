using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;

namespace HostelworldScrape
{
    class CheckExtension
    {
        public static List<string> extensionList = new List<string>();

        public static void getExtensions()
        {
            string extensions = File.ReadAllText(@"fileExtensions.json");
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(extensions);
           
                foreach (KeyValuePair<string, Newtonsoft.Json.Linq.JToken> item in jObject)
                {

                    string key = item.Key;
                    var val = item.Value;
                    foreach (string thing in val)
                    {
                        extensionList.Add(thing);
                    }
                }
           
        }

        public static bool validExtension(string input)
        {
            bool output = false;
            foreach(string extension in extensionList)
            {
                
                int l = input.IndexOf("?");
                if (l >0)
                {
                    input = input.Substring(0, l);
                    //Logger.writeLog(Form1.txtConsole,@"INPUT '?'" + input);
                }
                l = input.IndexOf("%");
                if (l >0)
                {
                    input = input.Substring(0, l);
                    //Logger.writeLog(Form1.txtConsole,@"INPUT '%'" + input);
                }
                if(input == extension)
                {
                    output = true;
                }
                if (output == true)
                {
                    //Logger.writeLog(Form1.txtConsole,"Extension: " + input);
                }
                else
                {
                    //Logger.writeLog(Form1.txtConsole,"BAD EXTENSION: " + input);
                }
                
            }
            
            return output;

        }
    }
}
