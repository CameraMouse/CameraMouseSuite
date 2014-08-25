namespace AHMTrackingSuite
{
    partial class AHMClickMovementPanel
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
            this.comboBoxAutoStart = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxUpdateFequency = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxAutoStart);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.comboBoxUpdateFequency);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(11, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(395, 54);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            // 
            // comboBoxAutoStart
            // 
            this.comboBoxAutoStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAutoStart.FormattingEnabled = true;
            this.comboBoxAutoStart.Items.AddRange(new object[] {
            "None",
            "Left Eyebrow",
            "Right Eyebrow"});
            this.comboBoxAutoStart.Location = new System.Drawing.Point(66, 21);
            this.comboBoxAutoStart.Name = "comboBoxAutoStart";
            this.comboBoxAutoStart.Size = new System.Drawing.Size(109, 21);
            this.comboBoxAutoStart.TabIndex = 13;
            this.comboBoxAutoStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxAutoStart_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
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
            this.comboBoxUpdateFequency.Location = new System.Drawing.Point(308, 21);
            this.comboBoxUpdateFequency.Name = "comboBoxUpdateFequency";
            this.comboBoxUpdateFequency.Size = new System.Drawing.Size(74, 21);
            this.comboBoxUpdateFequency.TabIndex = 10;
            this.comboBoxUpdateFequency.SelectedIndexChanged += new System.EventHandler(this.comboBoxUpdateFequency_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(189, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 27);
            this.label2.TabIndex = 9;
            this.label2.Text = "Correction Frequency";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AHMClickMovementPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "AHMClickMovementPanel";
            this.Size = new System.Drawing.Size(418, 73);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBoxAutoStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxUpdateFequency;
        private System.Windows.Forms.Label label2;
    }
}
