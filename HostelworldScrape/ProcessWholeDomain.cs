using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.Windows.Forms;
using Newtonsoft;

namespace HostelworldScrape
{
    class ProcessWholeDomain
    {
        public static List<string> getEmailAddresses(string domain)
        {
            try
            {
                if (!domain.Contains("hostelworld"))
            {
                //Logger.writeLog(Form1.txtConsole,"Domain: " + domain);
                List<string> emailAddresses = new List<string>();
                List<string> linksInPage = new List<string>();


                HtmlAgilityPack.HtmlDocument originalDoc = new HtmlWeb().Load(domain);
                Uri workingPage = new Uri(domain);
                //Logger.writeLog(Form1.txtConsole,workingPage);
                //Logger.writeLog(Form1.txtConsole,"Scheme :" + workingPage.Scheme);
                if (workingPage.Scheme == "http")
                {
                    string originalDomain = workingPage.Host;

                    if (originalDomain.Contains("www."))
                    {
                        originalDomain = originalDomain.Replace("www.", "");
                    }
                    //Logger.writeLog(Form1.txtConsole,"originalDomain: " + originalDomain);
                    HtmlAgilityPack.HtmlNodeCollection links = originalDoc.DocumentNode.SelectNodes("//a[@href]");
                    //TESTING DOMAIN CHECK COUNTER
                    int domainCheck = 0;
                    
                    foreach (HtmlNode link in links)
                    {
                            
                                if (!originalDomain.EndsWith(@"/") && !link.Attributes["href"].Value.ToString().StartsWith(@"/"))
                                {
                                    originalDomain = originalDomain + @"/";
                                }
                                
                                

                            

                        domainCheck++;
                        if(domainCheck < 40)
                            {
                                //Logger.writeLog(Form1.txtConsole,"DOMAINCHECK: " + domainCheck);
                                if (link.Attributes["href"].Value.ToString().Contains(originalDomain) || link.Attributes["href"].Value.ToString().StartsWith(@"/") || link.Attributes["href"].Value.ToString().Contains(@"mailto:"))
                                {
                                    if (link.Attributes["href"].Value.ToString().StartsWith(@"/"))
                                    {
                                       
                                            string origDShortened = originalDomain;
                                        if (originalDomain.EndsWith(@"/"))
                                        {
                                            originalDomain = originalDomain.Replace(@"/", "");
                                        }
                                        linksInPage.Add(originalDomain + link.Attributes["href"].Value);
                                        //Logger.writeLog(Form1.txtConsole,"ORIGINAL DOMAIN");
                                        //Logger.writeLog(Form1.txtConsole,originalDomain + link.Attributes["href"].Value);
                                        //Logger.writeLog(Form1.txtConsole,"ORIGINAL DOMAIN");
                                    }
                                    if (link.Attributes["href"].Value.ToString().StartsWith(@"mailto:"))
                                    {
                                        linksInPage.Add(link.Attributes["href"].ToString().Replace(@"mailto:", ""));
                                    }
                                    else
                                    {
                                        linksInPage.Add(link.Attributes["href"].Value);
                                    }
                                    
                                }
                                else
                                {
                                    //Logger.writeLog(Form1.txtConsole,link.Attributes["href"].Value);
                                    //Logger.writeLog(Form1.txtConsole,"does not contain " + originalDomain);
                                    //Logger.writeLog(Form1.txtConsole,);
                                }
                         }
                        
                    }

                    List<string> extensionList = new List<string>();

                    string extensions = File.ReadAllText(@"fileExtensions.json");
                    var jObject = Newtonsoft.Json.Linq.JObject.Parse(extensions);
                    
                    

                    
                    foreach (string relLink in linksInPage)
                    {
                        

                        int counter = 0;
                        ////Logger.writeLog(Form1.txtConsole,relLink);

                        //Logger.writeLog(Form1.txtConsole,"REFERENCE: " + relLink);
                        if (relLink.Contains(@"mailto:"))
                        {
                            emailAddresses.Add(relLink.Replace(@"mailto:", ""));
                        }

                        if (!relLink.ToString().Contains(@"mailto:") && relLink.StartsWith("htt") )
                        {
                        Uri relUri = new Uri(relLink);
                        string pa = relUri.AbsoluteUri;
                        //Logger.writeLog(Form1.txtConsole,"Absolute URI: " + pa);
                            if (Path.HasExtension(pa))
                            {
                                
                                string input = Path.GetExtension(pa);
                                int index = input.IndexOf('?');
                                if(index > 0)
                                {
                                    input = input.Substring(0, index);
                                }
                                //where to check for valid extensions!
                                if(CheckExtension.validExtension(input))
                                    {
                                        //Logger.writeLog(Form1.txtConsole,"Valid");
                                    }
                                    else
                                    {
                                        //Logger.writeLog(Form1.txtConsole,"BAD");
                                    }
                            }

                            

                            
                            if (!relLink.Contains("mailto"))
                            {
                                
                                HtmlAgilityPack.HtmlDocument subDoc = new HtmlWeb().Load(relLink);
                                HtmlNodeCollection subCol = subDoc.DocumentNode.SelectNodes(@"//a");    
                                
                                

                                if (subDoc.DocumentNode.OuterHtml.Contains(@"mailto:"))
                                {
                                    //Logger.writeLog(Form1.txtConsole,"MAILTO");
                                    //Logger.writeLog(Form1.txtConsole,"MAILTO");
                                    //Logger.writeLog(Form1.txtConsole,"MAILTO");
                                    //Logger.writeLog(Form1.txtConsole,"MAILTO");
                                    //Logger.writeLog(Form1.txtConsole,"MAILTO");
                                }
                                foreach (string el in MailExtractor.ExtractEmails(subDoc.DocumentNode.OuterHtml))
                                {
                                       
                                    counter++;
                                    emailAddresses.Add(el);
                                };
                                if (counter == 0)
                                {
                                    //Logger.writeLog(Form1.txtConsole,"NO EMAIL");
                                }
                                else
                                {
                                    //Logger.writeLog(Form1.txtConsole,counter + " emails.");
                                }
                            }
                        }
                    }

                    emailAddresses = emailAddresses.Distinct().ToList();

                    
                }



                return emailAddresses;
            }
            else
            {
                List<string> str = new List<string>() {"NO SITE"};
                return str;
            }
            }
            catch(Exception ex)
            {
                //Logger.writeLog(Form1.txtConsole,ex.Message);
            }

            return null;
        }
    }
}
