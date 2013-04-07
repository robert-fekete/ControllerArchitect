using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControllerArchitect;

namespace Log
{
    public class DatabaseBasedLogger : Logger
    {

        public DatabaseBasedLogger(IProcess _Process):base(_Process)
        {

        }

        public void start()
        {
            throw new System.NotImplementedException();
        }

        public void DatabaseConnection()
        {
            throw new System.NotImplementedException();
        }

        public override double[] get()
        {
            if (Management.debug) Console.WriteLine("DataBasedLogger.get");
            //TODO
            return Process.get();
        }

        public override void set(double[] u)
        {
            if (Management.debug) Console.WriteLine("DabatBasedLogger.set");
            //TODO
            Process.set(u);
        }

        public override double[] update(double[] u)
        {
            if (Management.debug) Console.WriteLine("DataBasedLogger.update");
            //TODO
            return Process.update(u);
        }

        protected override void keepUpToDate()
        {
            if (Management.debug) Console.WriteLine("DataBased.Logger.keepUpToDate");
        }
    }
}
