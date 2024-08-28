using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussniessLayer;

namespace DVLDProject
{
    public partial class IssueDriverForFirstTime : Form
    {
        int LDLAppID = -1;
        int CurrentUserID = -1;
        public IssueDriverForFirstTime(int LDLAppID, int CurrentUserID)
        {
            InitializeComponent();
            this.LDLAppID = LDLAppID;
            this.CurrentUserID = CurrentUserID;
        }

        private void IssueDriverForFirstTime_Load(object sender, EventArgs e)
        {
            dlaInfoWithAppBasicInfo1.LoadInfo(this.LDLAppID);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsLDVLAView DLVLView = clsLDVLAView.Find(this.LDLAppID);

            clsDriver driver = new clsDriver();

            driver.PersonID = clsPerson.Find(DLVLView.NationalNo).ID;
            driver.CreatedByUserID = this.CurrentUserID;

            if(driver.Save())
            {
                clsLicense license = new clsLicense();
                license.AppID = clsLDLApp.FindByID(this.LDLAppID).ApplicationID;
                license.DriverID = driver.DriverID;
                license.LicenseClassID = clsLDLApp.FindByID(this.LDLAppID).LicenseClassID;
                license.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(license.LicenseClassID).DefValidLength);
                license.Notes = txtNotes.Text;
                license.PaidFees = clsLicenseClass.Find(license.LicenseClassID).ClassFees;
                if (DateTime.Compare(DateTime.Now, license.ExpirationDate) < 0)
                    license.IsActive = true;
                else
                    license.IsActive = false;
                license.IssueReason = 1;        //In this case equal 1 for new license for first time
                license.CreatedByUserID = this.CurrentUserID;
                if(license.Save())
                {
                    MessageBox.Show("New License has added successfully, License ID : " + license.LicenseID.ToString(),
                        "New License Add", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsApplication Application = clsApplication.Find(clsLDLApp.FindByID(this.LDLAppID).ApplicationID);
                    Application.AppStatus = 3;
                    if(Application.UpdateApp())
                        MessageBox.Show("Application has been completed",
                        "Complete Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        MessageBox.Show("Application has not been completed!",
                        "Complete Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("New License has not been Added!",
                        "New License Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("New Driver has not been Added!",
                        "New Driver Add", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
