using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Rectangle = System.Drawing.Rectangle;
using BCnEncoder.Encoder;
using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using BCnEncoder.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;
using Microsoft.Win32;
using ImageMagick;
using LibVLCSharp.Shared;
using EveExporter.GrannyNative;
using BCnEncoder.Shared.ImageFiles;
using System.Diagnostics.Eventing.Reader;

namespace EveExporter
{


    public partial class Form1 : Form
    {
        public RegistryKey registryKey;

        public string resFilesPath = @"C:\EVE\SharedCache\ResFiles";
        public string resFilesIndex = @"C:\EVE\SharedCache\tq\resfileindex.txts";
        public bool convertToPNG = false;
        public bool skipLowdetail = false;
        ExportProgress progressForm = new ExportProgress();


        public Form1()
        {
            InitializeComponent();

            // get or create all the registry keys
            registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Eve Exporter", true) ?? Registry.CurrentUser.CreateSubKey("SOFTWARE\\Eve Exporter");
            resFilesPath = (string)registryKey.GetValue("resFilesPath", @"C:\EVE\SharedCache\ResFiles");
            resFilesIndex = (string)registryKey.GetValue("resFilesIndex", @"C:\EVE\SharedCache\tq\resfileindex.txt");
            convertToPNG = Convert.ToBoolean(registryKey.GetValue("convertToPNG", false));            
            skipLowdetail = Convert.ToBoolean(registryKey.GetValue("skipLowdetail", false));

            UpdateRegistry();

            // for testing and development of the gr2 loader
//            Granny granny = new Granny(
  //              new MemoryStream(File.ReadAllBytes(@"C:\debug\soef1_t1.gr2"))
    //            );


        }

        public void UpdateRegistry()
        {
            registryKey.SetValue("resFilesPath", resFilesPath);
            registryKey.SetValue("resFilesIndex", resFilesIndex);
            registryKey.SetValue("convertToPNG", convertToPNG);
            registryKey.SetValue("skipLowdetail", skipLowdetail);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTreeView();
        }

        // Loads a preview of the selected file in the picture box and sets up the 'info' text box
        private void PrevewTreeNode(TreeNode node)
        {
            pictureBox1.Image = null;
            pictureBox1.Hide();
//            modelPreview1.Hide();
            videoPreview1.Hide();

            richTextBox1.Text = "";
            Cursor.Current = Cursors.WaitCursor;

            // The full file path is stored in the Name property
            string filePath = resFilesPath + @"\" + (string)node.Tag;
            Debug.WriteLine("File path is " + filePath);
            Debug.WriteLine("Node name is " + node.Text.ToLower());

            if (node.Text.ToLower().EndsWith(".webm"))
            {
                videoPreview1.Show();
                videoPreview1.SetVideo(filePath);
//                videoPreview1.Play();
            }


            //if (node.Text.ToLower().EndsWith(".gr2"))
            //{
            //    modelPreview1.Show();
//                modelPreview1.SetModel(filePath);
            //}   

            if (node.Text.ToLower().EndsWith(".png") | node.Text.ToLower().EndsWith(".jpg"))
            {
                pictureBox1.Show();

                try
                {
                    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
                    {
                        var image = Image.Load(fileStream);
                        Debug.WriteLine("Image loaded...");
                        MemoryStream memstream = new MemoryStream();
                        image.SaveAsPng(memstream);
                        memstream.Position = 0;
                        pictureBox1.Image = Bitmap.FromStream(memstream);
                        Debug.WriteLine("Image displayed...");

                        richTextBox1.Text = "Width: " + image.Width + "\nHeight: " + image.Height + "\nFormat: " + image.PixelType;

                    }

                }
                catch
                {
                    Debug.WriteLine("Failed to load PNG or JPG file");
                }
            }

            if (node.Text.ToLower().EndsWith(".dds"))
            {
                pictureBox1.Show();
                richTextBox1.Text = "";

                try
                {
                    System.Drawing.Image image;
                    DDSFile ddsFile = new DDSFile(filePath); // this will dump the headers for troubleshooting

                    string ddsCompression = "Unknown";

                    if ((ddsFile.header.Ddspf.DwFlags & (uint)0x4) != 0) { ddsCompression = "Compressed"; }

                    // switch statement based on if ddsFile.header.Ddspf.DwFourCC is "DXT1" or "DXT5"
                    switch (ddsFile.header.Ddspf.DwFourCC)
                    {
                        case DDSFile.DXT1:
                            ddsCompression += " DXT1";
                            break;
                        case DDSFile.DXT2:
                            ddsCompression += " DXT2";
                            break;
                        case DDSFile.DXT3:
                            ddsCompression += " DXT3";
                            break;
                        case DDSFile.DXT4:
                            ddsCompression += " DXT4";
                            break;
                        case DDSFile.DXT5:
                            ddsCompression += " DXT5";
                            break;
                        case DDSFile.DX10:
                            ddsCompression += " DX10";
                            break;
                        case DDSFile.ATI1:
                            ddsCompression += " ATI1/BC4";
                            break;
                        case DDSFile.ATI2:
                            ddsCompression += " ATI2/BC5";
                            break;

                        default:
                            ddsCompression += " Unknown (" + ddsFile.header.Ddspf.DwFourCC + ")";
                            break;
                    }


                    richTextBox1.Text += "Width: " + ddsFile.header.DwWidth + "\n" +
                                        "Height: " + ddsFile.header.DwHeight + "\n" +
                                        "Compression: " + ddsCompression + "\n";


                    try
                    {
                        image = loadDDSWithMagick(filePath);
                        pictureBox1.Image = image;
                        richTextBox1.Text += "Loaded DDS file with ImageMagick\n";
                        richTextBox1.Text += "Format: " + image.PixelFormat + "\n";

                    }
                    catch (Exception e)
                    {
                        richTextBox1.Text += "Failed to load DDS file with ImageMagick\n";
  //                      richTextBox1.Text += e.ToString();



                        try
                        {
                            image = loadDDSWithSharp(filePath);
                            pictureBox1.Image = image;
                            richTextBox1.Text += "Loaded DDS file with ImageSharp BCnDecoder\n";
                            richTextBox1.Text += "Format: " + image.PixelFormat + "\n";

                        }
                        catch (Exception ex)
                        {
                            richTextBox1.Text += "Failed to load DDS file with ImageSharp BCnDecoder\n";
//                            richTextBox1.Text += ex.ToString();

                            ddsFile.DumpHeaders();
                        }
                    }

                }

                catch
                {
                    Debug.WriteLine("Failed to load DDS file for unknown reason");

                }
            }

            Cursor.Current = Cursors.Default;

        }

