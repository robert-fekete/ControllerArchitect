using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Pendulum
{
    /**
     * Grafikon rajzolást megvalósító osztály
     * */
    public partial class DrawableRectangle : UserControl
    {
        // Kirajzolandó pontok
        private List<Point> data;

        // Az időtengely felosztását adja, folyamatosan csökkentve, hogy ne fusson ki a grafikon a négyzetből
        private double ratio;

        // A felosztást számszerűsítő címkék
        private List<Label> labels;

        public DrawableRectangle()
        {
            InitializeComponent();
            data = new List<Point>();
            labels = new List<Label>();
            ratio = 1;

            // X tengely mértékegysége
            Label unit = new Label();
            unit.Text = "(ms)";
            unit.Location = new Point(445, 330);

            #region y tengely beosztása
            Label lbl1 = new Label();
            lbl1.Width = 10;
            lbl1.Location = new System.Drawing.Point(58, 248);
            lbl1.Text = "1";
            this.Controls.Add(lbl1);

            Label lbl2 = new Label();
            lbl2.Width = 10;
            lbl2.Location = new System.Drawing.Point(58, 188);
            lbl2.Text = "2";
            this.Controls.Add(lbl2);

            Label lbl3 = new Label();
            lbl3.Width = 10;
            lbl3.Location = new System.Drawing.Point(58, 128);
            lbl3.Text = "3";
            this.Controls.Add(lbl3);

            Label lbl4 = new Label();
            lbl4.Width = 10;
            lbl4.Location = new System.Drawing.Point(58, 68);
            lbl4.Text = "4";
            this.Controls.Add(lbl4);

            #endregion
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            #region Négyzetháló rács
            for (int i = (20 + 15); i < (440 + 15); i += 20)
            {
                e.Graphics.DrawLine(Pens.White, new Point(i, 15), new Point(i, 390 + 15));
            }
            for (int j = (20 + 15); j < (390 + 15); j += 20)
            {
                e.Graphics.DrawLine(Pens.White, new Point(15, j), new Point(440 + 15, j));
            }
            #endregion

            #region Y tengely

            //tengely
            e.Graphics.DrawLine(Pens.Black, new Point(75, 15), new Point(75, 405));

            //nyíl fej
            e.Graphics.DrawLine(Pens.Black, new Point(75, 15), new Point(80, 25));
            e.Graphics.DrawLine(Pens.Black, new Point(75, 15), new Point(70, 25));

            //Beosztás
            e.Graphics.DrawLine(Pens.Black, new Point(70, 255), new Point(80, 255));
            e.Graphics.DrawLine(Pens.Black, new Point(70, 195), new Point(80, 195));
            e.Graphics.DrawLine(Pens.Black, new Point(70, 135), new Point(80, 135));
            e.Graphics.DrawLine(Pens.Black, new Point(70, 75), new Point(80, 75));
            #endregion

            #region X tengely

            // tengely
            e.Graphics.DrawLine(Pens.Black, new Point(15, 315), new Point(455, 315));

            // nyíl fej
            e.Graphics.DrawLine(Pens.Black, new Point(445, 310), new Point(455, 315));
            e.Graphics.DrawLine(Pens.Black, new Point(445, 320), new Point(455, 315));

            // Beosztás és hozzá vezető egyenesek
            e.Graphics.DrawLine(Pens.Black, new Point(415, 320), new Point(415, 310));
            e.Graphics.DrawLine(Pens.LightGray, new Point(415, 15), new Point(415, 405));
            e.Graphics.DrawLine(Pens.Black, new Point(245, 320), new Point(245, 310));
            e.Graphics.DrawLine(Pens.LightGray, new Point(245, 15), new Point(245, 405));
            e.Graphics.DrawLine(Pens.Black, new Point(160, 320), new Point(160, 310));
            e.Graphics.DrawLine(Pens.LightGray, new Point(160, 15), new Point(160, 405));
            e.Graphics.DrawLine(Pens.Black, new Point(117, 320), new Point(117, 310));
            e.Graphics.DrawLine(Pens.LightGray, new Point(117, 15), new Point(117, 405));
            #endregion

            #region Keret
            e.Graphics.DrawRectangle(Pens.Gray, 15, 15, 440, 390);
            #endregion

            #region Görbe
            if (data.Count >= 2)
            {
                e.Graphics.DrawCurve(Pens.Red, data.ToArray());
            }
            #endregion
        }

        int firstTime = 0;

        /**
         * Egy pont hozzáadásáért felelős fgv
         * */
        public void add(double _in)
        {

            // Ha nem indult még el a kirajzolás
            if (data.Count == 0)
            {
                // Kezdő idő millisec-ben
                DateTime temp = DateTime.Now;
                firstTime = ((temp.Minute * 60) + temp.Second) * 1000 + temp.Millisecond;

                // 315-ben van az X tengely
                // 3 * 20 pixel egy osztás
                int value = 315 - Convert.ToInt32(_in * 3 * 20.0);
                data.Add(new Point(75, value));
            }
            // Ha egy pont le lett már rakva
            else if (data.Count == 1)
            {
                // Idő millisecben
                DateTime temp = DateTime.Now;
                int time = ((temp.Minute * 60) + temp.Second) * 1000 + temp.Millisecond;

                // Első és második beérkező adat között eltelt időt állandónak tekintve a következő adatokra nézve kiszámolja a felosztás arányát, ha 20 pixelt számol két adat közé
                ratio = 20.0 / (time - firstTime);

                // Kiszámolja a bekött pont idő és érték koordinátáját
                int t = Convert.ToInt32((time - firstTime) * ratio) + 75;
                int value = 315 - Convert.ToInt32(_in * 3 * 20.0);
                data.Add(new Point(t,value));

                // Létrehoz egy új címkét, jelezve a beosztás nagyságát
                Label tempLabel = new Label();

                // A 410. pixelnél lévő beosztáshoz számolja az értéket, hogy a fent kiszámolt arány mellett, hány millisec-et jelez. (410-nél van a beosztás, és 75-nél az Y tengely. 410-75=345)
                tempLabel.Text = Convert.ToInt32(345 / ratio).ToString();
                tempLabel.Width = 30;
                tempLabel.Location = new System.Drawing.Point(410, 330);
                this.Controls.Add(tempLabel);
                labels.Add(tempLabel);
            }
            // Ha több, mint egy pont van
            else
            {
                // Idő kiszámolása millisecben (csak a perc, másodperc és ezredmásodperc számít)
                DateTime temp = DateTime.Now;
                int time = ((temp.Minute * 60) + temp.Second) * 1000 + temp.Millisecond;

                // Iőd és érték koordináták kiszámolása
                int t = Convert.ToInt32((time - firstTime) * ratio) + 75;
                int value = 315 - Convert.ToInt32(_in * 3 * 20.0);
                data.Add(new Point(t,value));

                // Megkeresi az utolsó pontot a tömbben (.NET 4 Last() fgv helyett)
                Point last = new Point();

                foreach (var point in data)
	            {
		            last = point;
	            }

                // Ha az utolsó pont túl lógna a 415. pixelen, akkor felezzük az arányt, és mindegyik pont relatív X koordinátáját
                if (last.X > 415)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        // 75. pixelnél van az Y tengely
                        data[i] = new Point(Convert.ToInt32((data[i].X - 75) / 2.0) + 75, data[i].Y);
                    }
                    ratio = ratio / 2;

                    foreach (var l in labels)
                    {
                        // A címkéket is csúsztatjuk
                        l.Location = new Point(Convert.ToInt32((l.Location.X - 75) / 2.0) + 75, l.Location.Y);
                    }

                    // Kiszámolja, hogy milyen értékű címke tartozik most a 410-ös pixelhez (410-nél van a beosztás, és 75-nél az Y tengely. 410-75=345)
                    Label tempLabel = new Label();
                    tempLabel.Text = Convert.ToInt32(345 / ratio).ToString();
                    tempLabel.Width = 40;
                    tempLabel.Location = new System.Drawing.Point(410, 330);
                    this.Controls.Add(tempLabel);
                    labels.Add(tempLabel);
                }
            }
            this.Invalidate();
        }

        public void Clear()
        {
            ratio = 1;
            data.Clear();
            foreach (var l in labels)
            {
                l.Dispose();
            }
            labels.Clear();
        }
    }
}
