using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ControllerGui
{
    public partial class AssemblyPicker : UserControl
    {
        private static readonly string defaultFolder = @"D:\C#\users\roberto\documents\visual studio 2010\Projects\ControllerArchitect\dlls\";
        public AssemblyPicker()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = defaultFolder;
            openFileDialog1.Title = "Choose some additional assemblies...";

            System.IO.DirectoryInfo defDir = new System.IO.DirectoryInfo(defaultFolder);

            System.IO.FileInfo[] dlls = defDir.GetFiles("*Controller.dll");
            foreach (var file in dlls)
            {
                if (file.Name != "AController.dll")
                    checkedListBox1.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;

            dlls = defDir.GetFiles("*Logger.dll");
            foreach (var file in dlls)
            {
                checkedListBox2.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox2.ItemCheck += CheckedListBox2_ItemCheck;

            dlls = defDir.GetFiles("*Connection.dll");
            foreach (var file in dlls)
            {
                if (file.Name != "AConnection.dll")
                    checkedListBox3.Items.Add(new AssemblyItem(file.FullName, file.Name));
            }
            checkedListBox3.ItemCheck += CheckedListBox3_ItemCheck;

            dlls = defDir.GetFiles("*Accession.dll");
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
                if(name[i].Contains("Controller.dll") && !name[i].Contains("AController.dll"))
                {
                    bool has = false;
                    foreach (var item in checkedListBox1.Items)
                    {
                        if (item.ToString().CompareTo(name[i]) == 0)
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
                    foreach (var item in checkedListBox2.Items)
                    {
                        if (item.ToString().CompareTo(name[i]) == 0)
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
                if (name[i].Contains("Connection.dll") && !name[i].Contains("AConnection.dll"))
                {
                    bool has = false;
                    foreach (var item in checkedListBox3.Items)
                    {
                        if (item.ToString().CompareTo(name[i]) == 0)
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
                    foreach (var item in checkedListBox4.Items)
                    {
                        if (item.ToString().CompareTo(name[i]) == 0)
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
    }
}
