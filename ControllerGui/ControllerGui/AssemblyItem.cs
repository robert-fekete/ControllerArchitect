using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControllerGui
{
    class AssemblyItem
    {
        private string path;
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        public AssemblyItem(string _file, string _name)
        {
            path = _file;
            name = _name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
