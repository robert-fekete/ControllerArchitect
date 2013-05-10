using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Management;
using System.Windows.Forms;
using System.IO;

namespace ControllerGui
{
    /**
     * Az assembly-k betöltéséért felelős osztály
     * */
    public static class Management
    {
        private static AController controller;
        private static dynamic tempAcc;
        private static dynamic tempProc;
        private static dynamic tempLogger;
        private static APresenter pres;

        public static AController Controller
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
    
        /**
         * A függvény leellenőrzi az AssemblyPicker felületen beállított assembly-ket, hogy a program számára emészthető formátumúak-e
         * Példányosítja a megfelelő objektumokat
         * */
        public static bool InitSession(Form1 owner)
        {
            if (owner.pck.button1.Enabled) return true;
            Assembly ass;
            Type type = null;
            StreamReader _textStreamReader = null;
            ConstructorInfo con1 = null;
            Type AccessionBaseType = null;
            Type IProcInterface = null;


            #region A folyamat megvalósításának betöltése
            try
            {
                //A Dll fájl betöltése
                ass = Assembly.LoadFrom(owner.pck.textBox4.Text);
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "There is not such a file. The Accession path is probably wrong.";
                return false;
            }

            // Namespace resource megkeresése és betöltése
            foreach (var s in ass.GetManifestResourceNames())
	        {
		        if(s.Contains("Namespace.txt"))
                {
                    _textStreamReader = new StreamReader(ass.GetManifestResourceStream(s));
                }
	        }
            if (_textStreamReader == null)
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "There is no Namespace.txt resource in the Accession assembly. The namespace suppose to be defined in Namespace.txt Embedded Resource.";
                return false;
            }

            string nameSpace = _textStreamReader.ReadLine();

            //A per jel mentén felbontja a stringet, és az utolsó darabot (fájlnév és kiterjesztés) még a pontnál kettévágja, és veszi az elsőt (a fájlnevet kiterjesztés nélkül)
            string className = (owner.pck.textBox4.Text.Split(new char[] { '\\' })).Last<string>().Split(new char[] { '.' }).First<string>();
            string typeName = nameSpace + "." + className;

