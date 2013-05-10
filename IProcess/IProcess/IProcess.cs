using System;
using System.Collections.Generic;
using System.Text;

namespace Management
{
    /**
     * A folyamatot implementáló osztályhoz tartozó interface
     * A transzparens megvalósítás miatt a logger osztály is ezt az interface-t valósítja meg
     * */
    public interface IProcess
    {
        // A ki és bemenő értékek nevesítése
        string[] getInputLabels();

        string[] getOutputLabels();

        double[] get();

        void set(double[] u);

        double[] update(double[] u);
    }
}
