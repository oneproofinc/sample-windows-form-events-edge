namespace EdgeUSBEvents
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.CheckBox verifyCheckBox;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.TextBox dataTextBox;



        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.verifyCheckBox = new System.Windows.Forms.CheckBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 10);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(150, 30);
            this.startButton.Text = "Start Monitoring";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // verifyCheckBox
            // 
            this.verifyCheckBox.Location = new System.Drawing.Point(180, 15);
            this.verifyCheckBox.Name = "verifyCheckBox";
            this.verifyCheckBox.Size = new System.Drawing.Size(80, 20);
            this.verifyCheckBox.Text = "Verify";
            this.verifyCheckBox.UseVisualStyleBackColor = true;
            this.verifyCheckBox.Checked = true;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(10, 50);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(560, 300);
            this.outputTextBox.ReadOnly = true;
            // 
            // dataLabel
            // 
            this.dataLabel = new System.Windows.Forms.Label();
            this.dataLabel.Location = new System.Drawing.Point(10, 360);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(100, 20);
            this.dataLabel.Text = "Last Data:";

            // 
            // dataTextBox
            // 
            this.dataTextBox = new System.Windows.Forms.TextBox();
            this.dataTextBox.Location = new System.Drawing.Point(120, 360);
            this.dataTextBox.Name = "dataTextBox";
            this.dataTextBox.Size = new System.Drawing.Size(450, 22);
            this.dataTextBox.ReadOnly = true;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 370);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.verifyCheckBox);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.dataTextBox);

            this.Name = "Form1";
            this.Text = "USB Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
