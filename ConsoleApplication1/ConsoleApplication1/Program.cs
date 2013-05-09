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

            Console.WriteLine(System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()),"txt") );
            Console.WriteLine(Double.NegativeInfinity);
            Console.WriteLine(Double.PositiveInfinity);
            Console.WriteLine(Double.Epsilon );
            Console.WriteLine(Double.MinValue);
            Console.WriteLine(Double.MaxValue);
        }
    }
}
