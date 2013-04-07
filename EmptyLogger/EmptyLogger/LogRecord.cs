using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    public class LogRecord
    {
        private DateTime timestamp;
        private double[] value;
        private string[] labels;
    
        public LogRecord(DateTime dt, string[] lbl, double[] values)
        {
            timestamp = dt;
            labels = lbl;
            value = values;
        }

        public DateTime TimeStamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }

        public double[] Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public string[] Labels
        {
            get
            {
                return labels;
            }
            set
            {
                labels = value;
            }
        }
    }
}
