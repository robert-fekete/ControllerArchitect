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

            //A folyamat megvalósításának betöltése
            //A Dll fájl betöltése
            Assembly ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\TestAccession\TestAccession\bin\Release\TestAccession.dll");
            
            //Megfelelő osztály títpusának lekérése
            Type type = ass.GetType("Pendulum.TestAccession");

            //Az APendulumAccession ősosztály típus eltárolása, később a konstruktor paraméter típusának át kell adni
            Type AccessionBaseType = type.BaseType;

            //Megfelelő paraméterezésű konstruktor lekérése
            ConstructorInfo con1 = type.GetConstructor(new Type[0] {});
            
            //A Konstruktor meghívása
            dynamic tempAcc = con1.Invoke(new object[0] {});

            //Az IProcess-t biztosító elfedő osztály betöltése
            ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\ModulConnection\ModulConnection\bin\Release\ModulConnection.dll");
            type = ass.GetType("Pendulum.ModulConnection");
        
            //IProcess interfész eltárolása, később konstruktor paraméternek kell
            Type IProcInterface = type.GetInterfaces()[0];

            //Pendulum.APendulumAccession paraméterű konstruktor
            ConstructorInfo con2 = type.GetConstructor(new Type[1] { AccessionBaseType });
            dynamic tempProc = con2.Invoke(new object[1] { tempAcc });
            
            //Logger betöltése
            ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\FileBasedLogger\FileBasedLogger\bin\Release\FileBasedLogger.dll");
            type = ass.GetType("Log.FileBasedLogger");
            con1 = type.GetConstructor(new Type[3]{IProcInterface,typeof(string[]),typeof(string[])});
            dynamic tempLogger = con1.Invoke(new object[3]{tempProc,tempProc.getInputLabels(),tempProc.getOutputLabels()});

            //A szabályozó betöltése
            //ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\TestController\TestController\bin\Release\TestController.dll");
            //type = ass.GetType("Controller.TestController");
            ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\PIDController\PIDController\bin\Release\PIDController.dll");
            type = ass.GetType("Controller.PIDController");
            con1 = type.GetConstructor(new Type[1]{typeof(IProcess)});
            dynamic tempController = con1.Invoke(new object[1]{tempLogger});


            tempController.Run();

        }
    }
}

//APendulumAccession tempAcc = new TestAccession();

//IProcess tempProc = new ModulConnection( tempAcc );

//IProcess tempLogger = new FileBasedLogger(tempProc, tempProc.getInputLabels(), tempProc.getOutputLabels());
//IProcess tempLogger = new EmptyLogger(tempProc);
//IProcess tempLogger = new DatabaseBasedLogger(tempProc);
//Assembly ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\DeviceAccession\DeviceAccession\bin\Release\DeviceAccession.dll");

//Assembly ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\EmptyLogger\EmptyLogger\bin\Release\EmptyLogger.dll");
//ass = Assembly.LoadFrom(@"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\DatabaseBasedLogger\DatabaseBasedLogger\bin\Release\DatabaseBasedLogger.dll");

//con1 = type.GetConstructor(new Type[1]{typeof(IProcess)});
//dynamic tempLogger = con1.Invoke(new object[1]{tempProc});

//Controller = new TestController(tempLogger);

           
