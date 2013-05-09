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
    public partial class AssemblyPicker : UserControl
    {
        public static string defaultControllerFolder = @"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\dlls\";
        public static string defaultLoggerFolder = @"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\dlls\";
        public static string defaultConnectionFolder = @"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\dlls\";
        public static string defaultAccessionFolder = @"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\dlls\";
        public static string defaultPickFolder = @"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\PIDController\PIDController\bin\Release";
        private Form1 owner;

        public AssemblyPicker(Form1 _owner)
        {
            owner = _owner;

            InitializeComponent();

            openFileDialog1.InitialDirectory = defaultPickFolder;
            openFileDialog1.Title = "Choose some additional assemblies...";

            refreshLists();
        }

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

            System.IO.FileInfo[] dlls = defDir.GetFiles("*Controller*");
            foreach (var file in dlls)
            {
                if (file.Name != "AController.dll")
                    checkedListBox1.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;

            defDir = new System.IO.DirectoryInfo(defaultLoggerFolder);

            dlls = defDir.GetFiles("*Logger*");
            foreach (var file in dlls)
            {
                checkedListBox2.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox2.ItemCheck += CheckedListBox2_ItemCheck;

            defDir = new System.IO.DirectoryInfo(defaultConnectionFolder);

            dlls = defDir.GetFiles("*Connection*");
            foreach (var file in dlls)
            {
                if (file.Name != "AConnection.dll")
                    checkedListBox3.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox3.ItemCheck += CheckedListBox3_ItemCheck;

            defDir = new System.IO.DirectoryInfo(defaultAccessionFolder);

            dlls = defDir.GetFiles("*Accession*");
            foreach (var file in dlls)
            {
                checkedListBox4.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox4.ItemCheck += CheckedListBox4_ItemCheck;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Controller") && !name[i].Contains("AController.dll"))
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

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Logger.dll"))
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

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Connection") && !name[i].Contains("AConnection.dll"))
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

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string[] path = openFileDialog1.FileNames;
            string[] name = openFileDialog1.SafeFileNames;
            for (int i = 0; i < path.Length; i++)
            {
                if (name[i].Contains("Accession.dll"))
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

        private void button1_Click(object sender, EventArgs e)
        {
            UserControl tempUI = Management.Controller.getInterface();
            APresenter tempPres = Management.getPresenter();
            owner.controllerSelected(tempUI,tempPres);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Management.InitSession(this.owner))
            {
                button1.Enabled = true;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox1.SelectedItem).Path);
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox2.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox2.SelectedItem).Path);
            }
        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox3.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox3.SelectedItem).Path);
            }
        }

        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox4.SelectedItem != null)
            {
                description(((AssemblyItem)checkedListBox4.SelectedItem).Path);
            }
        }

        private void description(string inputPath){
            owner.groupBox2.Text = "Description";
            owner.textBox1.Text = "";
            owner.pictureBox1.Image = null;

            Assembly _assembly = null;
            Stream _imageStream = null;
            StreamReader _textStreamReader = null;
            try
            {
                _assembly = Assembly.LoadFrom(inputPath);
            }
            catch
            {
                return;
            }
            try
            {
                _imageStream = _assembly.GetManifestResourceStream("Pendulum.Picture.bmp");
            }
            catch
            {

            }
            try
            {
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Pendulum.Description.txt"));
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
        }
    }
}
