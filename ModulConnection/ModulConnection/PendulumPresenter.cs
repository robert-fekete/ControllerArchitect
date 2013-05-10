using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Management;
using Microsoft.VisualBasic.PowerPacks;

namespace Pendulum
{
    /**
     * Megjelenítő felület a folyamathoz
     * Egy tab-os felület log-gal, animációval és grafikonnal
     * */
    public partial class PendulumPresenter : APresenter
    {
        // A megjelenítőn a befutható pálya hossza
        private int distance;

        // Az eszközben a befutható pálya hossza
        private int deviceDistance = 5;

        // A pendulum rúd hossza
        private int length;

        public PendulumPresenter()
        {
            InitializeComponent();
            distance = lineShape3.StartPoint.X - lineShape2.StartPoint.X;
            length = lineShape4.EndPoint.Y - lineShape4.StartPoint.Y;
        }

        /**
         * Logger felület frissítése
         * Delegate-el és invoke-al van megoldva, hogy ha nem a GUI szálból hívjuk, akkor is tudja frissíteni a felületet (invoke)
         * */
        public override void updateLog(string[] _input)
        {
            MethodInvoker methodInvokerDelegate = delegate()
            {
                listView1.Items.Add(new ListViewItem(_input));
            };

            // Ha az aktuális szál nem az UI szál akkor igaz
            if (this.InvokeRequired)
                this.Invoke(methodInvokerDelegate);
            else
                methodInvokerDelegate();
        }

        /**
         * Frissíti az animációt, és a grafikont
         * Delegate-el és invoke-al van megoldva, hogy ha nem a GUI szálból hívjuk, akkor is tudja frissíteni a felületet (invoke)
         * */
        public override void updateDraw(double[] values)
        {
            // Kiszámolja a kocsi pozícióját, a kar kezdőpozícióját (line1) és a kar végpozícióját (line2) a szögből
            int cartX = lineShape2.StartPoint.X + Convert.ToInt32((values[1] / deviceDistance) * distance) - 25;
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
                drawableRectangle1.add(values[1]);
            };

            // Ha az aktuális szál nem az UI szál akkor igaz
            if (this.InvokeRequired)
                this.Invoke(methodInvokerDelegate);
            else
                methodInvokerDelegate();
        }

        // Alaphelyzetbe állítja a megjelenítőt
        public override void reset()
        {
            listView1.Items.Clear();
            drawableRectangle1.Clear();
        }
    }
}
