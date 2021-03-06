﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using ControllerArchitect;

namespace Log
{
    public abstract class Logger : IProcess
    {
        protected double[] u;                               //A visszaküldött, nem nevesített beavatkozó jel
        protected double angleDiff;
        protected double positionDiff;
        private IProcess process;
        protected static string[] inputLabels;
        protected static string[] outputLabels;
        protected ConcurrentQueue<LogRecord> FIFOInput;
        protected ConcurrentQueue<LogRecord> FIFOOutput;

        //Szekvencia a tároláshoz szükséges ID-k előállítására
        protected class Sequence
        {
            int seqValue;

            //Inicializálja a szekvenciát az inputban kapott érték alapján
            public Sequence(int starter)
            {
                seqValue = starter + 1; 
            }

            //Vissza adja az aktuális ID-t és utána növeli
            public int get()
            {
                int temp = seqValue;
                seqValue++;
                return temp;
            }
        }

        public Logger(IProcess _Process)
        {
            process = _Process;
            FIFOInput = new ConcurrentQueue<LogRecord>();
            FIFOOutput = new ConcurrentQueue<LogRecord>();
        }
    
        public IProcess Process
        {
            get
            {
                return process;
            }
            set
            {
                process = value;
            }
        }
        /// <summary>
        /// </summary>
        public abstract double[] get();

        public abstract void set(double[] u);

        public abstract double[] update(double[] u);

        protected abstract void keepUpToDate();

        public string[] getInputLabels()
        {
            return inputLabels;
        }

        public string[] getOutputLabels()
        {
            return outputLabels;
        }
    }
}
