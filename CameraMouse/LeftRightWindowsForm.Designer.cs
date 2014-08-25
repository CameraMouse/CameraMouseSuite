namespace CameraMouseSuite
{
    partial class LeftRightWindowsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.controlPanel = new System.Windows.Forms.Panel();
            this.userMessage = new System.Windows.Forms.Label();
            this.settingButton = new System.Windows.Forms.Button();
            this.controlLabel = new System.Windows.Forms.Label();
            this.quitButton = new System.Windows.Forms.Button();
            this.rightVideoDisplay = new System.Windows.Forms.PictureBox();
            this.leftVideoDisplay = new System.Windows.Forms.PictureBox();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightVideoDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftVideoDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.Controls.Add(this.userMessage);
            this.controlPanel.Controls.Add(this.settingButton);
            this.controlPanel.Controls.Add(this.controlLabel);
            this.controlPanel.Controls.Add(this.quitButton);
            this.controlPanel.Location = new System.Drawing.Point(157, 258);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(376, 166);
            this.controlPanel.TabIndex = 3;
            // 
            // userMessage
            // 
            this.userMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.userMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userMessage.ForeColor = System.Drawing.Color.LimeGreen;
            this.userMessage.Location = new System.Drawing.Point(8, 85);
            this.userMessage.Name = "userMessage";
            this.userMessage.Size = new System.Drawing.Size(360, 48);
            this.userMessage.TabIndex = 3;
            this.userMessage.Text = "Please click on feature on face to track.";
            this.userMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // settingButton
            // 
            this.settingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.settingButton.Location = new System.Drawing.Point(122, 53);
            this.settingButton.Name = "settingButton";
            this.settingButton.Size = new System.Drawing.Size(136, 24);
            this.settingButton.TabIndex = 2;
            this.settingButton.Text = "Settings";
            this.settingButton.UseVisualStyleBackColor = true;
            this.settingButton.Click += new System.EventHandler(this.settingButton_Click);
            // 
            // controlLabel
            // 
            this.controlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlLabel.Location = new System.Drawing.Point(26, 18);
            this.controlLabel.Name = "controlLabel";
            this.controlLabel.Size = new System.Drawing.Size(328, 32);
            this.controlLabel.TabIndex = 1;
            this.controlLabel.Text = "Control: Mouse";
            this.controlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // quitButton
            // 
            this.quitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.quitButton.Location = new System.Drawing.Point(135, 143);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(108, 23);
            this.quitButton.TabIndex = 0;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // rightVideoDisplay
            // 
            this.rightVideoDisplay.Location = new System.Drawing.Point(355, 12);
            this.rightVideoDisplay.Name = "rightVideoDisplay";
            this.rightVideoDisplay.Size = new System.Drawing.Size(320, 240);
            this.rightVideoDisplay.TabIndex = 2;
            this.rightVideoDisplay.TabStop = false;
            this.rightVideoDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.rightVideoDisplay_Paint);
            this.rightVideoDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rightVideoDisplay_MouseUp);
            // 
            // leftVideoDisplay
            // 
            this.leftVideoDisplay.Location = new System.Drawing.Point(12, 12);
            this.leftVideoDisplay.Name = "leftVideoDisplay";
            this.leftVideoDisplay.Size = new System.Drawing.Size(320, 240);
            this.leftVideoDisplay.TabIndex = 1;
            this.leftVideoDisplay.TabStop = false;
            this.leftVideoDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.leftVideoDisplay_Paint);
            this.leftVideoDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.leftVideoDisplay_MouseUp);
            // 
            // LeftRightWindowsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 436);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.rightVideoDisplay);
            this.Controls.Add(this.leftVideoDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "LeftRightWindowsForm";
            this.Text = "Stereo Camera Mouse Suite";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LeftRightWindowsForm_FormClosed);
            this.controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightVideoDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftVideoDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox leftVideoDisplay;
        private System.Windows.Forms.PictureBox rightVideoDisplay;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Label userMessage;
        private System.Windows.Forms.Button settingButton;
        private System.Windows.Forms.Label controlLabel;
        private System.Windows.Forms.Button quitButton;
    }
}