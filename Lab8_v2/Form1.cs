using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Lab8_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "Lab8 by Elizabeth Kenny";
            fileNameListBox.DataSource = fileList;
            if (intTextBox.Text == "") { intTextBox.Text = "5"; }
        }

        public List<string> fileList = new List<string>();
        public string collectionFName = null;
        public int slideInterval;
         
        // add items to file list
        private void addButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDia = new OpenFileDialog();
            fileDia.Filter = "*.jpg;*.gif;*.png;*.bmp|*.jpg;*.gif;*.png;*.bmp|(*.*)All Files|*.*";
            fileDia.Multiselect = true;
            if (fileDia.ShowDialog() == DialogResult.OK)
            {
                foreach (string fname in fileDia.FileNames)
                    fileList.Add(fname);
            }
            fileNameListBox.DataSource = null;
            fileNameListBox.DataSource = fileList;
        }
        // delete items from file list
        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (fileList.Count == 0)
                return;
            for (int i = 0; i < fileNameListBox.SelectedItems.Count; i++)
                fileList.Remove((string)fileNameListBox.SelectedItems[i]);
            fileNameListBox.DataSource = null;
            fileNameListBox.DataSource = fileList;
        }
        // open .pix file
        private void openCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDia2 = new OpenFileDialog();
            fileDia2.Filter = "*.pix|*.pix";
            fileDia2.FilterIndex = 1;
            if (fileDia2.ShowDialog() == DialogResult.OK)
            {
                List<string> tempList = new List<string>();
                collectionFName = fileDia2.FileName;
                StreamReader fin = new StreamReader(fileDia2.OpenFile());
                while (true)
                {
                    string cur = fin.ReadLine();
                    if (cur == null)
                        break;
                    tempList.Add(cur);

                }
                fin.Close();
                if (tempList.Count == 0)
                {
                    MessageBox.Show("File is Empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                fileList.Clear();                                   // clear current list of files to open, 
                fileList = tempList;                                // set equal to temp list
                fileNameListBox.DataSource = null;
                fileNameListBox.DataSource = tempList;
            }
        }
        // save list of images to .pix file
        private void saveCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter fout = null;
            SaveFileDialog saveFileDia = new SaveFileDialog();
            saveFileDia.Filter = "*.pix|*.pix";
            if ((saveFileDia.ShowDialog() == DialogResult.OK) && saveFileDia.FileName != "")
            {
                try
                {
                    fout = new StreamWriter(saveFileDia.FileName);
                    foreach (string str in fileNameListBox.Items)
                        fout.WriteLine(str);
                    fout.Close();
                    MessageBox.Show("saved");
                }
                catch
                {
                    MessageBox.Show("Could not save file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (fout != null)
                        fout.Close();
                    return;
                }
            }
        }
        // exit program
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Close();
        }
        // start slideshow
        private void showButton_Click(object sender, EventArgs e)
        {
            if (fileNameListBox.Items.Count == 0)
            {
                MessageBox.Show("No images to show.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            // convert interval from string to int
            string stringInt = intTextBox.Text;
            slideInterval = Convert.ToInt32(stringInt);
            // if interval isn't valid
            if (slideInterval <= 0 || stringInt == "")
            {
                MessageBox.Show("Please enter an integer time value > 0 seconds.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                intTextBox.Text = "";
                return;
            }
            // initiate slide window
            (new ModalSlide() { Owner = this }).ShowDialog();
        }
    }

}
