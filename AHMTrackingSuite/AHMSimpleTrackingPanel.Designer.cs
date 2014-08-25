namespace AHMTrackingSuite
{
    partial class AHMSimpleTrackingPanel
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxUpdateFequency = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSetupType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxAutoStart);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.comboBoxUpdateFequency);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.comboBoxSetupType);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(11, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(552, 54);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Auto Start";
            // 
            // comboBoxUpdateFequency
            // 
            this.comboBoxUpdateFequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUpdateFequency.FormattingEnabled = true;
            this.comboBoxUpdateFequency.Items.AddRange(new object[] {
            "Fast",
            "Slow"});
            this.comboBoxUpdateFequency.Location = new System.Drawing.Point(225, 20);
            this.comboBoxUpdateFequency.Name = "comboBoxUpdateFequency";
            this.comboBoxUpdateFequency.Size = new System.Drawing.Size(82, 21);
            this.comboBoxUpdateFequency.TabIndex = 10;
            this.comboBoxUpdateFequency.SelectedIndexChanged += new System.EventHandler(this.comboBoxUpdateFequency_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(137, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 27);
            this.label2.TabIndex = 9;
            this.label2.Text = "Correction Frequency";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxSetupType
            // 
            this.comboBoxSetupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSetupType.FormattingEnabled = true;
            this.comboBoxSetupType.Items.AddRange(new object[] {
            "Natural Movement",
            "Rectangle Movement"});
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
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(95, 24);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(15, 14);
            this.checkBoxAutoStart.TabIndex = 13;
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.CheckedChanged += new System.EventHandler(this.checkBoxAutoStart_CheckedChanged);
            // 
            // AHMSimpleTrackingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "AHMSimpleTrackingPanel";
            this.Size = new System.Drawing.Size(574, 73);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxUpdateFequency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSetupType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
    }
}
