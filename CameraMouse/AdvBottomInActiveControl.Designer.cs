namespace CameraMouseSuite
{
    partial class AdvBottomInActiveControl
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
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.Ok = new System.Windows.Forms.Button();
            this.help = new System.Windows.Forms.Button();
            this.about = new System.Windows.Forms.Button();
            this.camera_advanced = new System.Windows.Forms.Button();
            this.video_selector_btn = new System.Windows.Forms.Button();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.Ok);
            this.groupBox11.Controls.Add(this.help);
            this.groupBox11.Controls.Add(this.about);
            this.groupBox11.Controls.Add(this.camera_advanced);
            this.groupBox11.Controls.Add(this.video_selector_btn);
            this.groupBox11.Location = new System.Drawing.Point(8, 1);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(650, 77);
            this.groupBox11.TabIndex = 21;
            this.groupBox11.TabStop = false;
            // 
            // Ok
            // 
            this.Ok.Enabled = false;
            this.Ok.Location = new System.Drawing.Point(582, 46);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(48, 22);
            this.Ok.TabIndex = 19;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            // 
            // help
            // 
            this.help.Location = new System.Drawing.Point(438, 46);
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(64, 22);
            this.help.TabIndex = 17;
            this.help.Text = "Help";
            this.help.UseVisualStyleBackColor = true;
            // 
            // about
            // 
            this.about.Location = new System.Drawing.Point(511, 46);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(64, 22);
            this.about.TabIndex = 18;
            this.about.Text = "About";
            this.about.UseVisualStyleBackColor = true;
            // 
            // camera_advanced
            // 
            this.camera_advanced.Location = new System.Drawing.Point(334, 46);
            this.camera_advanced.Name = "camera_advanced";
            this.camera_advanced.Size = new System.Drawing.Size(96, 22);
            this.camera_advanced.TabIndex = 16;
            this.camera_advanced.Text = "Camera Settings";
            this.camera_advanced.UseVisualStyleBackColor = true;
            // 
            // video_selector_btn
            // 
            this.video_selector_btn.Location = new System.Drawing.Point(398, 16);
            this.video_selector_btn.Name = "video_selector_btn";
            this.video_selector_btn.Size = new System.Drawing.Size(171, 22);
            this.video_selector_btn.TabIndex = 15;
            this.video_selector_btn.Text = "Change Video Source";
            this.video_selector_btn.UseVisualStyleBackColor = true;
            // 
            // AdvBottomInActiveControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBox11);
            this.Name = "AdvBottomInActiveControl";
            this.Size = new System.Drawing.Size(665, 85);
            this.groupBox11.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button about;
        private System.Windows.Forms.Button camera_advanced;
        private System.Windows.Forms.Button video_selector_btn;
    }
}
