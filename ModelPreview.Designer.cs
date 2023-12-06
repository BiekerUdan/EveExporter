namespace EveExporter
{
    partial class ModelPreview
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GLPanel = new Panel();
            ButtonPanel = new Panel();
            button1 = new Button();
            ButtonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // GLPanel
            // 
            GLPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GLPanel.Location = new System.Drawing.Point(6, 3);
            GLPanel.Name = "GLPanel";
            GLPanel.Size = new System.Drawing.Size(624, 449);
            GLPanel.TabIndex = 0;
            // 
            // ButtonPanel
            // 
            ButtonPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ButtonPanel.Controls.Add(button1);
            ButtonPanel.Location = new System.Drawing.Point(3, 458);
            ButtonPanel.Name = "ButtonPanel";
            ButtonPanel.Size = new System.Drawing.Size(627, 33);
            ButtonPanel.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(3, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // ModelPreview
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ButtonPanel);
            Controls.Add(GLPanel);
            Name = "ModelPreview";
            Size = new System.Drawing.Size(633, 494);
            ButtonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel GLPanel;
        private Panel ButtonPanel;
        private Button button1;
    }
}
