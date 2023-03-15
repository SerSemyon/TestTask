namespace TestTask
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
            testElement1 = new TestElement();
            ((System.ComponentModel.ISupportInitialize)testElement1).BeginInit();
            SuspendLayout();
            // 
            // testElement1
            // 
            testElement1.Dock = DockStyle.Fill;
            testElement1.Location = new Point(0, 0);
            testElement1.Name = "testElement1";
            testElement1.Size = new Size(800, 450);
            testElement1.TabIndex = 0;
            testElement1.TabStop = false;
            testElement1.Click += testElement1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(testElement1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)testElement1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TestElement testElement1;
    }
}