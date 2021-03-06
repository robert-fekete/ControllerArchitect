﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Pendulum
{
    /**
     * Pendulum kezdetleges szimulálására, a keretrendszer tesztelését segítő osztály
     * */
    public class TestAccession : APendulumAccession
    {
        private Thread sim;

        public TestAccession()
        {
            angle = 0;
            position = 0;
            goingDir = 0.0;
            isRightEnd = false;
            isLeftEnd = false;
            lockAttributes = new object();
            sim = new Thread(simulate);
            sim.IsBackground = true;
            sim.Start();

        }

        /**
         * Interface egyezés miatt
         * */
        public override double[] updateAnalogInput()
        {
            return new double[]{angle,position};

        }

        /**
         * Interface egyezés miatt
         * */
        public override bool[,] updateDigitalInput()
        {
            return new bool[2,1] { {isLeftEnd},{isRightEnd}};
        }

        /**
         * Interface egyezés miatt
         * */
        public override void updateDigitalOutput()
        {

        }

        /**
         * Ez a fgv felelős magáért a szimulációért
         * */
        private void simulate()
        {
            double tempPos = 0;
            double tempAngle = 0;
            double tempGoing = 0;
            double velocityAdjustmentConstant = 0.2;

            while (true)
            {
                lock (lockAttributes)
                {
                    tempPos = position;
                    tempAngle = angle;
                    tempGoing = goingDir;
                }
                
                if ((tempPos + velocityAdjustmentConstant * tempGoing) < 0.0)
                {
                    tempPos = 0.0;
                    isLeftEnd = true;
                }
                else if ((tempPos + velocityAdjustmentConstant * tempGoing) > 5.0)
                {
                    tempPos = 5.0;
                    isRightEnd = true;
                }
                else
                {
                    isLeftEnd = false;
                    isRightEnd = false;
                    if (tempGoing != 0)
                    {
                        tempPos += velocityAdjustmentConstant * tempGoing;
                    }
                }

                // Szög random generálása
                tempAngle = Math.Asin( tempPos - Math.Floor(tempPos) ) *180 / Math.PI;

                lock (lockAttributes)
                {
                    position = tempPos;
                    angle = tempAngle;
                }

                Thread.Sleep(50);
            }
            

        }
    }
}
