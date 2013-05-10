using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Management
{
    public abstract class AController
    {
        private IProcess process;

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

        public AController(IProcess _Process)
        {
            Process = _Process;
        }
        
        public abstract void Run(APresenter _in);

        public abstract UserControl getInterface();

        public abstract void Stop();
    }
}
