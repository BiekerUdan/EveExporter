using System.Diagnostics;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;

namespace EveExporter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            splitContainer2 = new SplitContainer();
            panel1 = new Panel();
//            modelPreview1 = new ModelPreview();
            videoPreview1 = new VideoPreview();
            pictureBox1 = new PictureBox();
            groupBox1 = new GroupBox();
            richTextBox1 = new RichTextBox();
            button2 = new Button();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.ImeMode = ImeMode.On;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(3, 4, 3, 4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2MinSize = 250;
            splitContainer1.Size = new Size(1050, 829);
            splitContainer1.SplitterDistance = 344;
            splitContainer1.SplitterWidth = 11;
            splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeView1.Location = new Point(14, 16);
            treeView1.Margin = new Padding(3, 4, 3, 4);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(326, 800);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick_1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(3, 4, 3, 4);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(panel1);
            splitContainer2.Panel1.RightToLeft = RightToLeft.No;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox1);
            splitContainer2.Panel2.RightToLeft = RightToLeft.No;
            splitContainer2.RightToLeft = RightToLeft.No;
            splitContainer2.Size = new Size(695, 829);
            splitContainer2.SplitterDistance = 543;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            //            panel1.Controls.Add(modelPreview1);
            panel1.Controls.Add(videoPreview1);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(10, 16);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(670, 523);
            panel1.TabIndex = 2;
            // 
            // videoPreview1
            // 
            videoPreview1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            videoPreview1.Location = new Point(0, 0);
            videoPreview1.Name = "videoPreview1";
            videoPreview1.Size = new Size(670, 523);
            videoPreview1.TabIndex = 1;
             
            // modelPreview1
            // 
            //            modelPreview1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            //           modelPreview1.Location = new Point(0, 0);
            //          modelPreview1.Name = "modelPreview1";
            //         modelPreview1.Size = new Size(670, 523);
            //        modelPreview1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(3, 4);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(658, 511);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(richTextBox1);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(3, 4);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(678, 255);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Info";
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(7, 29);
            richTextBox1.Margin = new Padding(3, 4, 3, 4);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(663, 178);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Location = new Point(484, 216);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(86, 29);
            button2.TabIndex = 1;
            button2.Text = "Settings";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Enabled = false;
            button1.Location = new Point(576, 216);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "Export";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1050, 829);
            Controls.Add(splitContainer1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(600, 600);
            Name = "Form1";
            Text = "Eve Exporter";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TreeView treeView1;
        private GroupBox groupBox1;



        public void LoadTreeView()
        {
            string path = resFilesIndex;

            // Create the dictionary to hold a reference to each TreeNode
            var nodeDict = new Dictionary<string, TreeNode>();

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                // Create the root TreeNode
                TreeNode rootNode = new TreeNode("root");
                nodeDict.Add("root", rootNode);

                foreach (string line in lines)
                {
                    // split the line into parts (its a comma separated file)
                    string[] parts = line.Split(',');
                    if (parts.Length > 0)  // looks like a valid line
                    {
                        // The first part is the 'virtual' file path
                        var filePath = parts[0];

                        // Generate a list of keys for each part of the file path
                        String[] keys = filePath.Split('/');
                        string previousKey = "root";

                        for (int i = 0; i < keys.Length; i++)
                        {
                            string fullKey = previousKey + "/" + keys[i];

                            // If the TreeNode doesn't already exist in the dictionary, create it
                            if (!nodeDict.ContainsKey(fullKey))
                            {
                                TreeNode newNode = new TreeNode(keys[i]);
                                if (i == keys.Length - 1)
                                {
                                    newNode.Tag = parts[1];
                                }
                                else
                                {
                                    newNode.Tag = "folder";
                                }

                                nodeDict[previousKey].Nodes.Add(newNode);
                                nodeDict.Add(fullKey, newNode);
                            }

                            previousKey = fullKey;
                        }
                    }
                }

                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(rootNode);
                treeView1.EndUpdate();
            }
            else
            {
                MessageBox.Show("File not found: " + path);
            }
        }

        private Button button1;
        private Button button2;
        private RichTextBox richTextBox1;
        private Panel panel1;
        private PictureBox pictureBox1;
        private ModelPreview modelPreview1;
        private VideoPreview videoPreview1;
    }
}