using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControllerArchitect
{
    public abstract class APendulumAccession
    {
        protected bool isRightEnd;
        protected bool isLeftEnd;
        protected double goingDir;
        protected double angle;
        protected double position;
        protected object lockAttributes;
    
        public abstract double[,] updateAnalogInput();

        public abstract bool[,] updateDigitalInput();

        public abstract void updateDigitalOutput();


        public bool RightEnd
        {
            get 
            {
                bool temp = false;
                lock (lockAttributes)
                {
                    temp = isRightEnd;
                }
                if (Management.debug) Console.WriteLine("APendulumAccession.RightEnd");
                return temp; 
            }
        }

        public bool LeftEnd
        {
            get
            {
                bool temp = false;
                lock (lockAttributes)
                {
                    temp = isLeftEnd;
                }
                if (Management.debug) Console.WriteLine("APendulumAccession.LeftEnd");
                return temp;
            }
        }

        public double Angle
        {
            get
            {
                double temp = 0.0;
                lock (lockAttributes)
                {
                    temp = angle;
                }
                if (Management.debug) Console.WriteLine("APendulumAccession.Angle"); 
                return temp;
            }
        }

        public double Position
        {
            get
            {
                double temp = 0.0;
                lock (lockAttributes)
                {
                    temp = position;
                }
                if (Management.debug) Console.WriteLine("APendulumAccession.Position");
                return temp;
            }
        }

        public double GoingDir
        {
            set
            {
                if (Management.debug) Console.WriteLine("APendulumAccession.GoingDir");
                lock (lockAttributes)
                {
                    this.goingDir = value;
                }
            }
        }
    }
}