            try{

                //Megfelelő osztály títpusának lekérése
                type = ass.GetType(typeName);
                if (type == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The Accession assembly doesn't contain any class named: " + typeName;
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The Accession assembly doesn't contain any class named: " + typeName;
                return false;
            }
            try{
                //Az ősosztály típus eltárolása, később a konstruktor paraméter típusának át kell adni
                AccessionBaseType = type.BaseType;
                if (AccessionBaseType == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The following type doesn't have any basetype: " + typeName;
                    owner.textBox1.AppendText("It most implement one of the provided abstract basetypes.");
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The following type doesn't have any basetype: " + typeName;
                owner.textBox1.AppendText("It most implement one of the provided abstract basetypes.");
                return false;
            }
            try
            {
                //Megfelelő paraméterezésű konstruktor lekérése
                con1 = type.GetConstructor(new Type[0] { });
                if (con1 == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The the following type doesn't have an empty parameterized constructor: " + typeName;
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The the following type doesn't have an empty parameterized constructor: " + typeName;
                return false;
            }
            try
            {
                //A Konstruktor meghívása
                tempAcc = con1.Invoke(new object[0] { });
                if (tempAcc == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "Some problem occured during the use of the constructor. (Accession)";
                    return false;
                }
            }
            catch (Exception e)
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "Some problem occured during the use of the constructor. (Logger)";
                owner.textBox1.AppendText(e.Message + "\n");
                owner.textBox1.AppendText(e.StackTrace + "\n\n");
                if (e.InnerException != null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.AppendText("Inner Exception:\n");
                    owner.textBox1.AppendText(e.InnerException + "\n");
                }
                return false;
            }
            #endregion

            #region Az IProcess-t biztosító elfedő osztály betöltése

            _textStreamReader = null;
            type = null;
            con1 = null;

            try
            {
                //DLL betöltése
                ass = Assembly.LoadFrom(owner.pck.textBox3.Text);
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "There is not such a file. The Connection path is probably wrong.";
                return false;
            }

            // Namespace resource megkeresése és betöltése
            foreach (var s in ass.GetManifestResourceNames())
	        {
		        if(s.Contains("Namespace.txt"))
                {
                    _textStreamReader = new StreamReader(ass.GetManifestResourceStream(s));
                }
	        }
            if (_textStreamReader == null)
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "There is no Namespace.txt resource in the Connection assembly. The namespace suppose to be defined in Namespace.txt Embedded Resource.";
                return false;
            }
            nameSpace = _textStreamReader.ReadLine();

            //A per jel mentén felbontja a stringet, és az utolsó darabot (fájlnév és kiterjesztés) még a pontnál kettévágja, és veszi az elsőt (a fájlnevet kiterjesztés nélkül)
            className = (owner.pck.textBox3.Text.Split(new char[] { '\\' })).Last<string>().Split(new char[] { '.' }).First<string>();
            typeName = nameSpace + "." + className;

            try{

                //Megfelelő osztály títpusának lekérése
                type = ass.GetType(typeName);
                if (type == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The Connection assembly doesn't contain any class named: " + typeName;
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The Connection assembly doesn't contain any class named: " + typeName;
                return false;
            }
            try
            {   
                //IProcess interfész eltárolása, később konstruktor paraméternek kell
                foreach (var i in type.GetInterfaces())
                {
                    if (i.Name.Contains("IProcess"))
                    {
                        IProcInterface = i;                        
                    }
                }
                if (IProcInterface == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The Connection assembly doesn't contain any interface named: IProcess";
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The Connection assembly doesn't contain any interface named: IProcess";
                return false;
            }
            try
            {
                // Ősosztály paraméterű konstruktor
                con1 = type.GetConstructor(new Type[1] { AccessionBaseType });
                if (con1 == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The the following type doesn't have constructor with parameter " + AccessionBaseType.Name + " : " + typeName;
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The the following type doesn't have constructor with parameter " + AccessionBaseType.Name + " : " + typeName;
                return false;
            }
            try
            {
                //A Konstruktor meghívása
                tempProc = con1.Invoke(new object[1] { tempAcc });
                if (tempProc == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "Some problem occured during the use of the constructor. (Connection)";
                    return false;
                }
            }
            catch ( Exception e)
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "Some problem occured during the use of the constructor. (Logger)";
                owner.textBox1.AppendText(e.Message + "\n");
                owner.textBox1.AppendText(e.StackTrace + "\n\n");
                if (e.InnerException != null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.AppendText("Inner Exception:\n");
                    owner.textBox1.AppendText(e.InnerException + "\n");
                }
                return false;
            }
            try
            {
                // A folyamathoz tartozó presenter eltárolása
                pres = tempProc.getPresenter();
                if (pres == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The received presenter is null.";
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The received presenter is null.";
                return false;
            }

            #endregion

            #region Logger betöltése

            // Ha választottunk ki loggert
            if (owner.pck.textBox2.Text != "")
            {
                _textStreamReader = null;
                type = null;
                con1 = null;

                try
                {
                    //DLL betöltése
                    ass = Assembly.LoadFrom(owner.pck.textBox2.Text);
                }
                catch
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "There is not such a file. The Logger path is probably wrong.";
                    return false;
                }

                // Namespace resource megkeresése és betöltése
                foreach (var s in ass.GetManifestResourceNames())
                {
                    if (s.Contains("Namespace.txt"))
                    {
                        _textStreamReader = new StreamReader(ass.GetManifestResourceStream(s));
                    }
                }
                if (_textStreamReader == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "There is no Namespace.txt resource in the Logger assembly. The namespace suppose to be defined in Namespace.txt Embedded Resource.";
                    return false;
                }
                nameSpace = _textStreamReader.ReadLine();

                //A per jel mentén felbontja a stringet, és az utolsó darabot (fájlnév és kiterjesztés) még a pontnál kettévágja, és veszi az elsőt (a fájlnevet kiterjesztés nélkül)
                className = (owner.pck.textBox2.Text.Split(new char[] { '\\' })).Last<string>().Split(new char[] { '.' }).First<string>();
                typeName = nameSpace + "." + className;

                try
                {

                    //Megfelelő osztály títpusának lekérése
                    type = ass.GetType(typeName);
                    if (type == null)
                    {
                        owner.groupBox2.Text = "Error";
                        owner.textBox1.Text = "The Logger assembly doesn't contain any class named: " + typeName;
                        return false;
                    }
                }
                catch
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The Logger assembly doesn't contain any class named: " + typeName;
                    return false;
                }
                try
                {
                    // IProcess és 2 string tömb paraméterű konstruktor
                    con1 = type.GetConstructor(new Type[3] { IProcInterface, typeof(string[]), typeof(string[]) });
                    if (con1 == null)
                    {
                        owner.groupBox2.Text = "Error";
                        owner.textBox1.Text = "The the following type doesn't have constructor with parameter " + IProcInterface.Name + " and two string arrays: " + typeName;
                        return false;
                    }
                }
                catch
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The the following type doesn't have constructor with parameter " + IProcInterface.Name + " and two string arrays: " + typeName;
                    return false;
                }
                try
                {
                    //A Konstruktor meghívása
                    tempLogger = con1.Invoke(new object[3] { tempProc, tempProc.getInputLabels(), tempProc.getOutputLabels() });
                    if (tempLogger == null)
                    {
                        owner.groupBox2.Text = "Error";
                        owner.textBox1.Text = "Some problem occured during the use of the constructor. (Logger)";
                        return false;
                    }
                }
                catch (Exception e)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "Some problem occured during the use of the constructor. (Logger)";
                    owner.textBox1.AppendText(e.Message + "\n");
                    owner.textBox1.AppendText(e.StackTrace + "\n\n");
                    if (e.InnerException != null)
                    {
                        owner.groupBox2.Text = "Error";
                        owner.textBox1.AppendText("Inner Exception:\n");
                        owner.textBox1.AppendText(e.InnerException + "\n");
                    }
                    return false;
                }
            }

