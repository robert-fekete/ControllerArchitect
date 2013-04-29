using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControllerGui
{
    public partial class Chooser : MyUserControlls
    {
        public Chooser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "FASZASÁG KURVA JÓ\nSZÉTBASZATÓS\nNAGYON ADJA JÓ";
        }
    }
}
