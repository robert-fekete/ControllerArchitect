using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Management;

namespace Controller
{
    public class TestController : AController
    {

        public TestController(IProcess _Process) : base(_Process)
        {

        }

        public override void Run()
        {

            double[] temp = {0,0};
            double epsilon = 0.01;

            while (!(temp[1] > (2.5 - epsilon) && temp[1] < (2.5 + epsilon)))
            {
                if (temp[1] <= (2.5 - epsilon))
                {
                    Process.set(new double[] { 1.0 });
                    temp = Process.get();
                    Thread.Sleep(50);
                }
                if (temp[1] > (2.5 + epsilon))
                {
                    Process.set(new double[] { -0.3 });
                    temp = Process.get();
                    Thread.Sleep(50);
                }
                
                Console.WriteLine();

            }

            Process.set(new double[] { 0.0 });
             
            for (int i = 0; i < 5; i++)
            {
                temp = Process.get();
                Thread.Sleep(50);
                
                Console.WriteLine();
            }

        }
    }
}
