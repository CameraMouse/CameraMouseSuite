namespace BlinkLinkStandardTrackingSuite
{
    partial class BlinkLinkClickControlPanel
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
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.statusWindowComboBox = new System.Windows.Forms.ComboBox();
            this.soundComboBox = new System.Windows.Forms.ComboBox();
            this.switchEyesCheckBox = new System.Windows.Forms.CheckBox();
            this.shortWinkTimeTextBox = new System.Windows.Forms.TextBox();
            this.longWinkTimeTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.blinkActionComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.longRightWinkActionComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.longLeftWinkActionComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.shortRightWinkActionComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.shortLeftWinkActionComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.statusWindowComboBox);
            this.groupBox1.Controls.Add(this.soundComboBox);
            this.groupBox1.Controls.Add(this.switchEyesCheckBox);
            this.groupBox1.Controls.Add(this.shortWinkTimeTextBox);
            this.groupBox1.Controls.Add(this.longWinkTimeTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.blinkActionComboBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.longRightWinkActionComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.longLeftWinkActionComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.shortRightWinkActionComboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.shortLeftWinkActionComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 192);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(393, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 13);
            this.label9.TabIndex = 66;
            this.label9.Text = "Eye Status Window:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(100, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 65;
            this.label8.Text = "Sound:";
            // 
            // statusWindowComboBox
            // 
            this.statusWindowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.statusWindowComboBox.FormattingEnabled = true;
            this.statusWindowComboBox.Location = new System.Drawing.Point(502, 16);
            this.statusWindowComboBox.Name = "statusWindowComboBox";
            this.statusWindowComboBox.Size = new System.Drawing.Size(121, 21);
            this.statusWindowComboBox.TabIndex = 64;
            this.statusWindowComboBox.SelectedIndexChanged += new System.EventHandler(this.statusWindowComboBox_SelectedIndexChanged);
            // 
            // soundComboBox
            // 
            this.soundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.soundComboBox.FormattingEnabled = true;
            this.soundComboBox.Location = new System.Drawing.Point(147, 16);
            this.soundComboBox.Name = "soundComboBox";
            this.soundComboBox.Size = new System.Drawing.Size(121, 21);
            this.soundComboBox.TabIndex = 63;
            this.soundComboBox.SelectedIndexChanged += new System.EventHandler(this.soundComboBox_SelectedIndexChanged);
            // 
            // switchEyesCheckBox
            // 
            this.switchEyesCheckBox.AutoSize = true;
            this.switchEyesCheckBox.Checked = true;
            this.switchEyesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.switchEyesCheckBox.Location = new System.Drawing.Point(6, 18);
            this.switchEyesCheckBox.Name = "switchEyesCheckBox";
            this.switchEyesCheckBox.Size = new System.Drawing.Size(84, 17);
            this.switchEyesCheckBox.TabIndex = 62;
            this.switchEyesCheckBox.Text = "Switch Eyes";
            this.switchEyesCheckBox.UseVisualStyleBackColor = true;
            this.switchEyesCheckBox.CheckedChanged += new System.EventHandler(this.switchEyesCheckBox_CheckedChanged);
            // 
            // shortWinkTimeTextBox
            // 
            this.shortWinkTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.shortWinkTimeTextBox.Location = new System.Drawing.Point(147, 48);
            this.shortWinkTimeTextBox.Name = "shortWinkTimeTextBox";
            this.shortWinkTimeTextBox.Size = new System.Drawing.Size(121, 20);
            this.shortWinkTimeTextBox.TabIndex = 55;
            this.shortWinkTimeTextBox.TextChanged += new System.EventHandler(this.shortWinkTimeTextBox_TextChanged);
            // 
            // longWinkTimeTextBox
            // 
            this.longWinkTimeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.longWinkTimeTextBox.Location = new System.Drawing.Point(502, 48);
            this.longWinkTimeTextBox.Name = "longWinkTimeTextBox";
            this.longWinkTimeTextBox.Size = new System.Drawing.Size(121, 20);
            this.longWinkTimeTextBox.TabIndex = 54;
            this.longWinkTimeTextBox.TextChanged += new System.EventHandler(this.longWinkTimeTextBox_TextChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(359, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Long Wink Time (seconds):";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 52;
            this.label6.Text = "Short Wink Time (seconds):";
            // 
            // blinkActionComboBox
            // 
            this.blinkActionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blinkActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blinkActionComboBox.FormattingEnabled = true;
            this.blinkActionComboBox.Location = new System.Drawing.Point(147, 154);
            this.blinkActionComboBox.Name = "blinkActionComboBox";
            this.blinkActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.blinkActionComboBox.TabIndex = 51;
            this.blinkActionComboBox.SelectedIndexChanged += new System.EventHandler(this.blinkActionComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Blink Action:";
            // 
            // longRightWinkActionComboBox
            // 
            this.longRightWinkActionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.longRightWinkActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.longRightWinkActionComboBox.FormattingEnabled = true;
            this.longRightWinkActionComboBox.Location = new System.Drawing.Point(502, 121);
            this.longRightWinkActionComboBox.Name = "longRightWinkActionComboBox";
            this.longRightWinkActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.longRightWinkActionComboBox.TabIndex = 49;
            this.longRightWinkActionComboBox.SelectedIndexChanged += new System.EventHandler(this.longRightWinkActionComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(373, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Long Right Wink Action:";
            // 
            // longLeftWinkActionComboBox
            // 
            this.longLeftWinkActionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.longLeftWinkActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.longLeftWinkActionComboBox.FormattingEnabled = true;
            this.longLeftWinkActionComboBox.Location = new System.Drawing.Point(502, 84);
            this.longLeftWinkActionComboBox.Name = "longLeftWinkActionComboBox";
            this.longLeftWinkActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.longLeftWinkActionComboBox.TabIndex = 47;
            this.longLeftWinkActionComboBox.SelectedIndexChanged += new System.EventHandler(this.longLeftWinkActionComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(380, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Long Left Wink Action:";
            // 
            // shortRightWinkActionComboBox
            // 
            this.shortRightWinkActionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shortRightWinkActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shortRightWinkActionComboBox.FormattingEnabled = true;
            this.shortRightWinkActionComboBox.Location = new System.Drawing.Point(147, 116);
            this.shortRightWinkActionComboBox.Name = "shortRightWinkActionComboBox";
            this.shortRightWinkActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.shortRightWinkActionComboBox.TabIndex = 45;
            this.shortRightWinkActionComboBox.SelectedIndexChanged += new System.EventHandler(this.shortRightWinkActionComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "Short Right Wink Action:";
            // 
            // shortLeftWinkActionComboBox
            // 
            this.shortLeftWinkActionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.shortLeftWinkActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shortLeftWinkActionComboBox.FormattingEnabled = true;
            this.shortLeftWinkActionComboBox.Location = new System.Drawing.Point(147, 79);
            this.shortLeftWinkActionComboBox.Name = "shortLeftWinkActionComboBox";
            this.shortLeftWinkActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.shortLeftWinkActionComboBox.TabIndex = 43;
            this.shortLeftWinkActionComboBox.SelectedIndexChanged += new System.EventHandler(this.shortLeftWinkActionComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Short Left Wink Action:";
            // 
            // BlinkLinkClickControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "BlinkLinkClickControlPanel";
            this.Size = new System.Drawing.Size(650, 196);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox shortWinkTimeTextBox;
        private System.Windows.Forms.TextBox longWinkTimeTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox blinkActionComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox longRightWinkActionComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox longLeftWinkActionComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox shortRightWinkActionComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox shortLeftWinkActionComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox switchEyesCheckBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox statusWindowComboBox;
        private System.Windows.Forms.ComboBox soundComboBox;


    }
}
