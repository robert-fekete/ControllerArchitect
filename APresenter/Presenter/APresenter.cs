using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Management
{
    public partial class APresenter : UserControl
    {
        public APresenter()
        {
            InitializeComponent();
        }

        public virtual void updateDraw(double[] values) { }

        public virtual void updateLog(string[] _input) { }

        public virtual void reset() { }
    }
}
