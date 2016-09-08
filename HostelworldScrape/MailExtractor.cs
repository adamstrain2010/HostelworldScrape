using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HostelworldScrape
{
    class MailExtractor
    {
        public static List<string> ExtractEmails(string inputText)
        {
            List<string> outputList = new List<string>();
            string data = inputText; //read File 
                                     //instantiate with this pattern 
            List<string> mailToEmails = new List<string>();
            if (data.Contains(@"mailto:"))
            {
                for(int i =0; i < 20; i++)
                {
                    int indexOfMailTo = data.IndexOf(@"mailto:") + 7;
                    int mailToEmailIndex = data.IndexOf("\"", indexOfMailTo);
                    int length = mailToEmailIndex - indexOfMailTo;
                    string emailFromMailTo = data.Substring(indexOfMailTo, length);
                    outputList.Add(emailFromMailTo);
                }
            }
            //foreach(string email in mailToEmails)
            //{
            //    //Logger.writeLog(Form1.txtConsole,"MAIL TO EMAIL: " + email);
            //    //Logger.writeLog(Form1.txtConsole,"MAIL TO EMAIL: " + email);
            //    //Logger.writeLog(Form1.txtConsole,"MAIL TO EMAIL: " + email);
            //    //Logger.writeLog(Form1.txtConsole,"MAIL TO EMAIL: " + email);
            //}
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            //find items that matches with our pattern
            MatchCollection emailMatches = emailRegex.Matches(data);

            StringBuilder sb = new StringBuilder();

            foreach (Match emailMatch in emailMatches)
            {
               
                sb.AppendLine(emailMatch.Value);
            }
            //store to file
            outputList.Add(sb.ToString());
            return outputList;
        }
    }
}
