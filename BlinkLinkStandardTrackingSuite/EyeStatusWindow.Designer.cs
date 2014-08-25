namespace BlinkLinkStandardTrackingSuite
{
    partial class EyeStatusWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mousePictureBox = new System.Windows.Forms.PictureBox();
            this.rightEyeStatusPictureBox = new System.Windows.Forms.PictureBox();
            this.leftEyeStatusPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mousePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightEyeStatusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftEyeStatusPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(100, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Mouse";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Right Eye Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Left Eye Status";
            // 
            // mousePictureBox
            // 
            this.mousePictureBox.Location = new System.Drawing.Point(95, 6);
            this.mousePictureBox.Name = "mousePictureBox";
            this.mousePictureBox.Size = new System.Drawing.Size(50, 50);
            this.mousePictureBox.TabIndex = 29;
            this.mousePictureBox.TabStop = false;
            // 
            // rightEyeStatusPictureBox
            // 
            this.rightEyeStatusPictureBox.Location = new System.Drawing.Point(178, 6);
            this.rightEyeStatusPictureBox.Name = "rightEyeStatusPictureBox";
            this.rightEyeStatusPictureBox.Size = new System.Drawing.Size(50, 50);
            this.rightEyeStatusPictureBox.TabIndex = 28;
            this.rightEyeStatusPictureBox.TabStop = false;
            // 
            // leftEyeStatusPictureBox
            // 
            this.leftEyeStatusPictureBox.Location = new System.Drawing.Point(19, 6);
            this.leftEyeStatusPictureBox.Name = "leftEyeStatusPictureBox";
            this.leftEyeStatusPictureBox.Size = new System.Drawing.Size(50, 50);
            this.leftEyeStatusPictureBox.TabIndex = 24;
            this.leftEyeStatusPictureBox.TabStop = false;
            // 
            // EyeStatusWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Olive;
            this.ClientSize = new System.Drawing.Size(254, 76);
            this.ControlBox = false;
            this.Controls.Add(this.mousePictureBox);
            this.Controls.Add(this.rightEyeStatusPictureBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.leftEyeStatusPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EyeStatusWindow";
            this.Opacity = 0.8;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "EyeStatusWindow";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Olive;
            this.Load += new System.EventHandler(this.EyeStatusWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mousePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightEyeStatusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftEyeStatusPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mousePictureBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox leftEyeStatusPictureBox;
        private System.Windows.Forms.PictureBox rightEyeStatusPictureBox;
    }
}