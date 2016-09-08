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
    public partial class APIkey : Form
    {
        public APIkey()
        {
            InitializeComponent();
        }

        private void APIkey_Load(object sender, EventArgs e)
        {
            txtAPIKey.Text =  Properties.Settings.Default.APIkey;
        }
    }
}
