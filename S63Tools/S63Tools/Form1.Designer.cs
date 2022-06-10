namespace S63Tools
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
            this.textBoxUserPermit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.labelMId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelMKey = new System.Windows.Forms.Label();
            this.labelHwId = new System.Windows.Forms.Label();
            this.openFileDialogPermit = new System.Windows.Forms.OpenFileDialog();
            this.buttonDecryptCells = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // textBoxUserPermit
            // 
            this.textBoxUserPermit.Location = new System.Drawing.Point(89, 12);
            this.textBoxUserPermit.Name = "textBoxUserPermit";
            this.textBoxUserPermit.Size = new System.Drawing.Size(217, 23);
            this.textBoxUserPermit.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Permit:";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(312, 12);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(75, 23);
            this.buttonCalculate.TabIndex = 2;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // labelMId
            // 
            this.labelMId.AutoSize = true;
            this.labelMId.Location = new System.Drawing.Point(89, 55);
            this.labelMId.Name = "labelMId";
            this.labelMId.Size = new System.Drawing.Size(12, 15);
            this.labelMId.TabIndex = 3;
            this.labelMId.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "M_ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "M_KEY:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "HW_ID:";
            // 
            // labelMKey
            // 
            this.labelMKey.AutoSize = true;
            this.labelMKey.Location = new System.Drawing.Point(89, 79);
            this.labelMKey.Name = "labelMKey";
            this.labelMKey.Size = new System.Drawing.Size(12, 15);
            this.labelMKey.TabIndex = 7;
            this.labelMKey.Text = "-";
            // 
            // labelHwId
            // 
            this.labelHwId.AutoSize = true;
            this.labelHwId.Location = new System.Drawing.Point(89, 104);
            this.labelHwId.Name = "labelHwId";
            this.labelHwId.Size = new System.Drawing.Size(12, 15);
            this.labelHwId.TabIndex = 8;
            this.labelHwId.Text = "-";
            // 
            // openFileDialogPermit
            // 
            this.openFileDialogPermit.FileName = "PERMIT.TXT";
            this.openFileDialogPermit.Filter = "Permit files|*.txt";
            // 
            // buttonDecryptCells
            // 
            this.buttonDecryptCells.Location = new System.Drawing.Point(12, 145);
            this.buttonDecryptCells.Name = "buttonDecryptCells";
            this.buttonDecryptCells.Size = new System.Drawing.Size(269, 79);
            this.buttonDecryptCells.TabIndex = 9;
            this.buttonDecryptCells.Text = "Decrypt cells. First select the PERMIT.TXT file, then select the (root) folder wh" +
    "ich contains the encrypted cells.";
            this.buttonDecryptCells.UseVisualStyleBackColor = true;
            this.buttonDecryptCells.Click += new System.EventHandler(this.buttonDecryptCells_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonDecryptCells);
            this.Controls.Add(this.labelHwId);
            this.Controls.Add(this.labelMKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelMId);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUserPermit);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBoxUserPermit;
        private Label label1;
        private Button buttonCalculate;
        private Label labelMId;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label labelMKey;
        private Label labelHwId;
        private OpenFileDialog openFileDialogPermit;
        private Button buttonDecryptCells;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}