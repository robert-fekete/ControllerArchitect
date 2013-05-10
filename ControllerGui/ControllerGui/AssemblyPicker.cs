using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Controller;
using System.Reflection;
using System.IO;
using Management;

namespace ControllerGui
{
    /**
     * Az assembly kiválasztó felület  
     * */
    public partial class AssemblyPicker : UserControl
    {
        // A listák betöltésének induló könyvtárai
        public static string defaultControllerFolder = System.IO.Directory.GetCurrentDirectory() + @"\bin";
        public static string defaultLoggerFolder = System.IO.Directory.GetCurrentDirectory() + @"\bin";
        public static string defaultConnectionFolder = System.IO.Directory.GetCurrentDirectory() + @"\bin";
        public static string defaultAccessionFolder = System.IO.Directory.GetCurrentDirectory() + @"\bin";
        public static string defaultPickFolder = System.IO.Directory.GetCurrentDirectory() + @"\bin";
        private Form1 owner;

        public AssemblyPicker(Form1 _owner)
        {
            owner = _owner;

            InitializeComponent();

            openFileDialog1.InitialDirectory = defaultPickFolder;
            openFileDialog1.Title = "Choose some additional assemblies...";

            refreshLists();
        }

        /**
         * Frissíti a listákat, az induló mappájuk szerint, és kiüríti a textbox-okat 
         * */
        public void refreshLists()
        {
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            checkedListBox3.Items.Clear();
            checkedListBox4.Items.Clear();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            System.IO.DirectoryInfo defDir = new System.IO.DirectoryInfo(defaultControllerFolder);
            System.IO.FileInfo[] dlls;

            if (defDir.Exists == true)
            {
                // Betölti azokat a fájlokat, amelyek nevében szerepel a Controller szó
                dlls = defDir.GetFiles("*Controller*");

                // Ha nem az abstract ősosztály, akkor hozzáadja a listához
                foreach (var file in dlls)
                {
                    if (file.Name != "AController.dll")
                        checkedListBox1.Items.Add(new AssemblyItem(file.FullName, file.Name));
                }
            }
            else
            {
                defDir.Create();
            }

            // Listener regisztrálása
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;

            defDir = new System.IO.DirectoryInfo(defaultLoggerFolder);

            if (defDir.Exists == true)
            {

                dlls = defDir.GetFiles("*Logger*");
                foreach (var file in dlls)
                {
                    if (file.Name != "ALogger.dll")
                        checkedListBox2.Items.Add(new AssemblyItem(file.FullName, file.Name));
                }
            }
            else
            {
                defDir.Create();
            }

            // Listener regisztrálása
            checkedListBox2.ItemCheck += CheckedListBox2_ItemCheck;

            defDir = new System.IO.DirectoryInfo(defaultConnectionFolder);

            if (defDir.Exists == true)
            {
                dlls = defDir.GetFiles("*Connection*");
                foreach (var file in dlls)
                {
                    if (file.Name != "AConnection.dll")
                        checkedListBox3.Items.Add(new AssemblyItem(file.FullName, file.Name));
                }
            }
            else
            {
                defDir.Create();
            }

            // Listener regisztrálása
            checkedListBox3.ItemCheck += CheckedListBox3_ItemCheck;

            defDir = new System.IO.DirectoryInfo(defaultAccessionFolder);

            if (defDir.Exists == true)
            {
                dlls = defDir.GetFiles("*Accession*");
                foreach (var file in dlls)
                {
                    if (file.Name != "APendulumAccession.dll")
                        checkedListBox4.Items.Add(new AssemblyItem(file.FullName, file.Name));
                }
            }
            else
            {
                defDir.Create();
            }

            // Listener regisztrálása
            checkedListBox4.ItemCheck += CheckedListBox4_ItemCheck;
        }