        private void treeView1_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private System.Drawing.Image loadDDSWithMagick(string filePath)
        {
            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                var image = new MagickImage(fileStream);
                MemoryStream memstream = new MemoryStream();
                image.Write(memstream, MagickFormat.Png);
                memstream.Position = 0;
                return Bitmap.FromStream(memstream);
            }
        }


        private System.Drawing.Image loadDDSWithSharp(string filePath)
        {
            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                BcDecoder decoder = new BcDecoder();
                using Image image = decoder.DecodeToImageRgba32(fileStream);
                MemoryStream memstream = new MemoryStream();
                image.SaveAsPng(memstream);
                memstream.Position = 0;
                return Bitmap.FromStream(memstream);
            }
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                PrevewTreeNode(e.Node);
                button1.Enabled = true; // Enable the button if the selected node is a leaf node
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // folderDialog.SelectedPath contains the path of the selected folder 
                    string selectedFolder = folderDialog.SelectedPath;

                    // Here you can process the selected folder.
                    // ...


                    progressForm.progressBar1.Minimum = 0;
                    progressForm.progressBar1.Maximum = CountNodes(treeView1.SelectedNode);
                    progressForm.progressBar1.Value = 1;
                    progressForm.progressBar1.Step = 1;
                    progressForm.Show();

                    ExportTree(treeView1.SelectedNode, selectedFolder);

                    progressForm.Hide();

                }
            }
        }

        // this should return the number of leaf nodes
        private int CountNodes(TreeNode node)
        {
            int count = 0;
            if (node.Nodes.Count > 0)
            {
                // I am not a leaf node and I need to recurse
                foreach (TreeNode childNode in node.Nodes)
                {
                    count += CountNodes(childNode);
                }
            }
            else
            {
                return 1;
            }

            return count;
        }

        // recusively saves a node tree into the destination path
        private void ExportTree(TreeNode node, string path)
        {
 

            if (node.Nodes.Count > 0) // If it is a non-leaf node
            {
                string directoryPath = Path.Combine(path, node.Text);
                Debug.WriteLine("Creating folder " + directoryPath);
                Directory.CreateDirectory(directoryPath);

                foreach (TreeNode childNode in node.Nodes)
                {
                    ExportTree(childNode, directoryPath); // Recursive call for each child node
                }
            }
            else // It's a leaf node
            {
                ExportNode(Path.Combine(path, node.Text), node); // Call the SaveNode method

//                if (progressForm.progressBar1.Value < progressForm.progressBar1.Maximum)
  //              {
    //                
      //          }

            }
        }

        // saves a single leaf node to the destination path which includes the filename
        private void ExportNode(string path, TreeNode node)
        {

            progressForm.progressBar1.PerformStep();

            if ((skipLowdetail) && (node.Text.ToLower().Contains("lowdetail"))){
                return;
            }

            string sourcePath = resFilesPath + @"\" + (string)node.Tag;

            if (node.Text.ToLower().EndsWith(".dds") && convertToPNG)
            {
                Debug.WriteLine("Exporting file " + sourcePath + " to " + path + " as png");
                System.Drawing.Image? image = null;

                try
                {
                    image = loadDDSWithMagick(sourcePath);
                }
                catch
                {
                    try
                    {
                        image = loadDDSWithSharp(sourcePath);
                    }
                    catch
                    {
                        // none
                    }
                }

                if (image != null)
                {
                    string newFilename = path.Substring(0, path.Length - 4) + ".png";

                    byte[] imageBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, image.RawFormat);
                        ms.Position = 0;

                        using (MagickImage magickImage = new MagickImage(ms))
                        {
                            // Create write settings for the MagickImage with no compression
                            MagickReadSettings readSettings = new MagickReadSettings()
                            {
                                Compression = CompressionMethod.NoCompression
                            };

                            // Apply the read settings and set the format
                            magickImage.Read(ms, readSettings);
                            magickImage.Format = MagickFormat.Png;

                            // Save the image to a file with no compression
                            magickImage.Write(newFilename);
                        }

                    }
                }
            }
            else
            {
                Debug.WriteLine("Exporting file " + sourcePath + " to " + path + " as " + node.Text);
                try
                {
                    File.Copy(sourcePath, path, true);
                }
                catch { }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings(this);
            settingsForm.ShowDialog();

        }
    }
}