using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HostelworldScrape
{
    class Logger
    {
        public static void writeLog(TextBox txt, string text)
        {
            txt.AppendText(System.Environment.NewLine + text);
        }
    }
}
         