using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Management;

namespace Pendulum
{
    public abstract class Connection : IProcess
    {
        private double reference;
        protected string[] inputLabels;
        protected string[] outputLabels;

        public abstract double[] get();

        public abstract void set(double[] u);

        public abstract double[] update(double[] u);

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