            #endregion

            #region A szabályozó betöltése

            _textStreamReader = null;
            type = null;
            con1 = null;

            try
            {
                //DLL betöltése
                ass = Assembly.LoadFrom(owner.pck.textBox1.Text);
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "There is not such a file. The Controller path is probably wrong.";
                return false;
            }

            // Namespace resource megkeresése és betöltése
            foreach (var s in ass.GetManifestResourceNames())
            {
                if (s.Contains("Namespace.txt"))
                {
                    _textStreamReader = new StreamReader(ass.GetManifestResourceStream(s));
                }
            }
            if (_textStreamReader == null)
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "There is no Namespace.txt resource in the Controller assembly. The namespace suppose to be defined in Namespace.txt Embedded Resource.";
                return false;
            }
            nameSpace = _textStreamReader.ReadLine();

            //A per jel mentén felbontja a stringet, és az utolsó darabot (fájlnév és kiterjesztés) még a pontnál kettévágja, és veszi az elsőt (a fájlnevet kiterjesztés nélkül)
            className = (owner.pck.textBox1.Text.Split(new char[] { '\\' })).Last<string>().Split(new char[] { '.' }).First<string>();
            typeName = nameSpace + "." + className;

            try
            {
                //Megfelelő osztály títpusának lekérése
                type = ass.GetType(typeName);
                if (type == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The Controller assembly doesn't contain any class named: " + typeName;
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The Controller assembly doesn't contain any class named: " + typeName;
                return false;
            }
            try
            {
                // Konstruktor meghívása
                con1 = type.GetConstructor(new Type[1] { typeof(IProcess) });
                if (con1 == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "The the following type doesn't have constructor with parameter " + IProcInterface.Name + " : " + typeName;
                    return false;
                }
            }
            catch
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "The the following type doesn't have constructor with parameter " + IProcInterface.Name + " : " + typeName;
                return false;
            }
            try
            {
                // Ha nem választottunk ki Logger-t, akkor közvetlen a process-t hívogatja
                if (owner.pck.textBox2.Text == "")
                {
                    controller = con1.Invoke(new object[1] { tempProc }) as AController;
                }
                else
                {
                    controller = con1.Invoke(new object[1] { tempLogger }) as AController;
                }
                if (controller == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "Some problem occured during the use of the constructor. (Controller)";
                    return false;
                }
            }
            catch (Exception e)
            {
                owner.groupBox2.Text = "Error";
                owner.textBox1.Text = "Some problem occured during the use of the constructor. (Logger)";
                owner.textBox1.AppendText(e.Message + "\n");
                owner.textBox1.AppendText(e.StackTrace + "\n\n");
                if (e.InnerException != null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.AppendText("Inner Exception:\n");
                    owner.textBox1.AppendText(e.InnerException + "\n");
                }
                return false;
            }

            #endregion

            return true;
        }

        public static APresenter getPresenter()
        {
            return pres;
        }

        /**
         * Két assembly összeállítás között törölni kell a változókat
         * */
        public static void Clear()
        {
            tempAcc = null;
            tempProc = null;
            tempLogger = null;
            controller = null;
        }
    }
}