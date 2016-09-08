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
    public partial class resultsScreen : Form
    {
        public resultsScreen()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void setLabels(string location, string outputLoc)
        {
            labelLocation.Text = location;
            labelOutputLoc.Text = outputLoc;
        }
    }
}
