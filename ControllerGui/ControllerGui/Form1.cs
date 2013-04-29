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
        public Form1()
        {
            InitializeComponent();
            AssemblyPicker pck = new AssemblyPicker();
            pck.Location = new System.Drawing.Point(22, 0);
            pck.Size = new System.Drawing.Size(440, 400);
            pck.TabIndex = 2;
            this.splitContainer1.Panel1.Controls.Add(pck);

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Chooser alma = new Chooser();
            alma.Location = new System.Drawing.Point(22, 39);
            alma.Name = "alma";
            alma.Size = new System.Drawing.Size(440, 270);
            alma.TabIndex = 2;
            this.splitContainer1.Panel1.Controls.Add(alma);
        }
    }
}
