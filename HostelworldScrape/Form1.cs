using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace HostelworldScrape
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static List<Hostel> resultData = new List<Hostel>();

        string selectedCountry = null;
        string selectedCity = null;
        Dictionary<string, List<string>> countryCities;
        //results
        public static List<string> hostelNames = new List<string>();
        public static List<string> hostelAdds = new List<string>();
        public static List<List<string>> hostelEmails = new List<List<string>>();
        public SaveFileDialog outputLoc = new SaveFileDialog();

        private void Form1_Load(object sender, EventArgs e)
        {

            toolStripStatusLabel1.Text = "inactive";
               
            comboBoxCity.Enabled = false;
            CheckExtension.getExtensions();
            CheckBannedDomains.getBannedDomains();
            string fullList = null;
            List<string> countries = new List<string>();
            
            using (StreamReader sr = new StreamReader("countries.json"))
            {
                fullList = sr.ReadToEnd();
            }
            countryCities = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(fullList);
            foreach(string country in countryCities.Keys)
            {
                countries.Add(country);
            }
            countries.Sort();
            foreach (string country in countries)
            {
                comboBoxCountry.Items.Add(country);
            }
            comboBoxCountry.SelectedItem = "United Kingdom";
            radioCountry.Focus();
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCity.Items.Clear();
            selectedCountry = comboBoxCountry.SelectedItem.ToString();
            List<string> cities = new List<string>();
            foreach (KeyValuePair<string, List<string>> kvp in countryCities)
            {
                if(selectedCountry == kvp.Key)
                {
                    foreach(string city in kvp.Value)
                    {
                        cities.Add(city); ;
                    }
                    cities.Sort();
                    foreach(string city in cities)
                    {
                        comboBoxCity.Items.Add(city);
                    }
                }
            }
        }

        public string googlisedHostelName;
       
        public static string getRedirectPath(string url)
        {
         
            StringBuilder sb = new StringBuilder();
            string location = string.Copy(url);
            
            while (!String.IsNullOrWhiteSpace(location))
            {
                sb.AppendLine(location);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(location);
                request.AllowAutoRedirect = false;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    string check = response.ResponseUri.ToString();
                    location = response.GetResponseHeader("Location");
                }
                
            }
            return sb.ToString();
        }

        private void gatherFinalData(List<string> hostels, List<string> sites, List<List<string>> emails)
        {
            for(int i = 0; i < hostels.Count; i++)
            {
                Hostel newHostel = new Hostel();
                newHostel.name = hostels[i];
                newHostel.website = sites[i];
                newHostel.emails = emails[i];
                            resultData.Add(newHostel);
            }
            Logger.writeLog(txtConsole,"RESULTS RESULTS RESULTS");
            Logger.writeLog(txtConsole, "");
            foreach (Hostel hostel in resultData)
            {
                Logger.writeLog(txtConsole,hostel.name);
                Logger.writeLog(txtConsole,hostel.website);
                Logger.writeLog(txtConsole,"");
            }
        }

        public bool isScrapeComplete = false;

        
        private void scrape()
        {
            toolStripStatusLabel1.Text = "working, please be patient";
            if(radioCity.Checked == true)
            {
                string html = "";
                string city = comboBoxCity.SelectedItem.ToString();
                string inputCityString = city;
                string spaceRemoveCityInputString = inputCityString.Replace(" ", @"%20");

                string inputCountryString = selectedCountry;
                string spaceRemoveCountryInputString = inputCountryString.Replace(" ", @"%20");

                Uri filepath = new Uri(@"http://www.hostelworld.com/findabed.php/ChosenCity." + spaceRemoveCityInputString + @"/" + @"ChosenCountry." + spaceRemoveCountryInputString + @"?propertytype=HOSTEL&ShowAll=1");
               
                //string filepath = @"http://www.hostelworld.com/hostels/" + inputString +  + @"?propertytype=HOSTEL&ShowAll=1";
                HttpWebResponse response = null;
                Logger.writeLog(txtConsole,inputCityString + ", " + inputCountryString);
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filepath);

                    using (response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream stream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(stream);
                        html = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {

                    Logger.writeLog(txtConsole,inputCityString + " is not a valid hostelworld destination.");
                }

                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);


                //Logger.writeLog(txtConsole,document.DocumentNode.OuterHtml);




                HtmlNodeCollection hostelNames = document.DocumentNode.SelectNodes("//h2//a");
                ////Logger.writeLog(txtConsole,document.DocumentNode.OuterHtml);
                HtmlNodeCollection hostelAddresses = document.DocumentNode.SelectNodes("//div[contains(@class, 'addressline')]");
                //test if clause remove if not working
                if (hostelAddresses != null)
                {
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
                        if (googlisedHostelName.Contains(selectedCity))
                        {
                            googlisedHostelName.Replace(selectedCity, "");
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

                    Logger.writeLog(txtConsole,"HOSTELS COUNT: " + hostels.Count());
                    for (int i = 0; i < hostels.Count; i++)
                    {
                        if (hostels[i].Contains(selectedCity))
                        {
                            if (!hostels[i].StartsWith(selectedCity))
                            {
                                //Logger.writeLog(txtConsole,"");
                                hostels[i] = hostels[i].Remove(hostels[i].IndexOf(selectedCity), selectedCity.Length);
                                //Logger.writeLog(txtConsole,hostels[i]);
                                //Logger.writeLog(txtConsole,"REMOVED: " + selectedCity);
                            }
                        }
                        hostels[i] = hostels[i].Replace(@"&#039;", @"%27");
                        //MessageBox.Show(hostels[i]);
                        
                        hostels[i] = hostels[i].Replace(@"'", "");
                        string hostelPar = googleTest.getDomain(hostels[i], selectedCity, hostelAdds[i]);
                        Logger.writeLog(txtConsole,"HOSTELPAR: " + hostelPar);
                        links.Add(hostelPar);
                        workingsite = hostelPar;
                    }
                    

                    List<string> emailListA = new List<string>();
                    List<string> finalEmails = new List<string>();

                    List<string> currentWorkingTuple = new List<string>();
                    for (int i = 0; i < links.Count(); i++)
                    {
                        //Logger.writeLog(txtConsole,"link: " + links[i]);
                        if (links[i] != "NO WEBSITE")
                        {
                            //emailListA = ProcessWholeDomain.getEmailAddresses(links[i]);
                            Logger.writeLog(txtConsole,"LINK: " + links[i]);
                            emailListA = ProcessDomain.getEmails(links[i]);
                            Logger.writeLog(txtConsole,"EMAIL LIST A COUNT : " + emailListA.Count);
                            //Logger.writeLog(txtConsole,"yep");
                            List<string> workingEmails = new List<string>();
                            if (emailListA != null)
                            {
                                foreach (string em in emailListA)
                                {
                                    //NULL TESTING
                                    if (em != null)
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
                            List<string> workingEmails = new List<string>();
                            workingEmails.Add("NO EMAIL - NO WEBSITE");
                            emails.Add(workingEmails);
                        }
                    }

                    
                    gatherFinalData(hostels, links, emails);
                    //exportToExcel.export(resultData);
                }
                else
                {
                    Logger.writeLog(txtConsole,"Hostelworld does not list " + inputCityString + ", " + inputCountryString + @"!");
                }

            
            exportToExcel.export(resultData, appVars.outputLoc);
            



        
        

            //string filepath = @"http://www.hostelworld.com/hostels/" + selectedCity + @"?propertytype=HOSTEL&ShowAll=1";
            //HtmlAgilityPack.HtmlDocument document = new HtmlWeb().Load(filepath);
            ////HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filepath);
            ////HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            ////MessageBox.Show(response.ToString());

            //HtmlNodeCollection hostelNames = document.DocumentNode.SelectNodes("//h2//a");
            //////Logger.writeLog(txtConsole,document.DocumentNode.OuterHtml);
            //HtmlNodeCollection hostelAddresses = document.DocumentNode.SelectNodes("//div[contains(@class, 'addressline')]");
            ////test if clause remove if not working
            //if (hostelAddresses != null)
            //{
            //    string workingHostel = null;
            //    string workingsite = null;


            //    foreach (HtmlNode addNode in hostelAddresses)
            //    {
            //        string split = addNode.InnerHtml.Substring(0, addNode.InnerHtml.IndexOf("...") - 1);
            //        hostelAdds.Add(split);
            //    }
            //    List<string> hostels = new List<string>();
            //    List<string> links = new List<string>();
            //    List<List<string>> emails = new List<List<string>>();
            //    for (int i2 = 0; i2 < hostelNames.Count; i2++)
            //    {
            //        workingHostel = hostelNames[i2].InnerText.ToString();
            //        googlisedHostelName = hostelNames[i2].InnerText.ToString().Replace(" ", "+");
            //        if (googlisedHostelName.Contains(selectedCity))
            //        {
            //            googlisedHostelName.Replace(selectedCity, "");
            //        }

            //        StringBuilder sb = new StringBuilder();
            //        foreach (char c in googlisedHostelName)
            //        {
            //            if (!char.IsPunctuation(c))
            //            {
            //                sb.Append(c);
            //            }
            //        }
            //        googlisedHostelName = sb.ToString();

            //        hostels.Add(hostelNames[i2].InnerHtml);

            //    }


            //    for (int i = 0; i < hostels.Count; i++)
            //    {
            //        if (hostels[i].Contains(selectedCity))
            //        {
            //            if (!hostels[i].StartsWith(selectedCity))
            //            {
            //                //Logger.writeLog(txtConsole,"");
            //                hostels[i] = hostels[i].Remove(hostels[i].IndexOf(selectedCity), selectedCity.Length);
            //                //Logger.writeLog(txtConsole,hostels[i]);
            //                //Logger.writeLog(txtConsole,"REMOVED: " + selectedCity);
            //            }
            //        }
            //        string hostelPar = googleTest.getDomain(hostels[i], selectedCity, hostelAdds[i]);
            //        links.Add(hostelPar);
            //        workingsite = hostelPar;
            //    }
            //    using (StreamWriter sw = new StreamWriter(@"C:\Users\adams\Desktop\log.txt"))
            //    {
            //        foreach (string query in googleTest.queries)
            //        {
            //            sw.WriteLine(query);
            //        }
            //    }

            //    List<string> emailListA = new List<string>();
            //    List<string> finalEmails = new List<string>();

            //    List<string> currentWorkingTuple = new List<string>();
            //    for (int i = 0; i < links.Count(); i++)
            //    {
            //        //Logger.writeLog(txtConsole,"link: " + links[i]);
            //        if (links[i] != "NO WEBSITE")
            //        {
            //            emailListA = ProcessWholeDomain.getEmailAddresses(links[i]);
            //            //Logger.writeLog(txtConsole,"yep");
            //            List<string> workingEmails = new List<string>();
            //            if (emailListA != null)
            //            {
            //                foreach (string em in emailListA)
            //                {
            //                    //NULL TESTING
            //                    if (em != null)
            //                    {
            //                        finalEmails.Add(em);
            //                        currentWorkingTuple.Add(em);
            //                        workingEmails.Add(em);

            //                    }
            //                }
            //            }

            //            emails.Add(workingEmails);

            //        }
            //        else
            //        {
            //            List<string> workingEmails = new List<string>();
            //            workingEmails.Add("NO EMAIL - NO WEBSITE");
            //            emails.Add(workingEmails);
            //        }
            //        if (emailListA != null)
            //        {
            //            //Logger.writeLog(txtConsole,emailListA.Count());
            //        }
            //        else
            //        {
            //            //Logger.writeLog(txtConsole,@"emailListA == null");
            //        }


            //    }

            //    using (StreamWriter sw3 = new StreamWriter(@"C:\Users\adams\Desktop\email_list.txt"))
            //    {

            //        foreach (string email in finalEmails)
            //        {

            //            //Logger.writeLog(txtConsole,"FINAL EMAIL: " + email);
            //            sw3.WriteLine(email);

            //        }

            //    }




            //    using (StreamWriter sr2 = new StreamWriter(txtOutputLoc.Text))
            //    {
            //        sr2.WriteLine("<!DOCTYPE html>");
            //        sr2.WriteLine("<html>");
            //        sr2.WriteLine("<head>");
            //        sr2.WriteLine("</head>");
            //        sr2.WriteLine("<body>");
            //        sr2.WriteLine("<div style='width:100%'>");
            //        sr2.WriteLine("<table style='border: think solid black'>");
            //        for (int i = 0; i < hostels.Count; i++)
            //        {
            //            sr2.WriteLine("<tr>");
            //            sr2.WriteLine("<td>" + hostels[i] + "</td>");
            //            sr2.WriteLine("<td>" + "<a href=" + links[i] + ">link</a>" + "</td>");
            //            foreach (string email in emails[i])
            //            {
            //                sr2.WriteLine("<td>" + email + "</td>");
            //            }
            //            sr2.WriteLine("<tr>");
            //        }
            //        sr2.WriteLine("</table>");
            //        sr2.WriteLine("</div>");
            //        sr2.WriteLine("</body>");
            //        sr2.WriteLine("</html>");
            //    }

            //    gatherFinalData(hostels, links, emails);
            //    exportToExcel.export(resultData);
            //}
            //else
            //{
            //    MessageBox.Show("Hostelworld does not list " + selectedCity + "!");
            //}

            
            }
            else if(radioCountry.Checked == true)
            {
                selectedCountry = comboBoxCountry.SelectedItem.ToString();
            List<string> cities = new List<string>();
            foreach (KeyValuePair<string, List<string>> kvp in countryCities)
            {
                if(selectedCountry == kvp.Key)
                {
                    foreach(string city in kvp.Value)
                    {
                        cities.Add(city);
                    }
                    cities.Sort();
                }
            }

            

            string html = "";
            foreach(string city in cities)
            {
                string inputCityString = city;
                string spaceRemoveCityInputString = inputCityString.Replace(" ", @"%20");

                string inputCountryString = selectedCountry;
                string spaceRemoveCountryInputString = inputCountryString.Replace(" ", @"%20");

                Uri filepath = new Uri(@"http://www.hostelworld.com/findabed.php/ChosenCity." + spaceRemoveCityInputString + @"/" + @"ChosenCountry." + spaceRemoveCountryInputString);
                //string filepath = @"http://www.hostelworld.com/hostels/" + inputString +  + @"?propertytype=HOSTEL&ShowAll=1";
                HttpWebResponse response = null;
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,inputCityString + ", " + inputCountryString);
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                Logger.writeLog(txtConsole,"");
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filepath);
                
                    using (response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream stream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(stream);
                        html = reader.ReadToEnd();
                        //Logger.writeLog(txtConsole,html);
                    }
                }
                catch (Exception ex)
                {
                
                    Logger.writeLog(txtConsole,inputCityString + " is not a valid hostelworld destination.");
                }

                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);
            
            
            Logger.writeLog(txtConsole,document.DocumentNode.OuterHtml);




            HtmlNodeCollection hostelNames = document.DocumentNode.SelectNodes("//h2//a");
            ////Logger.writeLog(txtConsole,document.DocumentNode.OuterHtml);
            HtmlNodeCollection hostelAddresses = document.DocumentNode.SelectNodes("//div[contains(@class, 'addressline')]");
            //test if clause remove if not working
            if (hostelAddresses != null)
            {
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
                    if (googlisedHostelName.Contains(selectedCity))
                    {
                        googlisedHostelName.Replace(selectedCity, "");
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

                Logger.writeLog(txtConsole,"hostels count: " + hostels.Count());
                for (int i = 0; i < hostels.Count; i++)
                {
                    if (hostels[i].Contains(selectedCity))
                    {
                        if (!hostels[i].StartsWith(selectedCity))
                        {
                            //Logger.writeLog(txtConsole,"");
                            hostels[i] = hostels[i].Remove(hostels[i].IndexOf(selectedCity), selectedCity.Length);
                            Logger.writeLog(txtConsole,hostels[i]);
                            //Logger.writeLog(txtConsole,"REMOVED: " + selectedCity);
                        }
                    }
                    hostels[i] = hostels[i].Replace(@"&#039;", @"%27");
                    //MessageBox.Show(hostels[i]);
                    
                    hostels[i] = hostels[i].Replace(@"'", "");
                    Logger.writeLog(txtConsole,"hostels[" + i + "] = " + hostels[i]);
                    string hostelPar = googleTest.getDomain(hostels[i], selectedCity, hostelAdds[i]);
                    Logger.writeLog(txtConsole,"Hostel Par: " + hostelPar);
                    links.Add(hostelPar);
                    workingsite = hostelPar;
                }
                
                List<string> emailListA = new List<string>();
                List<string> finalEmails = new List<string>();

                List<string> currentWorkingTuple = new List<string>();
                for (int i = 0; i < links.Count(); i++)
                {
                    //Logger.writeLog(txtConsole,"link: " + links[i]);
                    if (links[i] != "NO WEBSITE")
                    {
                        //emailListA = ProcessWholeDomain.getEmailAddresses(links[i]);
                        emailListA = ProcessDomain.getEmails(links[i]);
                        Logger.writeLog(txtConsole,"EMAIL LIST A COUNT : " + emailListA.Count);
                        List<string> workingEmails = new List<string>();
                        if (emailListA != null)
                        {
                            foreach (string em in emailListA)
                            {
                                //NULL TESTING
                                if (em != null)
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
                        List<string> workingEmails = new List<string>();
                        workingEmails.Add("NO EMAIL - NO WEBSITE");
                        emails.Add(workingEmails);
                    }
                    if (emailListA != null)
                    {
                        //Logger.writeLog(txtConsole,emailListA.Count());
                    }
                    else
                    {
                        //Logger.writeLog(txtConsole,@"emailListA == null");
                    }


                }

                




                //using (StreamWriter sr2 = new StreamWriter(txtOutputLoc.Text))
                //{
                //    sr2.WriteLine("<!DOCTYPE html>");
                //    sr2.WriteLine("<html>");
                //    sr2.WriteLine("<head>");
                //    sr2.WriteLine("</head>");
                //    sr2.WriteLine("<body>");
                //    sr2.WriteLine("<div style='width:100%'>");
                //    sr2.WriteLine("<table style='border: think solid black'>");
                //    for (int i = 0; i < hostels.Count; i++)
                //    {
                //        sr2.WriteLine("<tr>");
                //        sr2.WriteLine("<td>" + hostels[i] + "</td>");
                //        sr2.WriteLine("<td>" + "<a href=" + links[i] + ">link</a>" + "</td>");
                //        foreach (string email in emails[i])
                //        {
                //            sr2.WriteLine("<td>" + email + "</td>");
                //        }
                //        sr2.WriteLine("<tr>");
                //    }
                //    sr2.WriteLine("</table>");
                //    sr2.WriteLine("</div>");
                //    sr2.WriteLine("</body>");
                //    sr2.WriteLine("</html>");
                //}

                gatherFinalData(hostels, links, emails);
                //exportToExcel.export(resultData);
            }
            else
            {
                Logger.writeLog(txtConsole,"Hostelworld does not list " + inputCityString + ", " + inputCountryString + @"!");
            }
                
            }
            exportToExcel.export(resultData, appVars.outputLoc);
            
            
            
        }
            string cityCountryChoice = "city";
            resultsScreen showResultsWindow = new resultsScreen();
            if (radioCity.Checked == true)
                {
                    cityCountryChoice = selectedCity;
                }
            else if (radioCountry.Checked == true)
                {
                    cityCountryChoice = selectedCountry;
                }
            showResultsWindow.setLabels(cityCountryChoice, txtOutputLoc.Text);
            showResultsWindow.ShowDialog();
            Application.Restart();
  
    }

        private void  btnScrape_Click(object sender, EventArgs e)
        {
            scrape();
            
        }

        private void comboBoxCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCity = comboBoxCity.SelectedItem.ToString();
            
        }

        

        private void txtOutputLoc_Click(object sender, EventArgs e)
        {
            outputLoc.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*" ;
            if(outputLoc.ShowDialog() == DialogResult.OK)
            {
                appVars.outputLoc = outputLoc.FileName;
                txtOutputLoc.Text = outputLoc.FileName;
            }
        }

       

     

        

        private void aPIKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            APIkey ak = new APIkey();
            ak.ShowDialog();
        }

        public static string ReplaceString(string str, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }


       

        

        

        private void createListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListForm displayList = new ListForm();
            displayList.Show();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
            


        }

        private void radioCountry_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxCity.Enabled = false;
        }

        private void radioCity_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxCity.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
