namespace AHMTrackingSuite
{
    partial class AHMTrackingPanel
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
            this.lightingCorrection = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxAutoStart = new System.Windows.Forms.ComboBox();
            this.NumTemplates = new System.Windows.Forms.ComboBox();
            this.comboBoxUpdateFequency = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxExtraDisplay = new System.Windows.Forms.CheckBox();
            this.comboBoxSetupType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lightingCorrection
            // 
            this.lightingCorrection.Location = new System.Drawing.Point(188, 15);
            this.lightingCorrection.Name = "lightingCorrection";
            this.lightingCorrection.Size = new System.Drawing.Size(120, 31);
            this.lightingCorrection.TabIndex = 5;
            this.lightingCorrection.Text = "Lighting Correction";
            this.lightingCorrection.UseVisualStyleBackColor = true;
            this.lightingCorrection.CheckedChanged += new System.EventHandler(this.lightingCorrection_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(119, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 31);
            this.label9.TabIndex = 2;
            this.label9.Text = "Number Templates:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.comboBoxAutoStart);
            this.groupBox4.Controls.Add(this.NumTemplates);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.comboBoxUpdateFequency);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.checkBoxExtraDisplay);
            this.groupBox4.Controls.Add(this.comboBoxSetupType);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.lightingCorrection);
            this.groupBox4.Location = new System.Drawing.Point(11, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(552, 77);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Auto Start:";
            // 
            // comboBoxAutoStart
            // 
            this.comboBoxAutoStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAutoStart.FormattingEnabled = true;
            this.comboBoxAutoStart.Items.AddRange(new object[] {
            "None",
            "LeftEye",
            "RightEye",
            "NoseMouth"});
            this.comboBoxAutoStart.Location = new System.Drawing.Point(70, 19);
            this.comboBoxAutoStart.Name = "comboBoxAutoStart";
            this.comboBoxAutoStart.Size = new System.Drawing.Size(82, 21);
            this.comboBoxAutoStart.TabIndex = 11;
            this.comboBoxAutoStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoStart_SelectedIndexChanged);
            // 
            // NumTemplates
            // 
            this.NumTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NumTemplates.FormattingEnabled = true;
            this.NumTemplates.Items.AddRange(new object[] {
            "4",
            "9",
            "16",
            "25"});
            this.NumTemplates.Location = new System.Drawing.Point(234, 47);
            this.NumTemplates.Name = "NumTemplates";
            this.NumTemplates.Size = new System.Drawing.Size(64, 21);
            this.NumTemplates.TabIndex = 1;
            this.NumTemplates.SelectedIndexChanged += new System.EventHandler(this.NumTemplates_SelectedIndexChanged);
            // 
            // comboBoxUpdateFequency
            // 
            this.comboBoxUpdateFequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUpdateFequency.FormattingEnabled = true;
            this.comboBoxUpdateFequency.Items.AddRange(new object[] {
            "Every Frame",
            "Once Per Second",
            "Twice Per Second",
            "Three Per Second"});
            this.comboBoxUpdateFequency.Location = new System.Drawing.Point(415, 47);
            this.comboBoxUpdateFequency.Name = "comboBoxUpdateFequency";
            this.comboBoxUpdateFequency.Size = new System.Drawing.Size(124, 21);
            this.comboBoxUpdateFequency.TabIndex = 10;
            this.comboBoxUpdateFequency.SelectedIndexChanged += new System.EventHandler(this.comboBoxUpdateFequency_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(314, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 27);
            this.label2.TabIndex = 9;
            this.label2.Text = "Update Frequency";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxExtraDisplay
            // 
            this.checkBoxExtraDisplay.AutoSize = true;
            this.checkBoxExtraDisplay.Location = new System.Drawing.Point(10, 49);
            this.checkBoxExtraDisplay.Name = "checkBoxExtraDisplay";
            this.checkBoxExtraDisplay.Size = new System.Drawing.Size(90, 17);
            this.checkBoxExtraDisplay.TabIndex = 8;
            this.checkBoxExtraDisplay.Text = "Extra Display ";
            this.checkBoxExtraDisplay.UseVisualStyleBackColor = true;
            this.checkBoxExtraDisplay.CheckedChanged += new System.EventHandler(this.checkBoxExtraDisplay_CheckedChanged);
            // 
            // comboBoxSetupType
            // 
            this.comboBoxSetupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetupType.FormattingEnabled = true;
            this.comboBoxSetupType.Items.AddRange(new object[] {
            "Natural Movement",
            "Key Press",
            "Movement - Infinite",
            "Movement - 30 Sec",
            "Movement - 45 Sec",
            "Movement - 60 Sec"});
            this.comboBoxSetupType.Location = new System.Drawing.Point(415, 20);
            this.comboBoxSetupType.Name = "comboBoxSetupType";
            this.comboBoxSetupType.Size = new System.Drawing.Size(124, 21);
            this.comboBoxSetupType.TabIndex = 7;
            this.comboBoxSetupType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSetupType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(337, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 27);
            this.label1.TabIndex = 6;
            this.label1.Text = "Setup Type:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AHMTrackingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "AHMTrackingPanel";
            this.Size = new System.Drawing.Size(574, 92);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox lightingCorrection;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox NumTemplates;
        private System.Windows.Forms.ComboBox comboBoxSetupType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxExtraDisplay;
        private System.Windows.Forms.ComboBox comboBoxUpdateFequency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxAutoStart;
    }
}
