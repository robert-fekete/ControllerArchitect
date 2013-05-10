using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Management;

namespace Log
{
    /**
     * Egy üres logger osztály, loggolás nélkül, az transzparensség tesztelésére
     * */
    public class EmptyLogger : Logger
    {
        public EmptyLogger(IProcess _Process, string[] inputLbls, string[] outputLbls) : base(_Process, inputLbls, outputLbls)
        {

        }

        public override double[] get()
        {
            return Process.get();
        }

        public override void set(double[] u)
        {
            Process.set(u);
        }

        public override double[] update(double[] u)
        {
            return Process.update(u);
        }

        protected override void keepUpToDate()
        {

        }
    }
}
