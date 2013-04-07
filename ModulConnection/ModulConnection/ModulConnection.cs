﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Management;

namespace Pendulum
{
    public class ModulConnection : Connection, Management.IProcess
    {
        private APendulumAccession accession;

        public APendulumAccession Accession
        {
            get
            {
                return accession;
            }
            set
            {
                accession = value;
            }
        }

        public ModulConnection(APendulumAccession _Accessor)
        {
            Accession = _Accessor;
            inputLabels = new string[] { "Angle", "Position" };
            outputLabels = new string[] { "u" };
        }

        public override double[] get()
        {
           accession.updateAnalogInput();
            accession.updateDigitalInput();
            return new double[]{accession.Angle,accession.Position};
        }

        public override void set(double[] u)
        {
            accession.GoingDir = u[0];
            accession.updateDigitalOutput();
        }

        public override double[] update(double[] u)
        {
            set(u);
            get();
            return new double[] { accession.Angle, accession.Position };
        }
    }
}
