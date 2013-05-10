using System;
using System.Collections.Generic;
using System.Text;
using Management;

namespace Pendulum
{
    /**
     * Az AConnection abstract ősosztály egy egységes felületet nyújt, ebből származtatva bármely folyamat hozzá illeszthető a rendszerhez
     * */
    public abstract class AConnection : Management.IProcess
    {
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

        public abstract APresenter getPresenter(); 
    }
}
