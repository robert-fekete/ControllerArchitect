using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Management;

namespace Log
{
    /**
     * Fájl alapú naplózást megvalósító osztály
     * */
    public class FileBasedLogger : Logger
    {
        public static string inputName = @"Logs\In-FileLog.txt";
        public static string outputName = @"Logs\Out-FileLog.txt";
        private StreamWriter sw;

        // Sequence-k az ID-k szekvenciális előállításához
        private Sequence inID;
        private Sequence outID;

        // Külön szálon megy a naplózás
        private Thread Logging;

        public FileBasedLogger(IProcess _Process,string[] inputLbls, string[]outputLbls) : base(_Process,inputLbls,outputLbls)
        {
            //Szekvenciák előállítása
            inID = new Sequence(getLastIndex(inputName));
            outID = new Sequence(getLastIndex(outputName));

            // Random nevet generál a log fájlnak
            inputName = System.IO.Path.ChangeExtension(@"Logs\IN_" + System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()), "txt");
            outputName = System.IO.Path.ChangeExtension(@"Logs\OUT_" + System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()), "txt");

            Logging = new Thread(keepUpToDate);
            Logging.IsBackground = true;
        }
        /**
         * Kikeresi a kapott fájlból hogy mi volt az utolsó bejegyzés indexe. Ha üres a fájl akkor 0
         * */
        private int getLastIndex(string fileName)
        {
            int starter = 0;
            try
            {
                string[] tempLines = File.ReadAllLines(fileName);
                if (tempLines.Length > 0)
                {
                    string[] lineParts = tempLines[tempLines.Length - 1].Split(';');
                    starter = Convert.ToInt32(lineParts[0]);
                }
            }
            catch (FileNotFoundException fnfe)
            {
                starter = 0;
                using (StreamWriter sw = File.AppendText("MainLog.txt"))
                {
                    sw.WriteLine("{0} FileNotFound: {1}", DateTime.Now.ToString("yy:MM:dd HH:mm:ss"),fnfe.Message);
                }
            }
            catch (DirectoryNotFoundException dnfe)
            {
                starter = 0;
                using (StreamWriter sw = File.AppendText("MainLog.txt"))
                {
                    sw.WriteLine("{0} DirectoryNotFound: {1}", DateTime.Now.ToString("yy:MM:dd HH:mm:ss"), dnfe.Message);
                }
                System.IO.Directory.CreateDirectory(@"Logs");
            }
            catch (Exception e)
            {
                starter = 0;
                using (StreamWriter sw = File.AppendText("MainLog.txt"))
                {
                    sw.WriteLine("{0} Unexpected Exception: {1}", DateTime.Now.ToString("yy:MM:dd HH:mm:ss"), e.Message);
                }
            }
            return starter;
        }
        
        /**
         * Eltárolja a lekért értéket, hozzáadja a naplózandó sorhoz, majd tovább adja a Controller felé
         * */
        public override double[] get()
        {
            // Ha még nem indult el a naplózás, akkor elindítja
            if (!Logging.IsAlive) Logging.Start();

            double [] tempValues = Process.get();
            FIFOInput.Enqueue(new LogRecord(DateTime.Now, inputLabels, tempValues));
            
            return tempValues;
        }

        /** 
         * Hozzáadja a naplózandó sorhoz a bemeneti értéket, majd tovább adja a Process felé
         * */
        public override void set(double[] u)
        {
            // Ha még nem indult el a naplózás, akkor elindítja
            if (!Logging.IsAlive) Logging.Start();

            FIFOOutput.Enqueue(new LogRecord(DateTime.Now,outputLabels, u));
            Process.set(u);
        }

        /**
         * Eltárolja a lekért értéket, hozzáadja a naplózandó sorhoz, majd tovább adja a Controller felé
         * Hozzáadja a naplózandó sorhoz a bemeneti értéket, majd tovább adja a Process felé
         * */
        public override double[] update(double[] u)
        {
            // Ha még nem indult el a naplózás, akkor elindítja
            if (!Logging.IsAlive) Logging.Start();

            double[] tempValues = Process.update(u);
            FIFOInput.Enqueue(new LogRecord(DateTime.Now,inputLabels,tempValues));
            FIFOOutput.Enqueue(new LogRecord(DateTime.Now, outputLabels, u));

            return tempValues;
        }

        /**
         * A függvény először kinaplózza a ki és bemeneti sorban felgyűlt bejegyzéseket.
         * A függvény ciklukus működésű, 5-ösével naplózza, hogy közel párhuzamosan fusson a két irány naplózása.
         * A függvény külön szálon való futásra van tervezve
         * */
        protected override void keepUpToDate()
        {
            while(true){

                using (sw = File.AppendText(inputName))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (FIFOInput.Count > 0)
                        {
                            LogRecord tempRec = null;

                            // TryDequeue true ha sikerült kivenni, és tempRec-be teszi a kivett rekordot
                            if (FIFOInput.TryDequeue(out tempRec))
                            {
                                double[] tempValues = tempRec.Value;
                                for (int j = 0; j < tempValues.Length; j++)
                                {

                                    sw.WriteLine(inID.get() + ";" + tempRec.TimeStamp.ToString("yyyy.MM.dd HH:mm:ss.fff") + ";" + tempValues[j] + ";" + inputLabels[j]);
                                }
                            }
                        }
                    }
                }

                using (sw = File.AppendText(outputName))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (FIFOOutput.Count > 0)
                        {
                            LogRecord tempRec = null;

                            // TryDequeue true ha sikerült kivenni, és tempRec-be teszi a kivett rekordot
                            if (FIFOOutput.TryDequeue(out tempRec))
                            {
                                double[] tempValues = tempRec.Value;
                                for (int j = 0; j < tempValues.Length; j++)
                                {
                                    sw.WriteLine(outID.get() + ";" + tempRec.TimeStamp.ToString("yyyy.MM.dd HH:mm:ss.fff") + ";" + tempValues[j] + ";" + outputLabels[0]);
                                }
                            }
                        }
                    }
                }

                Thread.Sleep(25);

            }
        }
    }
}
