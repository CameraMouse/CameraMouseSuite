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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public partial class LoggingControl : UserControl
    {
        private CMSViewAdapter viewAdapter = null;
        public CMSViewAdapter ViewAdapter
        {
            get
            {
                return viewAdapter;
            }
            set
            {
                viewAdapter = value;
            }
        }

        public LoggingControl()
        {
            InitializeComponent();
        }

        private CMSLogConfig logConfig = null;
        public CMSLogConfig LogConfig
        {
            get
            {
                return logConfig;
            }
            set
            {
                logConfig = value;
            }
        }

        private CMSIdentificationConfig idConfig = null;
        public CMSIdentificationConfig IdConfig
        {
            get
            {
                return idConfig;
            }
            set
            {
                idConfig = value;
            }
        }

        private bool isLoading = false;

        public void LoadControls()
        {
            isLoading = true;
            logConfig = viewAdapter.LogConfig;
            if (logConfig.LoggingBehavior.Equals(CMSLogBehavior.NoLogging))
            {
                this.radioButtonNone.Checked = true;
            }
            else if (logConfig.LoggingBehavior.Equals(CMSLogBehavior.OnlyInfrequentLogging))
            {
                this.radioButtonSparse.Checked = true;
            }
            else if (logConfig.LoggingBehavior.Equals(CMSLogBehavior.AllSmallMessages))
            {
                this.radioButtonRegular.Checked = true;
            }
            else if (logConfig.LoggingBehavior.Equals(CMSLogBehavior.FullLogging))
            {
                this.radioButtonFull.Checked = true;
            }

            this.textBoxServerUrl.Text = logConfig.LogServer;
            this.textBoxProxyUrl.Text = logConfig.ProxyServer;
            this.textBoxProxyUsername.Text = logConfig.ProxyUsername;
            this.textBoxProxyPassword.Text = logConfig.ProxyPassword;
            this.checkBoxEnablPCMessages.Checked = logConfig.EnablePCMessages;

            if(idConfig != null)
                buttonViewConsent.Visible = !idConfig.NotStudy;
            isLoading = false;
        }

        private void radioButtonNone_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (radioButtonNone.Checked)
                logConfig.LoggingBehavior = CMSLogBehavior.NoLogging;
        }
        private void radioButtonSparse_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (radioButtonSparse.Checked)
                logConfig.LoggingBehavior = CMSLogBehavior.OnlyInfrequentLogging;
        }
        private void radioButtonRegular_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (radioButtonRegular.Checked)
                logConfig.LoggingBehavior = CMSLogBehavior.AllSmallMessages;
        }
        private void radioButtonFull_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (this.radioButtonFull.Checked)
                logConfig.LoggingBehavior = CMSLogBehavior.FullLogging;
        }
        private void checkBoxEnablPCMessages_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            logConfig.EnablePCMessages = checkBoxEnablPCMessages.Checked;
        }
        private void textBoxServerUrl_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            logConfig.LogServer = textBoxServerUrl.Text;            

        }
        private void textBoxProxyUrl_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            logConfig.ProxyServer = textBoxProxyUrl.Text;
        }
        private void textBoxProxyUsername_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            logConfig.ProxyUsername = textBoxProxyUsername.Text;
        }
        private void textBoxProxyPassword_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            logConfig.ProxyPassword = textBoxProxyPassword.Text;
        }
        private void buttonStatusUpdate_Click(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            viewAdapter.LogConfig = logConfig;
            viewAdapter.SaveLogConfig();
            CMSLogger.UpdateLogger();
            UpdateStatus();

            if (CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogBehaviorEvent"))
            {
                CMSLogBehaviorEvent logEvent = new CMSLogBehaviorEvent(logConfig);
                CMSLogger.SendLogEvent(logEvent);
            }
        }

        public void Init()
        {
            CMSLogger.AddLoggerStatusChangeListener(LoggingStatusUpdate);
        }

        private void LoggingStatusUpdate()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LoggerStatusChange(LoggingStatusUpdate));
            }
            else
            {
                UpdateStatus();
            }
        }

        public void UpdateStatus()
        {
            bool isConnected = CMSLogger.IsLoggerConnected();
            bool isRunning = CMSLogger.IsLoggerRunning();
            Exception e = CMSLogger.GetNetworkException();

            if (e != null)
            {
                labelLogStatusDetails.Text = e.Message;
            }
            else
            {
                labelLogStatusDetails.Text = "";
            }

            bool neverConnect = false;
            IDictionary vars = Environment.GetEnvironmentVariables();
            if(vars.Contains("NeverConnect"))
                neverConnect = Boolean.Parse(vars["NeverConnect"] as string);

            if (!isRunning)
            {
                labelLogStatus.Text = "System is not Running";                
            }
            else if (neverConnect)
            {
                labelLogStatus.Text = "System is Running and can never Connect";
            }
            else if (!isConnected)
            {
                labelLogStatus.Text = "System is Running, but not Connected";
            }
            else
            {
                labelLogStatus.Text = "System is Running and Connected";
            }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            if (isLoading)
                return;
        }

        public event EventHandler OkClick
        {
            add
            {
                this.buttonOk.Click += value;
            }
            remove
            {
                this.buttonOk.Click -= value;
            }
        }

        public event EventHandler PersonalInfoClick
        {
            add
            {
                this.buttonEditPersonalInfo.Click += value;
            }
            remove
            {
                this.buttonEditPersonalInfo.Click -= value;
            }
        }

        public event EventHandler ViewConsentClick
        {
            add
            {
                buttonViewConsent.Click += value;
            }
            remove
            {
                buttonViewConsent.Click -= value;
            }
        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            if (!CMSLogger.CanCreateLogEvent(false, false, false, "CMSLogMessageEvent"))
                return;
   
            SendMessageForm smf = new SendMessageForm();
            if (!smf.ShowDialog().Equals(DialogResult.OK))
                return;

            string text = smf.MessageText;
            if (text == null)
                return;
            text = text.Trim();

            CMSLogMessageEvent logEvent = new CMSLogMessageEvent();
            logEvent.Text = text;
            CMSLogger.SendLogEvent(logEvent);
        }

    }
}
