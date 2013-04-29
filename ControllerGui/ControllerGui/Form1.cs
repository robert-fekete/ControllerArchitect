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
    public partial class Form1 : Form
    {
        public AssemblyPicker pck;

        public Form1()
        {
            InitializeComponent();
            pck = new AssemblyPicker(this);
            pck.Location = new System.Drawing.Point(22, 0);
            pck.Size = new System.Drawing.Size(440, 376);
            pck.TabIndex = 2;
            this.splitContainer1.Panel1.Controls.Add(pck);

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        public void controllerSelected(UserControl _input)
        {
            pck.Dispose();
            UserControl UI = _input;
            UI.Location = new System.Drawing.Point(22, 0);
            UI.Size = new System.Drawing.Size(440, 376);
            UI.TabIndex = 2;
            this.splitContainer1.Panel1.Controls.Add(UI);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Management.Controller.Run(textBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            textBox1.AppendText("SZARSZARSZAR\n");
        }
    }
}
