using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Management;
using Microsoft.VisualBasic.PowerPacks;

namespace ControllerGui
{
    public partial class PendulumPresenter : APresenter
    {
        private int distance;
        private int deviceDistance = 5;
        private int length;

        public PendulumPresenter()
        {
            InitializeComponent();
            distance = lineShape3.StartPoint.X - lineShape2.StartPoint.X;
            length = lineShape4.EndPoint.Y - lineShape4.StartPoint.Y;
            this.tabControl1.SelectedIndex = 2;
        }

        public override void updateLog(string[] _input)
        {
            MethodInvoker methodInvokerDelegate = delegate()
            {
                listView1.Items.Add(new ListViewItem(_input));
                //textBox1.AppendText(_input); 
            };

            //This will be true if Current thread is not UI thread.
            if (this.InvokeRequired)
                this.Invoke(methodInvokerDelegate);
            else
                methodInvokerDelegate();
        }

        public override void updateDraw(double[] values)
        {
            int cartX = lineShape2.StartPoint.X + Convert.ToInt32(values[1] / deviceDistance * distance) - 25;
            int lineX1 = cartX + 24;
            int lineY1 = lineShape4.StartPoint.Y;
            int lineX2 = Convert.ToInt32(Math.Sin(values[0] / 180 * Math.PI) * length) + lineX1;
            int lineY2 = lineY1 - Convert.ToInt32(Math.Cos(values[0] / 180 * Math.PI) * length);

            MethodInvoker methodInvokerDelegate = delegate()
            {
                rectangleShape2.Location = new System.Drawing.Point(cartX, rectangleShape2.Location.Y);
                lineShape4.StartPoint = new System.Drawing.Point(lineX1, lineY1);
                lineShape4.EndPoint = new System.Drawing.Point(lineX2, lineY2);
                ovalShape1.Location = new System.Drawing.Point(lineX2 - 7, lineY2 - 7);
            };

            //This will be true if Current thread is not UI thread.
            if (this.InvokeRequired)
                this.Invoke(methodInvokerDelegate);
            else
                methodInvokerDelegate();
        }
    }
}
