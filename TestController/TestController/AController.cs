using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Management;

namespace Controller
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

        public abstract void Run();
    }
}
