using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Pendulum
{
    /**
     * Az inverz inga eléréséhez tartozó ősosztály
     * */
    public abstract class APendulumAccession
    {
        protected bool isRightEnd;
        protected bool isLeftEnd;
        protected double goingDir;
        protected double angle;
        protected double position;
        protected object lockAttributes;
    
        public abstract double[] updateAnalogInput();

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

                return temp;
            }
        }

        /**
         * Az inga haladási irányát és sebességét lehet beállítani 
         * */
        public double GoingDir
        {
            set
            {
                lock (lockAttributes)
                {
                    this.goingDir = value;
                }
            }
        }
    }
}
