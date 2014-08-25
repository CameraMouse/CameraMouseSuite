namespace CameraMouseSuite
{
    partial class StandardClickControlPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardClickControlPanel));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.clickRadius = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.click_sound = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dwell = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.clicking = new System.Windows.Forms.CheckBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.clickRadius);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.click_sound);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.dwell);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.clicking);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox4.Location = new System.Drawing.Point(8, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(579, 44);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            // 
            // clickRadius
            // 
            this.clickRadius.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clickRadius.FormattingEnabled = true;
            this.clickRadius.Items.AddRange(new object[] {
            "Small",
            "Normal",
            "Large"});
            this.clickRadius.Location = new System.Drawing.Point(152, 15);
            this.clickRadius.Name = "clickRadius";
            this.clickRadius.Size = new System.Drawing.Size(64, 21);
            this.clickRadius.TabIndex = 1;
            this.clickRadius.SelectedIndexChanged += new System.EventHandler(this.clickRadius_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.ImageIndex = 3;
            this.label11.ImageList = this.imageList1;
            this.label11.Location = new System.Drawing.Point(541, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 23);
            this.label11.TabIndex = 6;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0.bmp");
            this.imageList1.Images.SetKeyName(1, "1.bmp");
            this.imageList1.Images.SetKeyName(2, "2.bmp");
            this.imageList1.Images.SetKeyName(3, "3.bmp");
            this.imageList1.Images.SetKeyName(4, "4.bmp");
            this.imageList1.Images.SetKeyName(5, "5.bmp");
            this.imageList1.Images.SetKeyName(6, "6.bmp");
            // 
            // click_sound
            // 
            this.click_sound.Location = new System.Drawing.Point(423, 10);
            this.click_sound.Name = "click_sound";
            this.click_sound.Size = new System.Drawing.Size(145, 31);
            this.click_sound.TabIndex = 5;
            this.click_sound.Text = "Play Clicking Sounds";
            this.click_sound.UseVisualStyleBackColor = true;
            this.click_sound.CheckedChanged += new System.EventHandler(this.click_sound_CheckedChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(244, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 22);
            this.label10.TabIndex = 4;
            this.label10.Text = "Dwell Time:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dwell
            // 
            this.dwell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dwell.FormattingEnabled = true;
            this.dwell.Items.AddRange(new object[] {
            "0.1 Sec",
            "0.25 Sec",
            "0.5 Sec",
            "0.75 Sec",
            "1 Sec",
            "1.5 Sec",
            "2 Sec",
            "3 Sec"});
            this.dwell.Location = new System.Drawing.Point(314, 15);
            this.dwell.Name = "dwell";
            this.dwell.Size = new System.Drawing.Size(72, 21);
            this.dwell.TabIndex = 3;
            this.dwell.SelectedIndexChanged += new System.EventHandler(this.dwell_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(101, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 22);
            this.label9.TabIndex = 2;
            this.label9.Text = "Radius:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clicking
            // 
            this.clicking.AutoSize = true;
            this.clicking.Location = new System.Drawing.Point(11, 17);
            this.clicking.Name = "clicking";
            this.clicking.Size = new System.Drawing.Size(63, 17);
            this.clicking.TabIndex = 0;
            this.clicking.Text = "Clicking";
            this.clicking.UseVisualStyleBackColor = true;
            this.clicking.CheckedChanged += new System.EventHandler(this.clicking_CheckedChanged);
            // 
            // StandardClickControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "StandardClickControlPanel";
            this.Size = new System.Drawing.Size(595, 54);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox click_sound;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox dwell;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox clickRadius;
        private System.Windows.Forms.CheckBox clicking;
        private System.Windows.Forms.ImageList imageList1;
    }
}
