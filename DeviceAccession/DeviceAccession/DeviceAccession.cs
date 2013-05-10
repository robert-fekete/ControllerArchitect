using System;
using System.Collections.Generic;
using System.Text;
using NationalInstruments.DAQmx;


/**
 * Referenciának meg kell adni:
 * 
 * NationalInstruments.DAQmx;
 * NationalInstruments.DAQmx.ComponentModel
 * NationalInstruments.Common;
 * NationalInstruments.Common.Native;
 * */

namespace Pendulum
{
    /**
     * A fizikai eszközhöz való hozzáférést biztosító osztály
     * */
    public class DeviceAccession : APendulumAccession
    {
        
        private Device DAQ;

        private Task ditask;
        private Task dotask;
        private Task aitask;

        private DigitalMultiChannelReader digreader;
        private DIChannel chLeftEnd;
        private DIChannel chRightEnd;

        private DigitalMultiChannelWriter digwriter;
        private DOChannel chMoveToRight;
        private DOChannel chMoveToLeft;

        private AnalogMultiChannelReader anreader;
        private AIChannel chAngle;
        private AIChannel chPosition;
        

        public DeviceAccession()
        {
            //Az attributumok elérésénél a lockhoz a szinkronizációs objektum
            lockAttributes = new object();
              
            
            DAQ = DaqSystem.Local.LoadDevice("Dev1");
            DAQ.SelfCalibrate();
             
            ditask = new Task();
            dotask = new Task();
            aitask = new Task();

            chLeftEnd = ditask.DIChannels.CreateChannel("Dev1/port0/line0", "Left End", ChannelLineGrouping.OneChannelForEachLine);
            chRightEnd = ditask.DIChannels.CreateChannel("Dev1/port0/line1", "Right End", ChannelLineGrouping.OneChannelForEachLine);

            chMoveToLeft = dotask.DOChannels.CreateChannel("Dev1/port1/line0", "Move to the Left", ChannelLineGrouping.OneChannelForEachLine);
            chMoveToRight = dotask.DOChannels.CreateChannel("Dev1/port1/line1", "Move to the Right", ChannelLineGrouping.OneChannelForEachLine);

            chAngle = aitask.AIChannels.CreateVoltageChannel("Dev1/ai0", "Angle", AITerminalConfiguration.Rse, -10, 10, AIVoltageUnits.Volts);
            chPosition = aitask.AIChannels.CreateVoltageChannel("Dev1/ai1", "Position", AITerminalConfiguration.Rse, -10, 10, AIVoltageUnits.Volts);

            ditask.Start();
            dotask.Start();
            aitask.Start();

            digreader = new DigitalMultiChannelReader(ditask.Stream);
            digwriter = new DigitalMultiChannelWriter(dotask.Stream);
            anreader = new AnalogMultiChannelReader(aitask.Stream);
            
        }
        
        public override double[] updateAnalogInput()
        {
            double[] temp = anreader.ReadSingleSample();
            angle = temp[0];
            position = temp[1];

            return temp;
        }

        public override bool[,] updateDigitalInput()
        {
            bool[,] ends = digreader.ReadSingleSampleMultiLine();
            isLeftEnd = !ends[0, 0];
            isRightEnd = !ends[1, 0];

            return ends;
            
        }

        public override void updateDigitalOutput()
        {
            bool isGoingLeft = goingDir < 0;
            bool isGoingRight = goingDir > 0;
            bool[,] data = new bool[2, 1];
            data[0, 0] = isGoingLeft;
            data[1, 0] = isGoingRight;
            digwriter.WriteSingleSampleMultiLine(true, data);
        }
    }
}
