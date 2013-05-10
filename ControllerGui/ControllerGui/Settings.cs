using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControllerGui
{
    /**
     * Settings menüpontban felugró, beállításokra szolgáló form
     * */
    public partial class Settings : Form
    {
        // Az alkalmazás fő ablaka
        private Form1 owner;

        public Settings(Form1 _owner)
        {
            InitializeComponent();

            owner = _owner;
            
            // Combobox feltöltése
            comboBox1.Items.Add("Select...");
            comboBox1.Items.Add("Controllers");
            comboBox1.Items.Add("Logger");
            comboBox1.Items.Add("Connection");
            comboBox1.Items.Add("Accession");
            comboBox1.SelectedIndex = 0;
        }

        /**
         * A comboboxban értékválasztás után fut le. Beállítja a mappa választó kiindulási pozícióját
         * */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 1: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultControllerFolder; break;
                case 2: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultLoggerFolder; break;
                case 3: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultConnectionFolder; break;
                case 4: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultAccessionFolder; break;
            }
            textBox1.Text = "";
        }

        /**
         * Browse gomb listener-e
         * Feldobja a mappa választó ablakot. és az eredményt a textbox-ba írja
         * */
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            buttonApply.Enabled = true;
        }

        /**
         * OK gomb listener-e
         * Az Apply listener-ére hív tovább, frissíti a listákat, majd elrejti az ablakot
         * */
        private void buttonOk_Click(object sender, EventArgs e)
        {
            buttonApply_Click(sender,e);
            owner.pck.refreshLists();
            this.Hide();
        }

        /**
         * A Cancel gomb listener-e
         * Elrejti az ablakot
         * */
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /**
         * Az Apply gomb listener-e
         * Ha nem üres a textbox értéke, akkor a comboboxnak megfelelő listához tartozó kiinduló mappát megváltoztatja
         * Érvényteleníti az Apply gombot, és frissíti a listákat
         * */
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 1: AssemblyPicker.defaultControllerFolder = textBox1.Text; break;
                    case 2: AssemblyPicker.defaultLoggerFolder = textBox1.Text; break;
                    case 3: AssemblyPicker.defaultConnectionFolder = textBox1.Text; break;
                    case 4: AssemblyPicker.defaultAccessionFolder = textBox1.Text; break;
                }
            }
            buttonApply.Enabled = false;
            owner.pck.refreshLists();
        }
    }
}
