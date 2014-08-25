namespace BlinkLinkStandardTrackingSuite
{
    partial class BlinkLinkClickControlSimplePanel
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
            if( disposing && (components != null) )
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.playSoundCheckBox = new System.Windows.Forms.CheckBox();
            this.shortWinkTimeTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.switchEyesCheckBox = new System.Windows.Forms.CheckBox();
            this.statusWindowComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.playSoundCheckBox);
            this.groupBox1.Controls.Add(this.shortWinkTimeTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.switchEyesCheckBox);
            this.groupBox1.Controls.Add(this.statusWindowComboBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // playSoundCheckBox
            // 
            this.playSoundCheckBox.AutoSize = true;
            this.playSoundCheckBox.Checked = true;
            this.playSoundCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playSoundCheckBox.Location = new System.Drawing.Point(96, 19);
            this.playSoundCheckBox.Name = "playSoundCheckBox";
            this.playSoundCheckBox.Size = new System.Drawing.Size(80, 17);
            this.playSoundCheckBox.TabIndex = 74;
            this.playSoundCheckBox.Text = "Play Sound";
            this.playSoundCheckBox.UseVisualStyleBackColor = true;
            this.playSoundCheckBox.CheckedChanged += new System.EventHandler(this.playSoundCheckBox_CheckedChanged);
            // 
            // shortWinkTimeTextBox
            // 
            this.shortWinkTimeTextBox.Location = new System.Drawing.Point(296, 17);
            this.shortWinkTimeTextBox.Name = "shortWinkTimeTextBox";
            this.shortWinkTimeTextBox.Size = new System.Drawing.Size(112, 20);
            this.shortWinkTimeTextBox.TabIndex = 73;
            this.shortWinkTimeTextBox.TextChanged += new System.EventHandler(this.shortWinkTimeTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(182, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 72;
            this.label6.Text = "Blink Time (seconds):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(414, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 13);
            this.label9.TabIndex = 71;
            this.label9.Text = "Eye Status Window:";
            // 
            // switchEyesCheckBox
            // 
            this.switchEyesCheckBox.AutoSize = true;
            this.switchEyesCheckBox.Checked = true;
            this.switchEyesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.switchEyesCheckBox.Location = new System.Drawing.Point(6, 19);
            this.switchEyesCheckBox.Name = "switchEyesCheckBox";
            this.switchEyesCheckBox.Size = new System.Drawing.Size(84, 17);
            this.switchEyesCheckBox.TabIndex = 67;
            this.switchEyesCheckBox.Text = "Switch Eyes";
            this.switchEyesCheckBox.UseVisualStyleBackColor = true;
            this.switchEyesCheckBox.CheckedChanged += new System.EventHandler(this.switchEyesCheckBox_CheckedChanged);
            // 
            // statusWindowComboBox
            // 
            this.statusWindowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statusWindowComboBox.FormattingEnabled = true;
            this.statusWindowComboBox.Location = new System.Drawing.Point(523, 17);
            this.statusWindowComboBox.Name = "statusWindowComboBox";
            this.statusWindowComboBox.Size = new System.Drawing.Size(121, 21);
            this.statusWindowComboBox.TabIndex = 69;
            this.statusWindowComboBox.SelectedIndexChanged += new System.EventHandler(this.statusWindowComboBox_SelectedIndexChanged);
            // 
            // BlinkLinkClickControlSimplePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "BlinkLinkClickControlSimplePanel";
            this.Size = new System.Drawing.Size(650, 52);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox switchEyesCheckBox;
        private System.Windows.Forms.ComboBox statusWindowComboBox;
        private System.Windows.Forms.CheckBox playSoundCheckBox;
        private System.Windows.Forms.TextBox shortWinkTimeTextBox;
        private System.Windows.Forms.Label label6;
    }
}
