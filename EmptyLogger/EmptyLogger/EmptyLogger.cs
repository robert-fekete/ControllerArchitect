using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControllerArchitect;

namespace Log
{
    public class EmptyLogger : Logger
    {
        public EmptyLogger(IProcess _Process) : base(_Process)
        {

        }

        public override double[] get()
        {
            if (Management.debug) Console.WriteLine("EmptyLogger.get");
            return Process.get();
        }

        public override void set(double[] u)
        {
            if (Management.debug) Console.WriteLine("EmptyLogger.set");
            Process.set(u);
        }

        public override double[] update(double[] u)
        {
            if (Management.debug) Console.WriteLine("EmptyLogger.update");
            return Process.update(u);
        }

        protected override void keepUpToDate()
        {
            if (Management.debug) Console.WriteLine("EmptyLogger.keepUpToDate");
            if (Management.debug) Console.WriteLine("Doing nothing, doesn't suppose to enter this method");
        }
    }
}
