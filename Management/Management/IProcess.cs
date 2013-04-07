using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Management
{
    public interface IProcess
    {
        string[] getInputLabels();

        string[] getOutputLabels();

        double[] get();

        void set(double[] u);

        double[] update(double[] u);
    }
}
