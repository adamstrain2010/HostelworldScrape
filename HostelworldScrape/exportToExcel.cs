using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace HostelworldScrape
{
    class exportToExcel
    {
        public static void export(List<Hostel> dataIn, string outputLoc)
        {
            using (StreamWriter sw = new StreamWriter(outputLoc, true))
            {
                foreach(Hostel outputHostel in dataIn)
                {
                    outputHostel.emails = outputHostel.emails.Distinct().ToList();
                    foreach(string email in outputHostel.emails)
                    {
                        sw.WriteLine(email);
                    }
                }
            }

            
                //foreach(Hostel testHos in dataIn)
                //{
                //    Logger.writeLog(Form1.txtConsole,"Name: " + testHos.name);
                //    Logger.writeLog(Form1.txtConsole,"Site: " + testHos.website);
                //    foreach(string testEmail in testHos.emails)
                //    {
                //        Logger.writeLog(Form1.txtConsole,"Email: " + testEmail);
                //    }
                //    Logger.writeLog(Form1.txtConsole,);
                //}
                //var excelApp = new Excel.Application();

                //excelApp.Visible = true;

                //excelApp.Workbooks.Add();

                //Excel._Worksheet worksheet = (Excel.Worksheet)excelApp.ActiveSheet;

                //worksheet.Cells[1, "A"] = "Hostel";
                //worksheet.Cells[1, "B"] = "Website";
                //worksheet.Cells[1, "C"] = "Email Addresses";
                //worksheet.Rows[1].Font.Bold = true;
                //var row = 1;
                //foreach (var dataRow in dataIn)
                //{
                //    row++;
                //    dataRow.emails = dataRow.emails.Distinct().ToList();
                //    //worksheet.Cells[row, "A"] = dataRow.name;
                //    //worksheet.Cells[row, "B"] = dataRow.website;
                
                //    //sw.WriteLine(@"**" + dataRow.name + @"**");
                //    for (int i = 0; i < dataRow.emails.Count(); i++)
                //    {
                        
                //        if (dataRow.emails.Count == 0)
                //        {
                //            //worksheet.Cells[row, "C"] = "NO EMAILS ON SITE";
                //        }
                //        else
                //        {
                //            //if(i != 0)
                //            //{
                //            //    row++;
                //            //}
                //            //worksheet.Cells[row, "C"] = dataRow.emails[i];
                            
                //            sw.WriteLine(dataRow.emails[i]);
                //        }
                //        }

                //    }
                //}
                //worksheet.Columns[3].WrapText = false;

                //worksheet.Columns[1].AutoFit();
                //worksheet.Columns[2].AutoFit();
                //worksheet.Columns[3].AutoFit();

            }
        }
}
