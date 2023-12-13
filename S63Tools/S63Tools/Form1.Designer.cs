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
            textBoxUserPermit = new TextBox();
            label1 = new Label();
            buttonCalculate = new Button();
            labelMId = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            labelMKey = new Label();
            labelHwId = new Label();
            openFileDialogPermit = new OpenFileDialog();
            buttonDecryptCells = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            label5 = new Label();
            buttonCalculateFromCellPermit = new Button();
            SuspendLayout();
            // 
            // textBoxUserPermit
            // 
            textBoxUserPermit.Location = new Point(89, 12);
            textBoxUserPermit.Name = "textBoxUserPermit";
            textBoxUserPermit.Size = new Size(217, 23);
            textBoxUserPermit.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 1;
            label1.Text = "User Permit:";
            // 
            // buttonCalculate
            // 
            buttonCalculate.Location = new Point(312, 12);
            buttonCalculate.Name = "buttonCalculate";
            buttonCalculate.Size = new Size(75, 23);
            buttonCalculate.TabIndex = 2;
            buttonCalculate.Text = "Calculate";
            buttonCalculate.UseVisualStyleBackColor = true;
            buttonCalculate.Click += buttonCalculate_Click;
            // 
            // labelMId
            // 
            labelMId.AutoSize = true;
            labelMId.Location = new Point(89, 55);
            labelMId.Name = "labelMId";
            labelMId.Size = new Size(12, 15);
            labelMId.TabIndex = 3;
            labelMId.Text = "-";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(37, 15);
            label2.TabIndex = 4;
            label2.Text = "M_ID:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 79);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 5;
            label3.Text = "M_KEY:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 104);
            label4.Name = "label4";
            label4.Size = new Size(46, 15);
            label4.TabIndex = 6;
            label4.Text = "HW_ID:";
            // 
            // labelMKey
            // 
            labelMKey.AutoSize = true;
            labelMKey.Location = new Point(89, 79);
            labelMKey.Name = "labelMKey";
            labelMKey.Size = new Size(12, 15);
            labelMKey.TabIndex = 7;
            labelMKey.Text = "-";
            // 
            // labelHwId
            // 
            labelHwId.AutoSize = true;
            labelHwId.Location = new Point(89, 104);
            labelHwId.Name = "labelHwId";
            labelHwId.Size = new Size(12, 15);
            labelHwId.TabIndex = 8;
            labelHwId.Text = "-";
            // 
            // openFileDialogPermit
            // 
            openFileDialogPermit.FileName = "PERMIT.TXT";
            openFileDialogPermit.Filter = "Permit files|*.txt";
            // 
            // buttonDecryptCells
            // 
            buttonDecryptCells.Location = new Point(12, 145);
            buttonDecryptCells.Name = "buttonDecryptCells";
            buttonDecryptCells.Size = new Size(269, 79);
            buttonDecryptCells.TabIndex = 9;
            buttonDecryptCells.Text = "Decrypt cells. First select the PERMIT.TXT file, then select the (root) folder which contains the encrypted cells.";
            buttonDecryptCells.UseVisualStyleBackColor = true;
            buttonDecryptCells.Click += buttonDecryptCells_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(393, 15);
            label5.Name = "label5";
            label5.Size = new Size(23, 15);
            label5.TabIndex = 10;
            label5.Text = "OR";
            // 
            // buttonCalculateFromCellPermit
            // 
            buttonCalculateFromCellPermit.Location = new Point(422, 11);
            buttonCalculateFromCellPermit.Name = "buttonCalculateFromCellPermit";
            buttonCalculateFromCellPermit.Size = new Size(182, 23);
            buttonCalculateFromCellPermit.TabIndex = 11;
            buttonCalculateFromCellPermit.Text = "Calculate from PERMIT.TXT";
            buttonCalculateFromCellPermit.UseVisualStyleBackColor = true;
            buttonCalculateFromCellPermit.Click += buttonCalculateFromCellPermit_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonCalculateFromCellPermit);
            Controls.Add(label5);
            Controls.Add(buttonDecryptCells);
            Controls.Add(labelHwId);
            Controls.Add(labelMKey);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(labelMId);
            Controls.Add(buttonCalculate);
            Controls.Add(label1);
            Controls.Add(textBoxUserPermit);
            Name = "Form1";
            Text = "S63 Tools";
            ResumeLayout(false);
            PerformLayout();
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
        private Label label5;
        private Button buttonCalculateFromCellPermit;
    }
}