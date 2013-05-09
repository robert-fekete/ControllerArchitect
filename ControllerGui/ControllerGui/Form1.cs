using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Management;
using System.IO;

namespace ControllerGui
{
    public partial class Form1 : Form
    {
        public AssemblyPicker pck;
        private APresenter pres;
        private UserControl UI;
        private Settings sett;

        public Form1()
        {
            InitializeComponent();
            sett = new Settings(this);

            newPicker();

            restoreConfigs();

            saveFileDialog1.InitialDirectory = @"D:\";

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void restoreConfigs()
        {
            string[] config = System.IO.File.ReadAllLines(@"Config\config");
            foreach (var s in config)
            {
                string[] temp = s.Split(new char[] { ':' },2) ;
                if (temp[0] == "Controller" && temp[1] != "")
                {
                    AssemblyPicker.defaultControllerFolder = temp[1];
                }
                else if (temp[0] == "Logger" && temp[1] != "")
                {
                    AssemblyPicker.defaultLoggerFolder = temp[1];
                }
                else if (temp[0] == "Connection" && temp[1] != "")
                {
                    AssemblyPicker.defaultConnectionFolder = temp[1];
                }
                else if (temp[0] == "Accession" && temp[1] != "")
                {
                    AssemblyPicker.defaultAccessionFolder = temp[1];
                }
                else if (temp[0] == "Browse" && temp[1] != "")
                {
                    AssemblyPicker.defaultPickFolder = temp[1];
                }
            }
        }

        private void newPicker()
        {
            pck = new AssemblyPicker(this);
            pck.Location = new System.Drawing.Point(10, 20);
            pck.Size = new System.Drawing.Size(440, 376);
            pck.TabIndex = 2;
            this.groupBox1.Controls.Add(pck);
        }

        public void controllerSelected(UserControl _inputUI,APresenter _inputPres)
        {
            pck.Dispose();
            UI = _inputUI;
            UI.Location = new System.Drawing.Point(10, 20);
            UI.Size = new System.Drawing.Size(440, 376);
            UI.TabIndex = 2;
            this.groupBox1.Controls.Add(UI);
            groupBox1.Text = "Controller";

            groupBox2.Dispose();
            pres = _inputPres;
            pres.Location = new System.Drawing.Point(10, 0);
            this.splitContainer1.Panel2.Controls.Add(pres);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pck.button1.Enabled == true)
            {
                if (backgroundWorker1.IsBusy == false)
                {
                    pres.reset();
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Management.Controller.Run(pres);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Dispose();
            Management.Clear();
            newPicker();
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"Config"))
            {
                System.IO.Directory.CreateDirectory(@"Config");
            }
            using (StreamWriter sw = new System.IO.StreamWriter(@"Config\config"))
            {
                sw.WriteLine("Controller:{0}", AssemblyPicker.defaultControllerFolder);
                sw.WriteLine("Logger:{0}", AssemblyPicker.defaultLoggerFolder);
                sw.WriteLine("Connection:{0}", AssemblyPicker.defaultConnectionFolder);
                sw.WriteLine("Accession:{0}", AssemblyPicker.defaultAccessionFolder);
                sw.WriteLine("Browse:{0}", AssemblyPicker.defaultPickFolder);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sett.Show();
        }
    }
}
