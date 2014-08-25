namespace CameraMouseSuite
{
    partial class AdvTrackerSelectionControl
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
            this.groupBoxTrackerSelection = new System.Windows.Forms.GroupBox();
            this.textBoxAdvDescription = new System.Windows.Forms.TextBox();
            this.listBoxAdvTrackers = new System.Windows.Forms.ListBox();
            this.labelAdvDescription = new System.Windows.Forms.Label();
            this.labelAdvAvailableTrackers = new System.Windows.Forms.Label();
            this.buttonAdvSelectNewTracker = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBoxTrackerSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTrackerSelection
            // 
            this.groupBoxTrackerSelection.Controls.Add(this.textBoxAdvDescription);
            this.groupBoxTrackerSelection.Controls.Add(this.listBoxAdvTrackers);
            this.groupBoxTrackerSelection.Controls.Add(this.labelAdvDescription);
            this.groupBoxTrackerSelection.Controls.Add(this.labelAdvAvailableTrackers);
            this.groupBoxTrackerSelection.Controls.Add(this.buttonAdvSelectNewTracker);
            this.groupBoxTrackerSelection.Controls.Add(this.labelTitle);
            this.groupBoxTrackerSelection.Location = new System.Drawing.Point(8, 3);
            this.groupBoxTrackerSelection.Name = "groupBoxTrackerSelection";
            this.groupBoxTrackerSelection.Size = new System.Drawing.Size(650, 278);
            this.groupBoxTrackerSelection.TabIndex = 1;
            this.groupBoxTrackerSelection.TabStop = false;
            // 
            // textBoxAdvDescription
            // 
            this.textBoxAdvDescription.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxAdvDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAdvDescription.Location = new System.Drawing.Point(373, 70);
            this.textBoxAdvDescription.Multiline = true;
            this.textBoxAdvDescription.Name = "textBoxAdvDescription";
            this.textBoxAdvDescription.Size = new System.Drawing.Size(204, 186);
            this.textBoxAdvDescription.TabIndex = 5;
            // 
            // listBoxAdvTrackers
            // 
            this.listBoxAdvTrackers.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxAdvTrackers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxAdvTrackers.FormattingEnabled = true;
            this.listBoxAdvTrackers.Location = new System.Drawing.Point(88, 70);
            this.listBoxAdvTrackers.Name = "listBoxAdvTrackers";
            this.listBoxAdvTrackers.Size = new System.Drawing.Size(204, 184);
            this.listBoxAdvTrackers.TabIndex = 2;
            this.listBoxAdvTrackers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxAdvTrackers_MouseDoubleClick);
            this.listBoxAdvTrackers.SelectedIndexChanged += new System.EventHandler(this.listBoxTrackers_SelectedIndexChanged);
            // 
            // labelAdvDescription
            // 
            this.labelAdvDescription.AutoSize = true;
            this.labelAdvDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdvDescription.Location = new System.Drawing.Point(438, 50);
            this.labelAdvDescription.Name = "labelAdvDescription";
            this.labelAdvDescription.Size = new System.Drawing.Size(76, 16);
            this.labelAdvDescription.TabIndex = 4;
            this.labelAdvDescription.Text = "Description";
            // 
            // labelAdvAvailableTrackers
            // 
            this.labelAdvAvailableTrackers.AutoSize = true;
            this.labelAdvAvailableTrackers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdvAvailableTrackers.Location = new System.Drawing.Point(124, 50);
            this.labelAdvAvailableTrackers.Name = "labelAdvAvailableTrackers";
            this.labelAdvAvailableTrackers.Size = new System.Drawing.Size(122, 16);
            this.labelAdvAvailableTrackers.TabIndex = 3;
            this.labelAdvAvailableTrackers.Text = "Available Trackers";
            // 
            // buttonAdvSelectNewTracker
            // 
            this.buttonAdvSelectNewTracker.Location = new System.Drawing.Point(6, 16);
            this.buttonAdvSelectNewTracker.Name = "buttonAdvSelectNewTracker";
            this.buttonAdvSelectNewTracker.Size = new System.Drawing.Size(139, 23);
            this.buttonAdvSelectNewTracker.TabIndex = 1;
            this.buttonAdvSelectNewTracker.Text = "Select  Tracker";
            this.buttonAdvSelectNewTracker.UseVisualStyleBackColor = true;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(245, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(180, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Tracker Selection";
            // 
            // AdvTrackerSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTrackerSelection);
            this.Name = "AdvTrackerSelectionControl";
            this.Size = new System.Drawing.Size(665, 289);
            this.groupBoxTrackerSelection.ResumeLayout(false);
            this.groupBoxTrackerSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTrackerSelection;
        private System.Windows.Forms.TextBox textBoxAdvDescription;
        private System.Windows.Forms.Label labelAdvDescription;
        private System.Windows.Forms.Label labelAdvAvailableTrackers;
        private System.Windows.Forms.ListBox listBoxAdvTrackers;
        private System.Windows.Forms.Button buttonAdvSelectNewTracker;
        private System.Windows.Forms.Label labelTitle;
    }
}
