namespace CameraMouseSuite
{
    partial class LoggingControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonFull = new System.Windows.Forms.RadioButton();
            this.radioButtonRegular = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonSparse = new System.Windows.Forms.RadioButton();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxProxyPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxProxyUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxProxyUrl = new System.Windows.Forms.TextBox();
            this.textBoxServerUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelLogStatus = new System.Windows.Forms.Label();
            this.labelLogStatusDetails = new System.Windows.Forms.Label();
            this.buttonStatusUpdate = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxEnablPCMessages = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonEditPersonalInfo = new System.Windows.Forms.Button();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.buttonViewConsent = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonFull);
            this.groupBox1.Controls.Add(this.radioButtonRegular);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.radioButtonSparse);
            this.groupBox1.Controls.Add(this.radioButtonNone);
            this.groupBox1.Location = new System.Drawing.Point(14, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 202);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Message Sending ";
            // 
            // radioButtonFull
            // 
            this.radioButtonFull.AutoSize = true;
            this.radioButtonFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonFull.Location = new System.Drawing.Point(10, 200);
            this.radioButtonFull.Name = "radioButtonFull";
            this.radioButtonFull.Size = new System.Drawing.Size(47, 20);
            this.radioButtonFull.TabIndex = 3;
            this.radioButtonFull.TabStop = true;
            this.radioButtonFull.Text = "Full";
            this.radioButtonFull.UseVisualStyleBackColor = true;
            this.radioButtonFull.Visible = false;
            this.radioButtonFull.CheckedChanged += new System.EventHandler(this.radioButtonFull_CheckedChanged);
            // 
            // radioButtonRegular
            // 
            this.radioButtonRegular.AutoSize = true;
            this.radioButtonRegular.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonRegular.Location = new System.Drawing.Point(10, 131);
            this.radioButtonRegular.Name = "radioButtonRegular";
            this.radioButtonRegular.Size = new System.Drawing.Size(74, 20);
            this.radioButtonRegular.TabIndex = 2;
            this.radioButtonRegular.TabStop = true;
            this.radioButtonRegular.Text = "Regular";
            this.radioButtonRegular.UseVisualStyleBackColor = true;
            this.radioButtonRegular.CheckedChanged += new System.EventHandler(this.radioButtonRegular_CheckedChanged);
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Location = new System.Drawing.Point(25, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(279, 29);
            this.label8.TabIndex = 7;
            this.label8.Text = "Large messages will be sent. This setting is\r\nrecommended for users with fast net" +
                "work connections.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Visible = false;
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(25, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(279, 38);
            this.label7.TabIndex = 6;
            this.label7.Text = "Small messages will be sent frequently. This setting is \r\nrecommended for users w" +
                "ith regular network connections.\r\n\r\n";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(25, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(279, 62);
            this.label6.TabIndex = 5;
            this.label6.Text = "Messages will only be sent infrequently. This setting is \r\nrecommended for users " +
                "with slower network connections.\r\n\r\n";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(25, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(279, 29);
            this.label5.TabIndex = 4;
            this.label5.Text = "No Messages will be sent.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButtonSparse
            // 
            this.radioButtonSparse.AutoSize = true;
            this.radioButtonSparse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSparse.Location = new System.Drawing.Point(10, 64);
            this.radioButtonSparse.Name = "radioButtonSparse";
            this.radioButtonSparse.Size = new System.Drawing.Size(70, 20);
            this.radioButtonSparse.TabIndex = 1;
            this.radioButtonSparse.TabStop = true;
            this.radioButtonSparse.Text = "Sparse";
            this.radioButtonSparse.UseVisualStyleBackColor = true;
            this.radioButtonSparse.CheckedChanged += new System.EventHandler(this.radioButtonSparse_CheckedChanged);
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNone.Location = new System.Drawing.Point(10, 19);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(59, 20);
            this.radioButtonNone.TabIndex = 0;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "None";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            this.radioButtonNone.CheckedChanged += new System.EventHandler(this.radioButtonNone_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.textBoxServerUrl);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(340, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 181);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Network Information";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxProxyPassword);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textBoxProxyUsername);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.textBoxProxyUrl);
            this.groupBox4.Location = new System.Drawing.Point(14, 51);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(284, 114);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Proxy";
            // 
            // textBoxProxyPassword
            // 
            this.textBoxProxyPassword.Location = new System.Drawing.Point(81, 81);
            this.textBoxProxyPassword.Name = "textBoxProxyPassword";
            this.textBoxProxyPassword.Size = new System.Drawing.Size(104, 20);
            this.textBoxProxyPassword.TabIndex = 7;
            this.textBoxProxyPassword.TextChanged += new System.EventHandler(this.textBoxProxyPassword_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password";
            // 
            // textBoxProxyUsername
            // 
            this.textBoxProxyUsername.Location = new System.Drawing.Point(81, 56);
            this.textBoxProxyUsername.Name = "textBoxProxyUsername";
            this.textBoxProxyUsername.Size = new System.Drawing.Size(104, 20);
            this.textBoxProxyUsername.TabIndex = 5;
            this.textBoxProxyUsername.TextChanged += new System.EventHandler(this.textBoxProxyUsername_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Url";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // textBoxProxyUrl
            // 
            this.textBoxProxyUrl.Location = new System.Drawing.Point(37, 24);
            this.textBoxProxyUrl.Name = "textBoxProxyUrl";
            this.textBoxProxyUrl.Size = new System.Drawing.Size(231, 20);
            this.textBoxProxyUrl.TabIndex = 3;
            this.textBoxProxyUrl.TextChanged += new System.EventHandler(this.textBoxProxyUrl_TextChanged);
            // 
            // textBoxServerUrl
            // 
            this.textBoxServerUrl.Location = new System.Drawing.Point(85, 28);
            this.textBoxServerUrl.Name = "textBoxServerUrl";
            this.textBoxServerUrl.Size = new System.Drawing.Size(213, 20);
            this.textBoxServerUrl.TabIndex = 1;
            this.textBoxServerUrl.TextChanged += new System.EventHandler(this.textBoxServerUrl_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Url";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelLogStatus);
            this.groupBox3.Controls.Add(this.labelLogStatusDetails);
            this.groupBox3.Location = new System.Drawing.Point(340, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(313, 99);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "System Status";
            // 
            // labelLogStatus
            // 
            this.labelLogStatus.AutoSize = true;
            this.labelLogStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLogStatus.Location = new System.Drawing.Point(11, 22);
            this.labelLogStatus.Name = "labelLogStatus";
            this.labelLogStatus.Size = new System.Drawing.Size(71, 16);
            this.labelLogStatus.TabIndex = 3;
            this.labelLogStatus.Text = "Log Status";
            // 
            // labelLogStatusDetails
            // 
            this.labelLogStatusDetails.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelLogStatusDetails.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelLogStatusDetails.Location = new System.Drawing.Point(11, 47);
            this.labelLogStatusDetails.Name = "labelLogStatusDetails";
            this.labelLogStatusDetails.Padding = new System.Windows.Forms.Padding(2);
            this.labelLogStatusDetails.Size = new System.Drawing.Size(292, 41);
            this.labelLogStatusDetails.TabIndex = 8;
            this.labelLogStatusDetails.Text = "Status Details";
            // 
            // buttonStatusUpdate
            // 
            this.buttonStatusUpdate.Location = new System.Drawing.Point(354, 309);
            this.buttonStatusUpdate.Name = "buttonStatusUpdate";
            this.buttonStatusUpdate.Size = new System.Drawing.Size(122, 22);
            this.buttonStatusUpdate.TabIndex = 33;
            this.buttonStatusUpdate.Text = "Update/Save";
            this.buttonStatusUpdate.UseVisualStyleBackColor = true;
            this.buttonStatusUpdate.Click += new System.EventHandler(this.buttonStatusUpdate_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBoxEnablPCMessages);
            this.groupBox5.Location = new System.Drawing.Point(14, 292);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(312, 66);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Behavior Message Sending";
            this.groupBox5.Visible = false;
            // 
            // checkBoxEnablPCMessages
            // 
            this.checkBoxEnablPCMessages.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxEnablPCMessages.Location = new System.Drawing.Point(8, 17);
            this.checkBoxEnablPCMessages.Name = "checkBoxEnablPCMessages";
            this.checkBoxEnablPCMessages.Size = new System.Drawing.Size(298, 43);
            this.checkBoxEnablPCMessages.TabIndex = 0;
            this.checkBoxEnablPCMessages.Text = "Enable the Camera Mouse to send the names of other applications being used. No in" +
                "formation about the current state of the applications will be sent.\r\n ";
            this.checkBoxEnablPCMessages.UseVisualStyleBackColor = true;
            this.checkBoxEnablPCMessages.CheckedChanged += new System.EventHandler(this.checkBoxEnablPCMessages_CheckedChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(606, 309);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(47, 49);
            this.buttonOk.TabIndex = 31;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonEditPersonalInfo
            // 
            this.buttonEditPersonalInfo.Location = new System.Drawing.Point(354, 336);
            this.buttonEditPersonalInfo.Name = "buttonEditPersonalInfo";
            this.buttonEditPersonalInfo.Size = new System.Drawing.Size(122, 22);
            this.buttonEditPersonalInfo.TabIndex = 32;
            this.buttonEditPersonalInfo.Text = "Personal Info";
            this.buttonEditPersonalInfo.UseVisualStyleBackColor = true;
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(485, 336);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(115, 22);
            this.buttonSendMessage.TabIndex = 34;
            this.buttonSendMessage.Text = "Send Us a Message!";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click);
            // 
            // buttonViewConsent
            // 
            this.buttonViewConsent.Location = new System.Drawing.Point(485, 309);
            this.buttonViewConsent.Name = "buttonViewConsent";
            this.buttonViewConsent.Size = new System.Drawing.Size(115, 22);
            this.buttonViewConsent.TabIndex = 35;
            this.buttonViewConsent.Text = "View Info Form";
            this.buttonViewConsent.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(14, 224);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(312, 134);
            this.textBox1.TabIndex = 36;
            this.textBox1.Text = "\r\n  To start, please:\r\n\r\n  1. Select either \"Sparse\" or \r\n      \"Regular\" message" +
                " sending\r\n\r\n  2. Press \"Update/Save\"";
            // 
            // LoggingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonViewConsent);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.buttonEditPersonalInfo);
            this.Controls.Add(this.buttonStatusUpdate);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LoggingControl";
            this.Size = new System.Drawing.Size(672, 374);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxServerUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxProxyUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxProxyUrl;
        private System.Windows.Forms.RadioButton radioButtonSparse;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.TextBox textBoxProxyPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonRegular;
        private System.Windows.Forms.RadioButton radioButtonFull;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBoxEnablPCMessages;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonEditPersonalInfo;
        private System.Windows.Forms.Label labelLogStatus;
        private System.Windows.Forms.Label labelLogStatusDetails;
        private System.Windows.Forms.Button buttonStatusUpdate;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.Button buttonViewConsent;
        private System.Windows.Forms.TextBox textBox1;
    }
}
