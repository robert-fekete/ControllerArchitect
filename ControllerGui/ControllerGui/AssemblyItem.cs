namespace ControllerGui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /**
     * A listában szereplő assembly elem, névvel és fájl eléréssel
     */
    internal class AssemblyItem
    {
        private string path;
        private string name;

        public AssemblyItem(string _file, string _name)
        {
            path = _file;
            this.name = _name;
        }

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

        public override string ToString()
        {
            return name;
        }
    }
}
