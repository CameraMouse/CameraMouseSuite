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
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace CameraMouseSuite
{

    public enum AgeGroup
    {
        MinorSixYounger,
        MinorSevenOlder,
        Adult
    }

    [XmlRoot("IdConfig")]
    public class CMSIdentificationConfig
    {
        public CMSIdentificationConfig Clone()
        {
            CMSIdentificationConfig idConfig = new CMSIdentificationConfig();
            //idConfig.NickName = NickName;
            idConfig.FirstName = FirstName;
            idConfig.LastName = LastName;
            idConfig.Email = Email;
            idConfig.City = City;
            idConfig.StateProvince = StateProvince;
            idConfig.Country = Country;
            idConfig.AgeGroup = AgeGroup;
            idConfig.ConsentAdultSignature = ConsentAdultSignature;
            idConfig.ConsentAdultWitness = ConsentAdultWitness;
            idConfig.ConsentAdultDate = ConsentAdultDate;
            idConfig.ConsentChildWitness = ConsentChildWitness;
            idConfig.ConsentChildWitnessRelationship = ConsentChildWitnessRelationship;
            idConfig.ConsentChildDate = ConsentChildDate;
            idConfig.ConsentParentSignature = ConsentParentSignature;
            idConfig.ConsentParentDate = ConsentParentDate;
            idConfig.Condition = Condition;
            idConfig.NotStudy = NotStudy;
            return idConfig;
        }

        private bool notStudy = false;
        [XmlIgnore()]
        public bool NotStudy
        {
            get
            {
                return notStudy;
            }
            set
            {
                notStudy = value;
            }
        }

        private string firstName = null;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        private string lastName = null;
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        private string email = null;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        private string city = null;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }

        private string condition = null;
        public string Condition
        {
            get
            {
                return condition;
            }
            set
            {
                condition = value;
            }
        }

        private string stateProvince = null;
        public string StateProvince
        {
            get
            {
                return stateProvince;
            }
            set
            {
                stateProvince = value;
            }
        }

        private string country = null;
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
            }
        }

        private AgeGroup ageGroup;
        public AgeGroup AgeGroup
        {
            get
            {
                return ageGroup;
            }
            set
            {
                ageGroup = value;
            }
        }

        private string consentAdultSignature = null;
        public string ConsentAdultSignature
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentAdultSignature;
            }
            set
            {
                consentAdultSignature = value;
            }
        }

        private string consentAdultWitness = null;
        public string ConsentAdultWitness
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentAdultWitness;
            }
            set
            {
                consentAdultWitness = value;
            }
        }

        private string consentAdultDate = null;
        public string ConsentAdultDate
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentAdultDate;
            }
            set
            {
                consentAdultDate = value;
            }
        }

        private string consentChildWitness = null;
        public string ConsentChildWitness
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentChildWitness;
            }
            set
            {
                consentChildWitness = value;
            }
        }

        private string consentChildWitnessRelationship = null;
        public string ConsentChildWitnessRelationship
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentChildWitnessRelationship;
            }
            set
            {
                consentChildWitnessRelationship = value;
            }
        }

        private string consentChildDate = null;
        public string ConsentChildDate
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentChildDate;
            }
            set
            {
                consentChildDate = value;
            }
        }

        private string consentParentDate = null;
        public string ConsentParentDate
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentParentDate;
            }
            set
            {
                consentParentDate = value;
            }
        }

        private string consentParentSignature = null;
        public string ConsentParentSignature
        {
            get
            {
                if (NotStudy)
                    return "NS";
                return consentParentSignature;
            }
            set
            {
                consentParentSignature = value;
            }
        }

        public bool PersonalInfoEntered
        {
            get
            {
                if (firstName != null && firstName.Length > 0 &&
                    lastName != null && firstName.Length > 0)
                    return true;
                return false;
            }
        }

        public bool HasConsent
        {
            get
            {
                if (notStudy)
                    return PersonalInfoEntered;

                //if (nickName == null || nickName.Length == 0)
                  //  return false;

                if (firstName == null || firstName.Length == 0)
                    return false;

                if (lastName == null || lastName.Length == 0)
                    return false;

                if (ageGroup == AgeGroup.Adult)
                {
                    if (consentAdultDate == null || consentAdultDate.Length == 0)
                        return false;

                    if (consentAdultSignature != null && consentAdultSignature.Length > 0)
                    {
                    }
                    else if ((consentAdultWitness == null || consentAdultWitness.Length == 0))// ||
                            //(consentAdultWitnessRelationship == null || consentAdultWitnessRelationship.Length == 0))
                    {
                        return false;
                    }
                }
                else if(ageGroup == AgeGroup.MinorSevenOlder)
                {
                    if (consentChildDate == null || this.consentChildDate.Length == 0)
                        return false;

                    if (consentChildWitness == null || this.consentChildWitness.Length == 0)
                        return false;

                    if (consentChildWitnessRelationship == null || this.consentChildWitnessRelationship.Length == 0)
                        return false;

                    if(consentParentDate == null || consentParentDate.Length == 0)
                        return false;

                    if(consentParentSignature == null || consentParentSignature.Length == 0)
                        return false;
                }
                else if (ageGroup == AgeGroup.MinorSixYounger)
                {
                    if (consentParentDate == null || consentParentDate.Length == 0)
                        return false;

                    if (consentParentSignature == null || consentParentSignature.Length == 0)
                        return false;
                }

                return true;
            }
        }

        public void ManufactureConsent()
        {
            if (ageGroup == AgeGroup.Adult)
            {
                consentAdultSignature = "Mn" + firstName + " " + lastName;
                consentAdultDate = DateTime.Now.ToShortDateString();
            }
            else if (ageGroup == AgeGroup.MinorSevenOlder)
            {
                consentChildDate = DateTime.Now.ToShortDateString();
                consentChildWitness = "MnWitness Witness";
                consentChildWitnessRelationship = "MnRelationship";
                consentParentSignature = "MnParent MnParent";
                consentParentDate = DateTime.Now.ToShortDateString();
            }
            else if (ageGroup == AgeGroup.MinorSixYounger)
            {
                consentParentSignature = "MnParent Parent";
                consentParentDate = DateTime.Now.ToShortDateString();
            }
        }

        public void UpdateIdentificationConfig(CMSIdentificationConfig idConfig)
        {
            FirstName = idConfig.FirstName;
            LastName = idConfig.LastName;
            Email = idConfig.Email;
            City = idConfig.City;
            StateProvince = idConfig.StateProvince;
            Country = idConfig.Country;
            AgeGroup = idConfig.AgeGroup;
            ConsentAdultSignature = idConfig.ConsentAdultSignature;
            ConsentAdultWitness = idConfig.ConsentAdultWitness;
            //ConsentAdultWitnessRelationship = idConfig.ConsentAdultWitnessRelationship;
            ConsentAdultDate = idConfig.ConsentAdultDate;
            ConsentChildWitness = idConfig.ConsentChildWitness;
            ConsentChildWitnessRelationship = idConfig.ConsentChildWitnessRelationship;
            ConsentChildDate = idConfig.ConsentChildDate;
            ConsentParentDate = idConfig.ConsentParentDate;
            ConsentParentSignature = idConfig.ConsentParentSignature;
            Condition = idConfig.Condition;
            NotStudy = idConfig.NotStudy;
        }
    }

}
