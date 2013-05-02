using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(Double.NaN);
            Console.WriteLine(Double.NegativeInfinity);
            Console.WriteLine(Double.PositiveInfinity);
            Console.WriteLine(Double.Epsilon );
            Console.WriteLine(Double.MinValue);
            Console.WriteLine(Double.MaxValue);
            int zero = 0;
            Console.WriteLine( 0/ zero);
        }
    }
}
