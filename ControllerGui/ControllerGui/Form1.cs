using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Management;

namespace ControllerGui
{
    public partial class Form1 : Form
    {
        public AssemblyPicker pck;
        private PendulumPresenter pres; 

        public Form1()
        {
            InitializeComponent();
            pck = new AssemblyPicker(this);
            pres = new PendulumPresenter();

            pres.Location = new System.Drawing.Point(10,0);
            this.splitContainer1.Panel2.Controls.Add(pres);

            pck.Location = new System.Drawing.Point(10,20);
            pck.Size = new System.Drawing.Size(440, 376);
            pck.TabIndex = 2;
            this.groupBox1.Controls.Add(pck);

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        public void controllerSelected(UserControl _input)
        {
            pck.Dispose();
            UserControl UI = _input;
            UI.Location = new System.Drawing.Point(10, 20);
            UI.Size = new System.Drawing.Size(440, 376);
            UI.TabIndex = 2;
            this.groupBox1.Controls.Add(UI);
            groupBox1.Text = "Controller";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Management.Controller.Run(pres);
        }

        private double put = 0.0;
        private void button2_Click(object sender, EventArgs e)
        {
            //backgroundWorker1.CancelAsync();
            pres.drawableRectangle1.add(put);
            put = put + 0.3;
        }
    }
}
