using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Management
{
    public class Management
    {
        static private AController controller;
        public static bool debug = true;

        static public AController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }
    
        public static void Main()
        {

            Console.WriteLine("Fut");

            //APendulumAccession tempAcc = new TestAccession();

            //IProcess tempProc = new ModulConnection( tempAcc );

            //IProcess tempLogger = new FileBasedLogger(tempProc, tempProc.getInputLabels(), tempProc.getOutputLabels());
            //IProcess tempLogger = new EmptyLogger(tempProc);
            //IProcess tempLogger = new DatabaseBasedLogger(tempProc);

            Assembly ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\TestAccession\TestAccession\bin\Release\TestAccession.dll");
            Type type = ass.GetType("Pendulum.TestAccession");
            
            ConstructorInfo con1 = type.GetConstructor(new Type[0] {});
            dynamic tempAcc = con1.Invoke(new object[0] {});

            ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\ModulConnection\ModulConnection\bin\Release\ModulConnection.dll");
            type = ass.GetType("Pendulum.ModulConnection");
            //Pendulum.APendulumAccession tipusu konstruktor
            ConstructorInfo con2 = type.GetConstructor(new Type[1] { ass.GetType("Pendulum.APendulumAccession")});
            
            dynamic tempProc = con2.Invoke(new object[1] { tempAcc });

            
            
            //Assembly ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\EmptyLogger\EmptyLogger\bin\Release\EmptyLogger.dll");
            ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\FileBasedLogger\FileBasedLogger\bin\Release\FileBasedLogger.dll");
            //ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\DatabaseBasedLogger\DatabaseBasedLogger\bin\Release\DatabaseBasedLogger.dll");

            type = ass.GetType("Log.FileBasedLogger");

            con1 = type.GetConstructor(new Type[3]{typeof(IProcess),typeof(string[]),typeof(string[])});
            
            dynamic tempLogger = con1.Invoke(new object[3]{tempProc,tempProc.getInputLabels(),tempProc.getOutputLabels()});

            //con1 = type.GetConstructor(new Type[1]{typeof(IProcess)});
            //dynamic tempLogger = con1.Invoke(new object[1]{tempProc});

            //Controller = new TestController(tempLogger);

            ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\TestController\TestController\bin\Release\TestController.dll");
            type = ass.GetType("Controller.TestController");

            con1 = type.GetConstructor(new Type[1]{typeof(IProcess)});
            dynamic tempController = con1.Invoke(new object[1]{tempLogger});


            tempController.Run();

        }
    }
}
