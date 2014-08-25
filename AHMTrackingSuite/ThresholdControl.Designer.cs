namespace AHMTrackingSuite
{
    partial class ThresholdControl
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
            this.progressBarError = new System.Windows.Forms.ProgressBar();
            this.trackBarEThreshold = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarEThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarError
            // 
            this.progressBarError.Location = new System.Drawing.Point(12, 26);
            this.progressBarError.Name = "progressBarError";
            this.progressBarError.Size = new System.Drawing.Size(177, 24);
            this.progressBarError.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarError.TabIndex = 19;
            // 
            // trackBarEThreshold
            // 
            this.trackBarEThreshold.Location = new System.Drawing.Point(4, 5);
            this.trackBarEThreshold.Maximum = 100;
            this.trackBarEThreshold.Name = "trackBarEThreshold";
            this.trackBarEThreshold.Size = new System.Drawing.Size(196, 45);
            this.trackBarEThreshold.TabIndex = 18;
            this.trackBarEThreshold.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarEThreshold.Value = 100;
            this.trackBarEThreshold.Scroll += new System.EventHandler(this.trackBarEThreshold_Scroll);
            // 
            // ThresholdControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBarError);
            this.Controls.Add(this.trackBarEThreshold);
            this.Name = "ThresholdControl";
            this.Size = new System.Drawing.Size(200, 63);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarEThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarError;
        private System.Windows.Forms.TrackBar trackBarEThreshold;
    }
}