        /**
         * A Browse gomb listener-e
         * Feldobja a fájl választót, és ha az eredménybne szerepel a Controller szó, és még nincs ilyen abszolút elérésű fájl
         * a listában, akkor hozzáadja
         * */
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Controller") && !name[i].Contains("AController.dll") && name[i].Contains(".dll"))
                {
                    bool has = false;
                    foreach (AssemblyItem item in checkedListBox1.Items)
                    {
                        if (item.Path.CompareTo(path[i]) == 0)
                        {
                            has = true;
                        }
                    }

                    if (!has)
                    {
                        checkedListBox1.Items.Add(new AssemblyItem(path[i], name[i]));
                    }
                }
            }
        }

        /**
         * Egy elem bejelölésekor, az előző bejelölt elemet jelöletlenre állítja
         * Ha nincs elem kijelölve, akkor üríti a textbox-ot
         * */
        private void CheckedListBox1_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int ix = 0; ix < checkedListBox1.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox1.SetItemChecked(ix, false);

                textBox1.Text = (checkedListBox1.Items[e.Index] as AssemblyItem).Path;
            }
            else
            {
                textBox1.Clear();
            }

            textBox1.Select(textBox1.Text.Length, 0);
        }

        /**
         * A Browse gomb listener-e
         * Feldobja a fájl választót, és ha az eredménybne szerepel a Logger szó, és még nincs ilyen abszolút elérésű fájl
         * a listában, akkor hozzáadja
         * */
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Logger") && name[i].Contains(".dll") && !name[i].Contains("ALogger.dll"))
                {
                    bool has = false;
                    foreach (AssemblyItem item in checkedListBox1.Items)
                    {
                        if (item.Path.CompareTo(path[i]) == 0)
                        {
                            has = true;
                        }
                    }

                    if (!has)
                    {
                        checkedListBox2.Items.Add(new AssemblyItem(path[i], name[i]));
                    }
                }
            }
        }

        /**
         * Egy elem bejelölésekor, az előző bejelölt elemet jelöletlenre állítja
         * Ha nincs elem kijelölve, akkor üríti a textbox-ot
         * */
        private void CheckedListBox2_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int ix = 0; ix < checkedListBox2.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox2.SetItemChecked(ix, false);

                textBox2.Text = (checkedListBox2.Items[e.Index] as AssemblyItem).Path;
            }
            else
            {
                textBox2.Clear();
            }

            textBox2.Select(textBox2.Text.Length, 0);
        }

        /**
         * A Browse gomb listener-e
         * Feldobja a fájl választót, és ha az eredménybne szerepel a Connection szó, és még nincs ilyen abszolút elérésű fájl
         * a listában, akkor hozzáadja
         * */
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Connection") && !name[i].Contains("AConnection.dll") && name[i].Contains(".dll"))
                {
                    bool has = false;
                    foreach (AssemblyItem item in checkedListBox1.Items)
                    {
                        if (item.Path.CompareTo(path[i]) == 0)
                        {
                            has = true;
                        }
                    }

                    if (!has)
                    {
                        checkedListBox3.Items.Add(new AssemblyItem(path[i], name[i]));
                    }
                }
            }
        }

        /**
         * Egy elem bejelölésekor, az előző bejelölt elemet jelöletlenre állítja
         * Ha nincs elem kijelölve, akkor üríti a textbox-ot
         * */
        private void CheckedListBox3_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int ix = 0; ix < checkedListBox3.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox3.SetItemChecked(ix, false);

                textBox3.Text = (checkedListBox3.Items[e.Index] as AssemblyItem).Path;
            }
            else
            {
                textBox3.Clear();
            }
            textBox3.Select(textBox3.Text.Length, 0);
        }

        /**
         * A Browse gomb listener-e
         * Feldobja a fájl választót, és ha az eredménybne szerepel a Accession szó, és még nincs ilyen abszolút elérésű fájl
         * a listában, akkor hozzáadja
         * */
        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Accession.dll") && name[i].Contains(".dll") && !name[i].Contains("APendulumAccession.dll"))
                {
                    bool has = false;
                    foreach (AssemblyItem item in checkedListBox1.Items)
                    {
                        if (item.Path.CompareTo(path[i]) == 0)
                        {
                            has = true;
                        }
                    }
                    if (!has)
                    {
                        checkedListBox4.Items.Add(new AssemblyItem(path[i], name[i]));
                    }
                }
            }
        }

        /**
         * Egy elem bejelölésekor, az előző bejelölt elemet jelöletlenre állítja
         * Ha nincs elem kijelölve, akkor üríti a textbox-ot
         * */
        private void CheckedListBox4_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int ix = 0; ix < checkedListBox4.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox4.SetItemChecked(ix, false);

                textBox4.Text = (checkedListBox4.Items[e.Index] as AssemblyItem).Path;
            }
            else
            {
                textBox4.Clear();
            }
            textBox4.Select(textBox4.Text.Length, 0);
        }

        /**
         * OK gomb listener-e
         * Beállítja a felületeket, visszaadja a vezérlést a Form1-nek
         * */
        private void button1_Click(object sender, EventArgs e)
        {
            UserControl tempUI = Management.Controller.getInterface();
            APresenter tempPres = Management.getPresenter();
            owner.controllerSelected(tempUI,tempPres);
        }

        /**
         * Test gomb action listener-e
         * Management osztályt hívja, betölti az assembly-ket
         * */
        private void button6_Click(object sender, EventArgs e)
        {
            if (Management.InitSession(this.owner))
            {
                button1.Enabled = true;
            }
        }

        /**
         * Kép és leírás frissítése
         * */
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox1.SelectedItem).Path);
            }
        }

        /**
         * Kép és leírás frissítése
         * */
        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox2.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox2.SelectedItem).Path);
            }
        }

        /**
         * Kép és leírás frissítése
         * */
        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox3.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox3.SelectedItem).Path);
            }
        }

        /**
         * Kép és leírás frissítése
         * */
        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox4.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox4.SelectedItem).Path);
            }
        }

        /**
         * A kép és leírás frissítéséért felelős osztály
         * */
        private void description(string inputPath){
            owner.groupBox2.Text = "Description";
            owner.textBox1.Text = "";
            owner.pictureBox1.Image = null;
            string nameSpace;

            Assembly _assembly = null;
            Stream _imageStream = null;
            StreamReader _textStreamReader = null;
            try
            {
                _assembly = Assembly.LoadFrom(inputPath);

                // Namespace resource megkeresése és betöltése
                foreach (var s in _assembly.GetManifestResourceNames())
                {
                    if (s.Contains("Namespace.txt"))
                    {
                        _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream(s));
                    }
                }
                if (_textStreamReader == null)
                {
                    owner.groupBox2.Text = "Error";
                    owner.textBox1.Text = "There is no Namespace.txt resource in the assembly. The namespace suppose to be defined in Namespace.txt Embedded Resource.";
                    return;
                }

                nameSpace = _textStreamReader.ReadLine();
            }
            catch
            {
                return;
            }
            try
            {
                _imageStream = _assembly.GetManifestResourceStream(nameSpace + ".Picture.bmp");
            }
            catch
            {

            }
            try
            {
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream(nameSpace + ".Description.txt"));
            }
            catch
            {
                owner.textBox1.Text = "There is no description for the assembly.";
            }

            if (_imageStream != null)
            {
                owner.pictureBox1.Image = new Bitmap(_imageStream);
            }
            if (_textStreamReader != null)
            {

                while (_textStreamReader.Peek() > -1)
                {
                    owner.textBox1.AppendText(_textStreamReader.ReadLine());
                    owner.textBox1.AppendText("\n");
                }
            }
            else
            {
                owner.textBox1.Text = "There is no description for the assembly.";
            }
            if (_textStreamReader != null)
            {
                _textStreamReader.Dispose();
            }
            if (_imageStream != null)
            {
                _imageStream.Dispose();
            }
        }
    }
}
