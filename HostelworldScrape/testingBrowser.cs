using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HostelworldScrape
{
    public partial class testingBrowser : Form
    {
        public testingBrowser()
        {
            InitializeComponent();
        }

        private void testingBrowser_Load(object sender, EventArgs e)
        {
            webBrowserPreview.Navigate(appVars.googleUrl);
        }
    }
}
