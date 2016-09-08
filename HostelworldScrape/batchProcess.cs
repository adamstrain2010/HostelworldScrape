using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft;
using System.Net.Http;
using HtmlAgilityPack;

namespace HostelworldScrape
{
    class batchProcess
    {
        public static string googlisedHostelName;

        public static List<Hostel> resultData = new List<Hostel>();

        public static string selectedCountry = null;
        public static string city = null;
        public static Dictionary<string, List<string>> countryCities;
        //results
        public static List<string> hostelNames = new List<string>();
        public static List<string> hostelAdds = new List<string>();
        public static List<List<string>> hostelEmails = new List<List<string>>();
        public static SaveFileDialog outputLoc = new SaveFileDialog();


        public static void process(List<string> cities, string country)
        {
            foreach(string city in cities)
            {
            string filepath = @"http://www.hostelworld.com/hostels/" + city + @"?propertytype=HOSTEL&ShowAll=1";
            HtmlAgilityPack.HtmlDocument document = new  HtmlWeb().Load(filepath);
            
            HtmlNodeCollection hostelNames = document.DocumentNode.SelectNodes("//h2//a");
            ////Logger.writeLog(Form1.txtConsole,document.DocumentNode.OuterHtml);
            HtmlNodeCollection hostelAddresses = document.DocumentNode.SelectNodes("//div[contains(@class, 'addressline')]");
            //test if clause remove if not working
            if (hostelAddresses != null) {
                string workingHostel = null;
            string workingsite = null;

            
            foreach (HtmlNode addNode in hostelAddresses)
            {
                string split = addNode.InnerHtml.Substring(0, addNode.InnerHtml.IndexOf("...") - 1);
                hostelAdds.Add(split);
            }
            List<string> hostels = new List<string>();
            List<string> links = new List<string>();
            List<List<string>> emails = new List<List<string>>();
            for (int i2 = 0; i2 < hostelNames.Count; i2++)
            {
                workingHostel = hostelNames[i2].InnerText.ToString();
                googlisedHostelName = hostelNames[i2].InnerText.ToString().Replace(" ", "+");
                if (googlisedHostelName.Contains(city))
                {
                    googlisedHostelName.Replace(city, "");
                }

                StringBuilder sb = new StringBuilder();
                foreach (char c in googlisedHostelName)
                {
                    if (!char.IsPunctuation(c))
                    {
                        sb.Append(c);
                    }
                }
                googlisedHostelName = sb.ToString();

                hostels.Add(hostelNames[i2].InnerHtml);

            }


            for (int i = 0; i < hostels.Count; i++)
            {
                if (hostels[i].Contains(city))
                {
                    if (!hostels[i].StartsWith(city)) {
                        //Logger.writeLog(Form1.txtConsole,);
                        hostels[i] = hostels[i].Remove(hostels[i].IndexOf(city), city.Length);
                        //Logger.writeLog(Form1.txtConsole,hostels[i]);
                        //Logger.writeLog(Form1.txtConsole,"REMOVED: " + city);
                    }
                }
                string hostelPar = googleTest.getDomain(hostels[i], city, hostelAdds[i]);

                links.Add(hostelPar);
                workingsite = hostelPar;
            }
            

            List<string> emailListA = new List<string>();
            List<string> finalEmails = new List<string>();

            List<string> currentWorkingTuple = new List<string>();
            for (int i = 0; i < links.Count(); i++)
            {
                //Logger.writeLog(Form1.txtConsole,"link: " + links[i]);
                if(links[i] != "NO WEBSITE")
                {
                    emailListA =  ProcessWholeDomain.getEmailAddresses(links[i]);
                    //Logger.writeLog(Form1.txtConsole,"yep");
                    List<string> workingEmails  = new List<string>();
                    if(emailListA != null)
                    {
                        foreach (string em in emailListA)
                    {
                        //NULL TESTING
                        if(em != null)
                        {
                            finalEmails.Add(em);
                            currentWorkingTuple.Add(em);
                            workingEmails.Add(em);

                        }
                                            }
                    }
                    
                    emails.Add(workingEmails);
                    
                }
                else
                {
                    List<string> workingEmails  = new List<string>();
                    workingEmails.Add("NO EMAIL - NO WEBSITE");
                    emails.Add(workingEmails);
                }
                if(emailListA != null)
                {
                    //Logger.writeLog(Form1.txtConsole,emailListA.Count());
                }
                else
                {
                    //Logger.writeLog(Form1.txtConsole,@"emailListA == null");
                }
                
                
            }

            

            gatherFinalData(hostels, links, emails);
            exportToExcel.export(resultData, appVars.outputLoc);
            }
            else
            {
                MessageBox.Show("Hostelworld does not list " + city + "!");
            }
        }
            }
         

        public static void gatherFinalData(List<string> hostels, List<string> sites, List<List<string>> emails)
        {
            for(int i = 0; i < hostels.Count; i++)
            {
                Hostel newHostel = new Hostel();
                newHostel.name = hostels[i];
                newHostel.website = sites[i];
                newHostel.emails = emails[i];
                //Logger.writeLog(Form1.txtConsole,"HOSTEL[i]:" + newHostel.name);
            resultData.Add(newHostel);
            }
            //Logger.writeLog(Form1.txtConsole,"RESULTS RESULTS RESULTS");
            //Logger.writeLog(Form1.txtConsole,);
            foreach(Hostel hostel in resultData)
            {
                //Logger.writeLog(Form1.txtConsole,hostel.name);
                //Logger.writeLog(Form1.txtConsole,hostel.website);
            }
        }
    }
}
