using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace HostelworldScrape
{
    class googleTest
    {
        public static List<string> queries = new List<string>();

        public static string getDomain(string query, string city, string address)
        {
            HtmlAgilityPack.HtmlDocument htmlSnippet = new HtmlAgilityPack.HtmlDocument();

            string fullQuery = @"https://www.google.co.uk/#q=" + query + "+hostel+in+" + city;
            fullQuery = fullQuery.Replace("  ", " ");
            fullQuery = fullQuery.Replace(" ", "+");
            //Logger.writeLog(Form1.txtConsole,fullQuery);
            queries.Add(fullQuery);
            

            StringBuilder sb = new StringBuilder();
            byte[] ResultsBuffer = new byte[8192];
            string SearchResults = "http://google.com/search?q=" + query + "+hostel+in+" + city;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SearchResults);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response != null)
            {
                
            }
            Stream resStream = response.GetResponseStream();
            string tempString = null;
            int count = 0;
            do
            {
                count = resStream.Read(ResultsBuffer, 0, ResultsBuffer.Length);
                if (count != 0)
                {
                    tempString = Encoding.ASCII.GetString(ResultsBuffer, 0, count);
                    sb.Append(tempString);
                }
            }

            while (count > 0);
            string sbb = sb.ToString();

            List<string> links = new List<string>();

            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.OptionOutputAsXml = true;
            html.LoadHtml(sbb);
            HtmlNode doc = html.DocumentNode;

            foreach (HtmlNode link in doc.SelectNodes("(//a[@href])"))
            {
                //HtmlAttribute att = link.Attributes["href"];
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                if (!hrefValue.ToString().ToUpper().Contains("GOOGLE") && hrefValue.ToString().Contains("/url?q=") && hrefValue.ToString().ToUpper().Contains("HTTP://"))
                {
                    int index = hrefValue.IndexOf("&");
                    if (index > 0)
                    {
                        hrefValue = hrefValue.Substring(0, index);
                        links.Add(hrefValue.Replace("/url?q=", ""));
                    }
                }
            }


            //CHECK LINKS FOR BANNED DOMAINS (HOSTELWORLD ETC)
            bool notLink = true;
            string firstLink = null;

            for (int i = 0; notLink; i++)
            {
                if(i < links.Count)
                {
                    if (CheckBannedDomains.check(links[i]))
                    { 
                        firstLink = links[i];
                        notLink = false;
                    }
                }
                else
                {
                    firstLink = "NOPE";
                    notLink = false;
                }
                
            }
            
            
            
            //string firstLink = links[0];
            //Logger.writeLog(Form1.txtConsole,firstLink);
            //change this
            return (firstLink);
        }
    }
}
