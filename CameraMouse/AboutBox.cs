/*                         Camera Mouse Suite
 *  Copyright (C) 2014, Samual Epstein
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;



namespace CameraMouseSuite

{

	/// <summary>

	/// Summary description for AboutBox.

	/// </summary>

	public class AboutBox : System.Windows.Forms.Form

	{

		private System.Windows.Forms.Panel panel1;

		private System.Windows.Forms.Button OK_btn;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.LinkLabel link2;

		private System.Windows.Forms.LinkLabel link1;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public AboutBox()

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			label1.Text = "This material is based upon work supported by the" + 

				" National Science Foundation under the grants IIS-0093667, " +

			"EIA 0202067, IIS-0308213, and IIS-0329009.  Any opinions, findings, " +

				"and conclusions or recommendations expressed in this material are "+

				"those of the authors and do not necessarily reflect the views of " + 

			"the National Science Foundation.";



			link2.Text = "Camera Mouse Suite is the result of research and development"+

				" by Prof. James Gips of Boston College and Prof. Margrit Betke of Boston "+

				"University and their students.  Software engineering by Mekinesis, Inc.";





			link2.Links.Add(69, 10, "www.cs.bc.edu/~gips");

			link2.Links.Add(108, 13, "www.cs.bu.edu/~betke");

			link2.Links.Add(188, 15, "www.mekinesis.com");

			link2.LinkClicked +=new LinkLabelLinkClickedEventHandler(link2_LinkClicked);





			link1.Text = "For more information visit www.cameramouse.org";

			link1.Links.Add(27, 22, "www.cameramouse.org");

			link1.LinkClicked +=new LinkLabelLinkClickedEventHandler(link1_LinkClicked);



		

		

		}



		/// <summary>

		/// Clean up any resources being used.

		/// </summary>

		protected override void Dispose( bool disposing )

		{

			if( disposing )

			{

				if(components != null)

				{

					components.Dispose();

				}

			}

			base.Dispose( disposing );

		}



		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()

		{

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));

            this.panel1 = new System.Windows.Forms.Panel();

            this.link1 = new System.Windows.Forms.LinkLabel();

            this.link2 = new System.Windows.Forms.LinkLabel();

            this.label1 = new System.Windows.Forms.Label();

            this.OK_btn = new System.Windows.Forms.Button();

            this.panel1.SuspendLayout();

            this.SuspendLayout();

            // 

            // panel1

            // 

            this.panel1.Controls.Add(this.link1);

            this.panel1.Controls.Add(this.link2);

            this.panel1.Controls.Add(this.label1);

            this.panel1.Controls.Add(this.OK_btn);

            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;

            this.panel1.Location = new System.Drawing.Point(0, 0);

            this.panel1.Name = "panel1";

            this.panel1.Size = new System.Drawing.Size(328, 374);

            this.panel1.TabIndex = 0;

            // 

            // link1

            // 

            this.link1.Location = new System.Drawing.Point(22, 281);

            this.link1.Name = "link1";

            this.link1.Size = new System.Drawing.Size(287, 28);

            this.link1.TabIndex = 3;

            // 

            // link2

            // 

            this.link2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.link2.Location = new System.Drawing.Point(21, 24);

            this.link2.Name = "link2";

            this.link2.Size = new System.Drawing.Size(288, 125);

            this.link2.TabIndex = 2;

            // 

            // label1

            // 

            this.label1.Location = new System.Drawing.Point(23, 166);

            this.label1.Name = "label1";

            this.label1.Size = new System.Drawing.Size(288, 101);

            this.label1.TabIndex = 1;

            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 

            // OK_btn

            // 

            this.OK_btn.Location = new System.Drawing.Point(137, 331);

            this.OK_btn.Name = "OK_btn";

            this.OK_btn.Size = new System.Drawing.Size(56, 32);

            this.OK_btn.TabIndex = 0;

            this.OK_btn.Text = "OK";

            this.OK_btn.Click += new System.EventHandler(this.OK_btn_Click);

            // 

            // AboutBox

            // 

            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

            this.ClientSize = new System.Drawing.Size(328, 374);

            this.Controls.Add(this.panel1);

            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            this.Name = "AboutBox";

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text = "About Camera Mouse 2009";

            this.panel1.ResumeLayout(false);

            this.ResumeLayout(false);



		}

		#endregion



		private void OK_btn_Click(object sender, System.EventArgs e)

		{

			Close();

		}



		private void link1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)

		{

			string target = e.Link.LinkData as string;



			// If the value looks like a URL, navigate to it.

			// Otherwise, display it in a message box.

			if(null != target && target.StartsWith("www"))

			{

				System.Diagnostics.Process.Start(target);

			}

		}



		private void link2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)

		{

			string target = (string)e.Link.LinkData;



			// If the value looks like a URL, navigate to it.

			// Otherwise, display it in a message box.

			if(null != target && target.StartsWith("www"))

			{

				System.Diagnostics.Process.Start(target);

			}

		}

	}

}

