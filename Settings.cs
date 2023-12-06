using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveExporter
{
    public partial class Settings : Form
    {
        public Form1 appForm;

        public Settings(Form1 parent)
        {
            InitializeComponent();
            this.appForm = parent;
            textBox1.Text = appForm.resFilesPath;
            textBox2.Text = appForm.resFilesIndex;
            checkBox1.Checked = appForm.convertToPNG;
            checkBox2.Checked = appForm.skipLowdetail;

            radioButton1.Hide();
            radioButton2.Hide();
            radioButton3.Hide();
            label4.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            appForm.LoadTreeView();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            appForm.convertToPNG = checkBox1.Checked;
            appForm.UpdateRegistry();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            appForm.skipLowdetail = checkBox2.Checked;
            appForm.UpdateRegistry();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a new instance of FolderBrowserDialog.
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                // Show the FolderBrowserDialog and capture the result.
                DialogResult result = fbd.ShowDialog();

                // Check if the user selected a folder and clicked the OK button.
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    // Do something with the selected folder.
                    // For instance, setting the path in a TextBox.
                    textBox1.Text = fbd.SelectedPath;
                    appForm.resFilesPath = fbd.SelectedPath;
                    appForm.UpdateRegistry();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create a new instance of FolderBrowserDialog.
            using (OpenFileDialog fbd = new OpenFileDialog())
            {
                // Show the FolderBrowserDialog and capture the result.
                DialogResult result = fbd.ShowDialog();

                // Check if the user selected a folder and clicked the OK button.
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    // Do something with the selected folder.
                    // For instance, setting the path in a TextBox.
                    textBox2.Text = fbd.FileName;
                    appForm.resFilesIndex = fbd.FileName;
                    appForm.UpdateRegistry();
                }
            }

        }
    }
}
