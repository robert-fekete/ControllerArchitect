using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Management;

namespace Controller
{
    public class PIDController : AController
    {
        public double reference;
        private double Kp;
        private double Ki;
        private double Kd;

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

            ui.textBox1.Text = "2,5";     //ref
            ui.textBox2.Text = "0,5";     //Kp
            ui.textBox3.Text= "0,1";      //Ki
            ui.textBox4.Text = "150,0";   //Kd
        }

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

        private void getInput()
        {
            reference = Convert.ToDouble(ui.textBox1.Text);
            Kp = Convert.ToDouble(ui.textBox2.Text);
            Ki = Convert.ToDouble(ui.textBox3.Text);
            Kd = Convert.ToDouble(ui.textBox4.Text);
        }

        public override void Run()
        {}

        public override void Run(TextBox _in)
        {
            getInput();

            double[] state;
            double epsilon = 0.001;

            #region Init

            oldTime = DateTime.Now.Millisecond;
            state = Process.get();
            oldError = reference - state[1];

            #endregion

            while (!(state[1] > (reference - epsilon) && state[1] < (reference + epsilon)))
            {

                state = Process.get();
                newTime = DateTime.Now.Millisecond;
                double[] u = new double[] { 0.0 };

                double error = reference - state[1];
                I = I + error;
                double D = (error - oldError) / (newTime - oldTime);

                u[0] = clap(clap(Kp * error) + clap(Ki * I) + clap(Kd * D));
                Process.set(u);
                
                oldTime = newTime;
                oldError = error;
                Thread.Sleep(50);

                _in.AppendText("End");
            }
        }

        public override UserControl getInterface()
        {
            return ui;
        }
    }
}
