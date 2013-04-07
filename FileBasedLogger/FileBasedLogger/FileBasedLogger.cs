using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Management;

namespace Log
{
    public class FileBasedLogger : Logger
    {
        private static readonly string inputName = @"In-FileLog.txt";
        private static readonly string outputName = @"Out-FileLog.txt";
        StreamWriter sw;
        Sequence inID;
        Sequence outID;
        Thread Logging;

        public FileBasedLogger(IProcess _Process,string[] inputLbls, string[]outputLbls) : base(_Process)
        {
            //Szekvenciák előállítása
            inID = new Sequence(getLastIndex(inputName));
            outID = new Sequence(getLastIndex(outputName));

            inputLabels = inputLbls;
            outputLabels = outputLbls;

            Logging = new Thread(keepUpToDate);
            Logging.IsBackground = true;
            Logging.Start();
        }

        //Kikeresi a kapott fájlból hogy mi volt az utolsó bejegyzés indexe. Ha üres a fájl akkor 0
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
                using(StreamWriter sw = File.AppendText("ÜberLog.txt"))
                {
                    sw.WriteLine("HAHA elkaptam egy exceptiont! Nem létezett a fájl!");
                    Console.WriteLine("HAHA");
                }
            }
            return starter;
        }
        
        //Eltárolja a lekért értéket, hozzáadja a naplózandó sorhoz, majd tovább adja a Controller felé
        public override double[] get()
        {
            double [] tempValues = Process.get();
            FIFOInput.Enqueue(new LogRecord(DateTime.Now, inputLabels, tempValues));
            
            return tempValues;
        }

        //Hozzáadja a naplózandó sorhoz a bemeneti értéket, majd tovább adja a Process felé
        public override void set(double[] u)
        {
            FIFOOutput.Enqueue(new LogRecord(DateTime.Now,outputLabels, u));
            Process.set(u);
        }

        //Eltárolja a lekért értéket, hozzáadja a naplózandó sorhoz, majd tovább adja a Controller felé
        //Hozzáadja a naplózandó sorhoz a bemeneti értéket, majd tovább adja a Process felé
        public override double[] update(double[] u)
        {
            double[] tempValues = Process.update(u);
            FIFOInput.Enqueue(new LogRecord(DateTime.Now,inputLabels,tempValues));
            FIFOOutput.Enqueue(new LogRecord(DateTime.Now, outputLabels, u));

            return tempValues;
        }

        //A függvény először kinaplózza a ki és bemeneti sorban felgyűlt bejegyzéseket.
        //A függvény ciklukus működésű, 5-ösével naplózza, hogy közel párhuzamosan fusson a két irány naplózása.
        //A függvény külön szálon való futásra van tervezve
        protected override void keepUpToDate()
        {
            while(true){
                using (sw = File.AppendText("In-FileLog.txt"))
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
                                    sw.WriteLine(inID.get() + ";" + tempRec.TimeStamp + ";" + tempValues[j] + ";" + inputLabels[j]);
                                }
                            }
                        }
                    }
                }

                using (sw = File.AppendText("Out-FileLog.txt"))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (FIFOOutput.Count > 0)
                        {
                           LogRecord tempRec = null;
                            if (FIFOOutput.TryDequeue(out tempRec))
                            {
                                double[] tempValues = tempRec.Value;
                                for (int j = 0; j < tempValues.Length; j++)
                                {
                                    sw.WriteLine(outID.get() + ";" + tempRec.TimeStamp + ";" + tempValues[j] + ";" + outputLabels[0]);
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
