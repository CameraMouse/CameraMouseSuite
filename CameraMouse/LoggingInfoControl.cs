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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CameraMouseSuite
{
    public partial class LoggingInfoControl : UserControl
    {   
        private CMSViewAdapter viewAdapter = null;
        
        public LoggingInfoControl()
        {
            InitializeComponent();
        }

        private CMSLogConfig logConfig = null;
        
        private CMSIdentificationConfig idConfig = null;
        
        enum LoggingInfoCurPage
        {
            ResearchIntroPage,
            PersonalIdPage,
            AdultConsentPage,
            ChildAssentPage,
            ParentConsentPage,
            LogginPage
        }

        enum LoggingInfoState
        {
            None,
            PersonalInfoEntered,
            HasConsent
        }

        private LoggingInfoState CurState
        {
            get
            {
                if (idConfig.HasConsent)
                    return LoggingInfoState.HasConsent;
                if (idConfig.PersonalInfoEntered)
                    return LoggingInfoState.PersonalInfoEntered;
                return LoggingInfoState.None;
            }
        }

        private AdultConsentControl adultConsentControl = null;
        private IdentificationControl idControl = null;
        private LoggingControl loggingControl = null;
        private ChildAssentControl childConsentControl = null;
        private ParentConsentControl parentConsentControl = null;
        private ParticipationRequestControl researchControl = null;

        private event EventHandler closeEvent;
        public event EventHandler CloseEvent
        {
            add
            {
                closeEvent += value;
            }
            remove
            {
                closeEvent -= value;
            }
        }
        
        public void Init(CMSViewAdapter viewAdapter)
        {
            this.viewAdapter = viewAdapter;
            this.idConfig = viewAdapter.IdentificationConfig;
            this.logConfig = viewAdapter.LogConfig;

            adultConsentControl = new AdultConsentControl();
            adultConsentControl.IdConfig = idConfig;
            adultConsentControl.AgreeClick += new EventHandler(adultConsentControl_AgreeClick);
            adultConsentControl.GoBackClick += new EventHandler(adultConsentControl_GoBackClick);
            
            idControl = new IdentificationControl();
            idControl.IdConfig = idConfig;
            idControl.OkClick += new EventHandler(idControl_OkClick);
            idControl.CancelClick += new EventHandler(idControl_CancelClick);

            loggingControl = new LoggingControl();
            loggingControl.Init();
            loggingControl.OkClick += new EventHandler(loggingControl_OkClick);
            loggingControl.PersonalInfoClick += new EventHandler(loggingControl_PersonalInfoClick);
            loggingControl.ViewConsentClick += new EventHandler(loggingControl_ViewConsentClick);
            loggingControl.ViewAdapter = viewAdapter;
            loggingControl.LoadControls();
            
            childConsentControl = new ChildAssentControl();
            childConsentControl.AgreeClick += new EventHandler(childConsentControl_AgreeClick);
            childConsentControl.GoBackClick += new EventHandler(childConsentControl_GoBackClick);
            childConsentControl.IdConfig = idConfig;

            parentConsentControl = new ParentConsentControl();
            parentConsentControl.AgreeClick += new EventHandler(parentConsentControl_AgreeClick);
            parentConsentControl.GoBackClick +=new EventHandler(parentConsentControl_GoBackClick);
            parentConsentControl.IdConfig = idConfig;

            researchControl = new ParticipationRequestControl();
            researchControl.AgreeClick += new EventHandler(researchControl_AgreeClick);

            LoggingInfoState state = CurState;
            if (state.Equals(LoggingInfoState.None))
                SetPage(LoggingInfoCurPage.ResearchIntroPage);
            else if (state.Equals(LoggingInfoState.PersonalInfoEntered))
                SetPage(LoggingInfoCurPage.PersonalIdPage);
            else if (state.Equals(LoggingInfoState.HasConsent))
                SetPage(LoggingInfoCurPage.LogginPage);
            
        }
        public void OnView()
        {
            loggingControl.LoadControls();
            loggingControl.UpdateStatus();
        }
        private void SetPage(LoggingInfoCurPage newPage)
        {
            Controls.Clear();
            if (newPage.Equals(LoggingInfoCurPage.ResearchIntroPage))
            {
                Controls.Add(researchControl);
            }
            else if (newPage.Equals(LoggingInfoCurPage.PersonalIdPage))
            {
                idControl.Init(!CurState.Equals(LoggingInfoState.HasConsent));
                Controls.Add(idControl);
            }
            else if (newPage.Equals(LoggingInfoCurPage.LogginPage))
            {
                Controls.Add(loggingControl);
            }
            else if (newPage.Equals(LoggingInfoCurPage.ChildAssentPage))
            {
                childConsentControl.Init();
                Controls.Add(childConsentControl);
            }
            else if (newPage.Equals(LoggingInfoCurPage.AdultConsentPage))
            {
                adultConsentControl.Init();
                Controls.Add(adultConsentControl);
            }
            else if (newPage.Equals(LoggingInfoCurPage.ParentConsentPage))
            {
                parentConsentControl.Init();
                Controls.Add(parentConsentControl);
            }
            Invalidate();
        }

        void loggingControl_ViewConsentClick(object sender, EventArgs e)
        {
            if (idConfig.AgeGroup == AgeGroup.MinorSixYounger)
            {
                SetPage(LoggingInfoCurPage.ParentConsentPage);
            }
            else if (idConfig.AgeGroup == AgeGroup.MinorSevenOlder)
            {
                SetPage(LoggingInfoCurPage.ChildAssentPage);
            }
            else if (idConfig.AgeGroup == AgeGroup.Adult)
            {
                SetPage(LoggingInfoCurPage.AdultConsentPage);
            }
        }
        void loggingControl_PersonalInfoClick(object sender, EventArgs e)
        {
            SetPage(LoggingInfoCurPage.PersonalIdPage);
        }
        void loggingControl_OkClick(object sender, EventArgs e)
        {
            closeEvent(this, e);
        }

        void idControl_CancelClick(object sender, EventArgs e)
        {
            if(CurState.Equals(LoggingInfoState.HasConsent))
                SetPage(LoggingInfoCurPage.LogginPage);
            else
                SetPage(LoggingInfoCurPage.ResearchIntroPage);            
        }
        void idControl_OkClick(object sender, EventArgs e)
        {
            if((idConfig.FirstName == null || idConfig.FirstName.Length == 0) || 
                (idConfig.LastName == null || idConfig.LastName.Length == 0))
            {
                MessageBox.Show("Please enter your first and last name");
                return;
            }

            viewAdapter.IdentificationConfig = idConfig;
            viewAdapter.SaveIdConfig();

            if (CurState.Equals(LoggingInfoState.HasConsent))
                SetPage(LoggingInfoCurPage.LogginPage);
            else if (idConfig.AgeGroup == AgeGroup.Adult)
                SetPage(LoggingInfoCurPage.AdultConsentPage);
            else if (idConfig.AgeGroup == AgeGroup.MinorSevenOlder)
                SetPage(LoggingInfoCurPage.ChildAssentPage);
            else if (idConfig.AgeGroup == AgeGroup.MinorSixYounger)
                SetPage(LoggingInfoCurPage.ParentConsentPage);
        }

        void researchControl_AgreeClick(object sender, EventArgs e)
        {
            SetPage(LoggingInfoCurPage.PersonalIdPage);
        }
    
        void childConsentControl_GoBackClick(object sender, EventArgs e)
        {
            SetPage(LoggingInfoCurPage.PersonalIdPage);
        }
        void childConsentControl_AgreeClick(object sender, EventArgs e)
        {
            /*
            if (!idConfig.HasConsent)
            {
                idConfig.ManufactureConsent();
                viewAdapter.IdentificationConfig = idConfig;
                viewAdapter.SaveIdConfig();
            }*/
            SetPage(LoggingInfoCurPage.ParentConsentPage);

            /*
            }
            else
            {
                if (idConfig.ConsentChildWitness == null ||
                    idConfig.ConsentChildWitness.Length == 0)
                    MessageBox.Show("Witness Name is Empty");
                else if (idConfig.ConsentChildDate == null ||
                    idConfig.ConsentChildDate.Length == 0)
                    MessageBox.Show("Date is Empty");
                else if (idConfig.ConsentChildDate == null ||
                    idConfig.ConsentChildDate.Length == 0)
                    MessageBox.Show("Relationship of witness to subject is Empty");
                else
                {
                    viewAdapter.IdentificationConfig = idConfig;
                    viewAdapter.SaveIdConfig();
                    SetPage(LoggingInfoCurPage.ParentConsentPage);
                }
            }*/
        }
        
        void adultConsentControl_GoBackClick(object sender, EventArgs e)
        {
            SetPage(LoggingInfoCurPage.PersonalIdPage);
        }
        void adultConsentControl_AgreeClick(object sender, EventArgs e)
        {
            if (!idConfig.HasConsent)
            {
                idConfig.ManufactureConsent();
                viewAdapter.IdentificationConfig = idConfig;
                viewAdapter.SaveIdConfig();
            }
            //viewAdapter.IdentificationConfig = idConfig;
            //viewAdapter.SaveIdConfig();
            SetPage(LoggingInfoCurPage.LogginPage);
            /*
                
            }
             else
             {
                 if (idConfig.ConsentAdultDate == null ||
                     idConfig.ConsentAdultDate.Length == 0)
                     MessageBox.Show("Date is Empty");
                 else if ((idConfig.ConsentAdultSignature == null ||
                           idConfig.ConsentAdultSignature.Length == 0) &&
                     (idConfig.ConsentAdultWitness == null ||
                           idConfig.ConsentAdultWitness.Length == 0))
                     MessageBox.Show("Please enter the name of the user or the witness.");
                 //else if (idConfig.ConsentAdultWitnessRelationship == null ||
                   //        idConfig.ConsentAdultWitnessRelationship.Length == 0)
                    //MessageBox.Show("Relationship of witness to subject is Empty");
                 else
                     MessageBox.Show("Unknown error, please contact authors of software.");
                
             }
             * */
        }

        void parentConsentControl_GoBackClick(object sender, EventArgs e)
        {
            if (idConfig.AgeGroup == AgeGroup.MinorSevenOlder)
            {
                SetPage(LoggingInfoCurPage.ChildAssentPage);
            }
            else
            {
                SetPage(LoggingInfoCurPage.PersonalIdPage);
            }
        }
        void parentConsentControl_AgreeClick(object sender, EventArgs e)
        {
            if (!idConfig.HasConsent)
            {
                idConfig.ManufactureConsent();
                viewAdapter.IdentificationConfig = idConfig;
                viewAdapter.SaveIdConfig();
            }

                SetPage(LoggingInfoCurPage.LogginPage);
            /*
            }
            else
            {
                if (idConfig.ConsentParentDate == null ||
                    idConfig.ConsentParentDate.Length == 0)
                    MessageBox.Show("Date is Empty");
                else if (idConfig.ConsentParentSignature == null ||
                          idConfig.ConsentParentSignature.Length == 0)
                    MessageBox.Show("Please enter the parent signature.");                
            }*/
        }

    }

    
}
