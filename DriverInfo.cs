using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussniessLayer;
using System.IO;

namespace DVLDProject
{
    public partial class DriverInfo : UserControl
    {
        int AppID = -1;
        public DriverInfo()
        {
            InitializeComponent();
        }

        
        private string _IssueReason(int IssueReason)
        {
            switch(IssueReason)
            {
                case 1:
                    return "First Time";
                case 2:
                    return "Lost";
                case 3:
                    return "Damaged";
                case 4:
                    return "Renew";
                default:
                    return "NotDefination";
            }
        }

        private string _Gendor(bool Gendor)
        {
            if (Gendor)
                return "Female";
            else
                return "Male";
        }

        private string _Notes(string Notes)
        {
            if (Notes != "")
                return Notes;
            else
                return "No Notes";
        }

        private string _IsActive(bool IsActive)
        {
            if (IsActive)
                return "Yes";
            else
                return "No";
        }

        private void _PersonImage(PictureBox pbPersonalImage, clsPerson person)
        {
            if (person.ImagePath != "")
            {
                using (FileStream stream = new FileStream(person.ImagePath, FileMode.Open, FileAccess.Read))
                {
                    pbPersonalImage.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
            }
            else if (person.Gendor)
                pbPersonalImage.Image = Properties.Resources.patient_female;
            else
                pbPersonalImage.Image = Properties.Resources.person_boy;
        }

        public void LoadLicenseData(int AppID)
        {
            this.AppID = AppID;

            clsLicense license = clsLicense.FindByAppID(AppID);
            clsDriver driver = clsDriver.FindByID(license.DriverID);
            clsPerson person = clsPerson.Find(driver.PersonID);

            lblClassName.Text = clsLicenseClass.Find(license.LicenseClassID).ClassName;
            lblName.Text = person.FullLongName();
            lblLicenseID.Text = license.LicenseID.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblGendor.Text = _Gendor(person.Gendor);
            lblIssueDate.Text = license.IssueDate.ToShortDateString();
            lblIssueReason.Text = _IssueReason(license.IssueReason);
            lblNotes.Text = _Notes(license.Notes);
            lblIsActive.Text = _IsActive(license.IsActive);
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblDriverID.Text = driver.DriverID.ToString();
            lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
            _PersonImage(pbPersonalImage, person);




        }

    }
}
