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
    /**
     * Az alkalmazás fő ablaka
     * */
    public partial class Form1 : Form
    {
        //AssemblyPicker adja a felületet a megfelelő assembly-k kiválasztásához
        public AssemblyPicker pck;

        //APresenter adja a felületet a folyamat eredményének megjelenítéséhez
        private APresenter pres;

        //A Controller paramétereinek beállításáre szolgáló felület
        private UserControl UI;

        //Settings menü-t megvalósító ablak
        private Settings sett;

        //A futtatásra kész állapotot jelöli
        bool runnable = false;

        public Form1()
        {
            InitializeComponent();
            sett = new Settings(this);

            restoreConfigs();

            newPicker();
        
            saveFileDialog1.InitialDirectory = @"D:\";

            //A szabályozó futása egy háttérszálon történik
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.WorkerSupportsCancellation = true;
        }
        /**
         * restoreConfigs
         * A config fájlból beolvasva beállítja az elmentet beállításokat
         * config fájl formátuma kulcsszó és érték kettősponttal elválasztva
         */
        private void restoreConfigs()
        {
            string[] config;
            try
            {
                config = System.IO.File.ReadAllLines(@"Config\config");
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
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(@"Config");
            }
            catch (FileNotFoundException)
            {
                File.Create(@"Config\config");
            }
        }
        /**
         *  newPicker
         *  Új összeállítás kiválasztásakor létrehozza az erre szolgáló felületet
         */
        private void newPicker()
        {
            pck = new AssemblyPicker(this);
            pck.Location = new System.Drawing.Point(10, 20);
            pck.Size = new System.Drawing.Size(440, 376);
            pck.TabIndex = 2;
            this.groupBox1.Controls.Add(pck);
        }

        /**
         * Ha kiválasztottuk a megfelelő assembly-ket, akkor a controller user interface-e jelenik meg, és a folyamat megjelenítője
         * Az AssemblyPicker osztályból hívódik meg
         * */
        public void controllerSelected(UserControl _inputUI,APresenter _inputPres)
        {
            pck.Dispose();
            UI = _inputUI;
            UI.Location = new System.Drawing.Point(10, 20);
            UI.Size = new System.Drawing.Size(440, 376);
            UI.TabIndex = 2;
            this.groupBox1.Controls.Add(UI);
            groupBox1.Text = "Controller";

            groupBox2.Hide();
            pres = _inputPres;
            pres.Location = new System.Drawing.Point(10, 0);
            this.splitContainer1.Panel2.Controls.Add(pres);

            runnable = true;
        }

        /**
         * Start gomb listener-e. 
         * Ha kiválasztottuk az assembly-ket, és azok együttműködésre képesek, és nem fut még a háttérszál, akkor elindítja
         */
        private void button1_Click(object sender, EventArgs e)
        {
            if (runnable)
            {
                if (backgroundWorker1.IsBusy == false)
                {
                    pres.reset();
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        /**
         * Egy delegate fgv a háttérszál számára
         */
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Management.Controller != null)
            {
                Management.Controller.Run(pres);
            }
        }

        /**
         * Stop gomb listener-e
         * Leállítja a háttérszál működését
         * */
        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            if (Management.Controller != null)
            {
                Management.Controller.Stop();
            }
        }

        /**
         * Exit menüpont listener-e
         * */
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /**
         * A New menüpont listener-e
         * Előhozza az assembly választó felületet, új összeállítást biztosítva
         * */
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Dispose();
            Management.Clear();
            pres.Dispose();
            newPicker();
            groupBox2.Show();
            runnable = false;
        }

        /**
         * Kilépés előtt fut le
         * Elmenti a beállítások értékeit
         * */
        private void Form1_FormClosing(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"Config"))
            {
                System.IO.Directory.CreateDirectory(@"Config");
            }
            using (StreamWriter sw = new System.IO.StreamWriter(@"Config\config"))
            {
                sw.WriteLine("Controller:{0}", AssemblyPicker.defaultControllerFolder );
                sw.WriteLine("Logger:{0}", AssemblyPicker.defaultLoggerFolder);
                sw.WriteLine("Connection:{0}", AssemblyPicker.defaultConnectionFolder);
                sw.WriteLine("Accession:{0}", AssemblyPicker.defaultAccessionFolder);
                sw.WriteLine("Browse:{0}", AssemblyPicker.defaultPickFolder);
            }
        }

        /**
         * A Settings menüpont listener-e
         * Előhozza a Settings ablakot
         * */
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sett.Show();
        }
    }
}
