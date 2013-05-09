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
    public partial class Settings : Form
    {
        private Form1 owner;

        public Settings(Form1 _owner)
        {
            InitializeComponent();

            owner = _owner;

            folderBrowserDialog1.Description = "Choose some additional assemblies...";

            comboBox1.Items.Add("Select...");
            comboBox1.Items.Add("Controllers");
            comboBox1.Items.Add("Logger");
            comboBox1.Items.Add("Connection");
            comboBox1.Items.Add("Accession");
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 1: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultControllerFolder; break;
                case 2: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultLoggerFolder; break;
                case 3: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultConnectionFolder; break;
                case 4: folderBrowserDialog1.SelectedPath = AssemblyPicker.defaultAccessionFolder; break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            buttonApply.Enabled = true;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            buttonApply_Click(sender,e);
            owner.pck.refreshLists();
            this.Hide();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            owner.pck.refreshLists();
            this.Hide();
        }

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
        }
    }
}
