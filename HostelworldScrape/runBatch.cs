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
    public partial class runBatch : Form
    {
        public runBatch()
        {
            InitializeComponent();
        }

        private void runBatch_Load(object sender, EventArgs e)
        {
            labelCountry.Text = appVars.staticBatch.country;
            foreach(string city in appVars.staticBatch.cities)
            {
                listBoxCities.Items.Add(city);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            batchProcess.process(appVars.staticBatch.cities, appVars.staticBatch.country);
        }
    }
}
