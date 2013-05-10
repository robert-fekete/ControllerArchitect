using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Management;

namespace Controller
{
    /**
     * PID szabályozó implementálása
     * */
    public class PIDController : AController
    {
        public double reference;
        private double Kp;
        private double Ki;
        private double Kd;
        private bool run;

        private double I;
        private long oldTime;
        private long newTime;
        private double oldError;

        private UserInterface ui;

        public PIDController(IProcess _Process) : base(_Process)
        {
            reference = 2.5;
            Kp = 0.5;
            Ki = 0.1;
            Kd = 150.0;
            I = 0;

            ui=new UserInterface();

            ui.textBox1.Text = reference.ToString();     //ref
            ui.textBox2.Text = Kp.ToString();     //Kp
            ui.textBox3.Text= Ki.ToString();      //Ki
            ui.textBox4.Text = Kd.ToString();   //Kd
        }

        /**
         * A függvény a kapott értéket -1;1 tartományba vágja
         * */
        public double clap(double input)
        {
            double temp = input;
            if (input > 1.0)
            {
                temp = 1.0;
            }
            if (input < -1.0)
            {
                temp = -1.0;
            }
            return temp;
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
            ui.textBox1.Text = reference.ToString();
            Kp = Convert.ToDouble(ui.textBox2.Text);
            Ki = Convert.ToDouble(ui.textBox3.Text);
            Kd = Convert.ToDouble(ui.textBox4.Text);
        }

        /**
         * A szabályozási folyamatért felelős fgv
         * */
        public override void Run(APresenter _in)
        {
            getInput();

            // state[0] - Angle
            // state[1] - Position
            double[] state = Process.get();
            double epsilon = 0.001;

            #region Init

            oldTime = DateTime.Now.Millisecond;
            state = Process.get();
            oldError = reference - state[1];

            #endregion

            // Addig megy a szabályozási folyamat, amíg a referencia érték epsilon sugarú körébe nem kerül a pozíció
            while (run && !(state[1] > (reference - epsilon) && state[1] < (reference + epsilon)))
            {

                state = Process.get();
                _in.updateDraw(state);

                newTime = DateTime.Now.Millisecond;
                double[] u = new double[] { 0.0 };

                //PID logika
                double error = reference - state[1];
                I = I + error;
                double D = (error - oldError) / (newTime - oldTime);

                u[0] = clap(clap(Kp * error) + clap(Ki * I) + clap(Kd * D));

                if( Double.IsNaN(u[0]))
                {
                    u[0] = 0.0;
                }
                Process.set(u);
                _in.updateLog(new string[] { DateTime.Now.ToString("HH:mm:ss.fff"), state[0].ToString("f5"),state[1].ToString("f5"), u[0].ToString("f5") });
                
                oldTime = newTime;
                oldError = error;
                Thread.Sleep(50);
            }

            double[] stop = new double[] { 0.0 };
            Process.set(stop);
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
