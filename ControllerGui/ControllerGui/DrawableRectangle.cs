using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ControllerGui
{
    public partial class DrawableRectangle : UserControl
    {
        private ArrayList data;

        public DrawableRectangle()
        {
            InitializeComponent();
            data = new ArrayList();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Rács
            for (int i = (20 + 15); i < (440 + 15); i+=20)
            {
                e.Graphics.DrawLine(Pens.White,new Point(i,15),new Point(i,390 + 15));
            }
            for (int j = (20 + 15); j < (390 + 15); j+=20)
            {
                e.Graphics.DrawLine(Pens.White,new Point(15,j),new Point(440 + 15,j));
            }

            //Koordináta tengelyek
            e.Graphics.DrawLine(Pens.Black, new Point(75, 15), new Point(75, 405));
            e.Graphics.DrawLine(Pens.Black, new Point(75, 15), new Point(80, 25));
            e.Graphics.DrawLine(Pens.Black, new Point(75, 15), new Point(70, 25));
            e.Graphics.DrawLine(Pens.Black, new Point(15, 315), new Point(455, 315));
            e.Graphics.DrawLine(Pens.Black, new Point(445, 310), new Point(455, 315));
            e.Graphics.DrawLine(Pens.Black, new Point(445, 320), new Point(455, 315));

            //Keret
            e.Graphics.DrawRectangle(Pens.Gray, 15, 15, 440, 390);

        }

        private class Record
        {
            private double value;
            private DateTime time;

            public Record(double _value, DateTime _time)
            {
                value = _value;
                time = _time;
            }

            public double Value { get { return value; } set { this.value = value; } }

            public DateTime Time 
            {
                get
                {
                    return time;
                }
                set
                {
                    time = value;
                }
            }
        }
    }
}
