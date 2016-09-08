using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace HostelworldScrape
{
    class ProcessDomain
    {
        public static List<string> getEmails(string domain)
        {
            //stores final email list
            List<string> emailsOutput = new List<string>();
            //stores links in first page
            List<string> links = new List<string>();
            //stores links of children
            List<string> childLinks = new List<string>();

            try
            {
                string pageToScan = domain;
            //page to scan converted to URI object
            Uri uriPageToScan = new Uri(pageToScan);
            //HTML document from URL
            HtmlAgilityPack.HtmlDocument scannedPage = new HtmlWeb().Load(pageToScan);
            //all link nodes in original page
            HtmlNodeCollection col = scannedPage.DocumentNode.SelectNodes(@"//a");


            //emails extracted from first page
            List<string> emails = ExtractEmails(scannedPage.DocumentNode.OuterHtml);
            //put emails into final email list
            //foreach (string pageEmail in emails)
            //{
            //    emailsOutput.Add(pageEmail);
            //}

            //iterate through links from first page
            foreach (HtmlNode link in col)
            {
                //check if current link has any attributes, if not ignore, else..
                if (link.HasAttributes)
                {
                    //check for href attribute, if doesnt exist, return null, if value == null ignore, else..
                    //check for ?subject etc
                    if (link.GetAttributeValue("href", null) != null)
                    {
                        //get href value
                        string val = link.Attributes["href"].Value;
                        //if href starts with 'mailto:' remove 'mailto:' keep rest of href add to final email list
                        if (val.StartsWith(@"mailto:"))
                        {   
                            //TESTING
                            val = val.Replace(@"mailto:", "");
                            if (val.Contains(@"?"))
                            {
                                val = val.Remove(val.IndexOf(@"?"));
                            }
                            emailsOutput.Add(val);
                        }
                        //if href doesn't start with 'mailto:'..
                        else
                        {
                            //if href starts with forward slash and has content concatenate the domain with the href and add to links list
                            if (val.StartsWith(@"/") && val.Length > 1 && !val.Contains(".jpeg") && !val.Contains(".jpg") && !val.Contains(".png") && !val.Contains(".gif") && !val.Contains(@"example.com"))
                            {
                                links.Add(uriPageToScan.GetLeftPart(UriPartial.Authority) + val);
                            }
                            //if href does NOT start with forward slash AND has content AND does NOT start with 'http' AND does NOT start with 'tel' add to links list
                            else if (!val.StartsWith(@"/") && val.Length > 0 && !val.StartsWith("http") && !val.StartsWith(@"tel") && !val.Contains(".jpeg") && !val.Contains(".jpg") && !val.Contains(".png") && !val.Contains(".gif") && !val.Contains(@"example.com"))
                            {
                                links.Add(uriPageToScan.GetLeftPart(UriPartial.Authority) + @"/" + val);
                            }
                            else if (!val.StartsWith(@"/") && val.Length > 0 && !val.StartsWith(@"tel") && !val.Contains(".jpeg") && !val.Contains(".jpg") && !val.Contains(".png") && !val.Contains(".gif") && !val.Contains(@"example.com"))
                            {
                                links.Add(val);
                            }
                        }
                    }
                }
            }
            //foreach (string email in MailExtracter.ExtractEmails(scannedPage.ToString()))
            //{
            //    emailsOutput.Add(email);
            //}

            //set counter at 0

            //iterate therough list of links in links list from original page
            
                for(int i = 0; i < 40; i++)
                {
                
                //Load inner link into HTML document
                
                   
                        HtmlAgilityPack.HtmlDocument innerDoc = new HtmlWeb().Load(links[i]);
                        
                        //Logger.writeLog(Form1.txtConsole,innerDoc.DocumentNode.OuterHtml);
                        //Get list of links in inner document
                        List<string> runningEmails = ExtractEmails(innerDoc.DocumentNode.OuterHtml);
                        foreach(string innerEmail in runningEmails)
                        {
                            if(!innerEmail.Contains(".jpeg") && !innerEmail.Contains(".jpg") && !innerEmail.Contains(".png") && !innerEmail.Contains(".gif") && !innerEmail.Contains(@"example.com"))
                        {
                            emailsOutput.Add(innerEmail);
                        }
                            
                        }

                        HtmlNodeCollection innerCol = innerDoc.DocumentNode.SelectNodes(@"//a");
                        //if list has links..
                        if (innerCol != null)
                        {
                            //iterate through links in list
                            foreach (HtmlNode innerLink in innerCol)
                            {
                                //if link from innerpage has any attributes, then..
                                if (innerLink.HasAttributes)
                                {
                                    //check if this link has a href value, if not set to null, skip if null
                                    if (innerLink.GetAttributeValue("href", null) != null)
                                    {
                                        //get href value
                                        string val = innerLink.Attributes["href"].Value;
                                        if (val.StartsWith(@"mailto:"))
                                        {
                                            val = val.Replace(@"mailto:", "");
                                            if (val.Contains(@"?"))
                                            {
                                                val = val.Remove(val.IndexOf(@"?"));
                                            }
                                            if (!emailsOutput.Contains(val) && !val.Contains(".jpeg") && !val.Contains(".jpg") && !val.Contains(".png") && !val.Contains(@"example.com"))
                                            {
                                                emailsOutput.Add(val);
                                            }

                                        }
                                        else
                                        {
                                            if (val.StartsWith(@"/") && val.Length > 1 && !val.Contains(".jpeg") && !val.Contains(".jpg") && !val.Contains(".png") && !val.Contains(@"example.com"))
                                            {
                                                childLinks.Add(uriPageToScan.GetLeftPart(UriPartial.Authority) + val);
                                            }
                                            if (!val.StartsWith(@"/") && val.Length > 0 && !val.StartsWith("http") && !val.StartsWith(@"tel") && !val.Contains(".jpeg") && !val.Contains(".jpg") && !val.Contains(".png") && !val.Contains(@"example.com"))
                                            {
                                                childLinks.Add(uriPageToScan.GetLeftPart(UriPartial.Authority) + @"/" + val);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
            }
            catch
            {

            }
            
            emailsOutput =  emailsOutput.Distinct().ToList();
            return emailsOutput;
        }

        public static List<string> ExtractEmails(string inFilePath)
            {
                List<string> foundEmails = new List<string>();

                string data = inFilePath; //read File 
                //instantiate with this pattern 
                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
                //find items that matches with our pattern
                MatchCollection emailMatches = emailRegex.Matches(data);

                StringBuilder sb = new StringBuilder();

                foreach (Match emailMatch in emailMatches)
                {
                    foundEmails.Add(emailMatch.Value);
                }
                //store to file

                return foundEmails;
            }
    }
}
