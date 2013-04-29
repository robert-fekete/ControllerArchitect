using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Management;
using System.Threading;

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

        public PIDController(IProcess _Process)
            : base(_Process)
        {
            reference = 2.5;
            Kp = 0.5;
            Ki = 0.1;
            Kd = 150.0;
            I = 0;
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

        public override void Run()
        {
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
                Console.WriteLine("u:{0}\tP:{1}\tI:{2}\tD:{3}", u[0], clap(Kp * error), clap(Ki * I), clap(Kd * D));

                oldTime = newTime;
                oldError = error;
                Thread.Sleep(50);
            }

            Console.WriteLine("Nyert");
        }
    }
}
