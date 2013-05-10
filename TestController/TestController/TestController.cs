using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Management;
using System.Windows.Forms;

namespace Controller
{
    /**
     * Egy kezdetleges szabályozót valósít meg
     * */
    public class TestController : AController
    {
        private bool run = true;
        private double reference;
        private UserInterface ui;

        public TestController(IProcess _Process) : base(_Process)
        {
            ui = new UserInterface();
            reference = 2.5;
            ui.textBox1.Text = reference.ToString();
        }

        /**
         * A fgv beolvassa a textbox-ok értékét a felhasználói felületről
         * Referencia értékre biztosítja, hogy 0 és 5 között legyen
         * */
        private void getInput()
        {
            reference = Convert.ToDouble(ui.textBox1.Text);
            reference = reference < 0 ? 0 : reference;
            reference = reference > 5 ? 5 : reference;
        }

        /**
         * A szabályozási folyamatért felelős fgv
         * */
        public override void Run(APresenter _in)
        {
            run = true;
            getInput();

            // state[0] - Angle
            // state[1] - Position
            double[] state = Process.get();
            double epsilon = 0.001;
            double[] u = new double[] { 0.0 };

            while (run && !(state[1] > (reference - epsilon) && state[1] < (reference + epsilon)))
            {
                state = Process.get();
                _in.updateDraw(state);

                // Ha a referenciánál kisebb, akkor jobbra megy, ha nagyobb, akkor (lassabban) balra
                if (state[1] <= (reference - epsilon))
                {
                    u[0] = 1.0;
                }
                if (state[1] > (reference + epsilon))
                {
                    u[0] = -0.3;
                }
                Process.set(u);
                _in.updateLog(new string[] { DateTime.Now.ToString("HH:mm:ss.fff"), state[0].ToString("f5"), state[1].ToString("f5"), u[0].ToString("f5") });

                Thread.Sleep(25);
                state = Process.get();
            }

            Process.set(new double[] { 0.0 });
             
            for (int i = 0; i < 5; i++)
            {
                state = Process.get();
                Thread.Sleep(25);
            }

        }

        public override UserControl getInterface()
        {
            return ui;
        }

        /**
         * Leállítja a folyamat futását
         * */
        public override void Stop()
        {
            run = false;
        }
    }
}
