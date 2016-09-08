using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft;

namespace HostelworldScrape
{
    public partial class ListForm : Form
    {
        public ListForm()
        {
            InitializeComponent();
        }

        public static string fullList = null;
        Dictionary<string, List<string>> countryCities;
        string selectedCountry;
        List<String> selectedCities = new List<string>();

        private void ListForm_Load(object sender, EventArgs e)
        {
            List<string> countries = new List<string>();
            using (StreamReader sr = new StreamReader(@"C:\Users\adams\Desktop\countries.json"))
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
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxSelectedCities.Items.Clear();
            listBoxCities.Items.Clear();
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
                        listBoxCities.Items.Add(city);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(listBoxSelectedCities.Items.Count > 20)
            {
                MessageBox.Show("Too many cities selected, google will block the IP");
            }
            else
            {
                listBoxSelectedCities.Items.Add(listBoxCities.SelectedItem);
                
            }
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listBoxSelectedCities.Items.Remove(listBoxSelectedCities.SelectedItem);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            listBoxSelectedCities.Items.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach(string city in listBoxSelectedCities.Items)
            {
                selectedCities.Add(city);   
            }

            BatchCountries selectedBatch = new BatchCountries();
            selectedBatch.country = selectedCountry;
            selectedBatch.cities = selectedCities;

            appVars.staticBatch = selectedBatch;
            runBatch batchGoScreen = new runBatch();
            batchGoScreen.Show();
        }
    }
}
