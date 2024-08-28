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

namespace DVLDProject
{
    public partial class DLAInfoWithAppBasicInfo : UserControl
    {

        int LDLAppID = -1;
        public DLAInfoWithAppBasicInfo()
        {
            InitializeComponent();
        }

        private string _AppStatus(int Status)
        {
            switch(Status)
            {
                case 1:
                    return "New";
                case 2:
                    return "Cancelled";
                case 3:
                    return "Completed";
                default:
                    return "????";
            }
        }

        public void LoadInfo(int LocalDrivingLicenseAppID)
        {
            this.LDLAppID = LocalDrivingLicenseAppID;

            clsLDLApp LocalApp = clsLDLApp.FindByID(this.LDLAppID);
            clsApplication BaseApp = clsApplication.Find(LocalApp.ApplicationID);


            lblDLAppID.Text = LocalApp.ID.ToString();
            lblLicenseClass.Text = clsLicenseClass.Find(LocalApp.LicenseClassID).ClassName;
            lblPassedTests.Text = clsLDVLAView.Find(this.LDLAppID).PassedTestCount.ToString() + "/3";

            lblBaseAppID.Text = BaseApp.ID.ToString();
            lblStatus.Text= _AppStatus(BaseApp.AppStatus);
            lblFees.Text = BaseApp.PaidFees.ToString();
            lblType.Text = clsApplicationType.Find(BaseApp.AppTypeID).ApplicationTitle;
            lblApplicant.Text = clsPerson.Find(BaseApp.AppPersonID).FullLongName();

            lblDate.Text = BaseApp.AppDateStart.ToShortDateString();
            lblStatusDate.Text = BaseApp.LastStatusDate.ToShortDateString();
            lblUser.Text = clsUser.Find(BaseApp.CreatedUserID).UserName;

            llPersonInfo.Enabled = true;

            if (clsLicense.IsLicenseExistByAppID(clsLDLApp.FindByID(this.LDLAppID).ApplicationID))
                llLicenseInfo.Enabled = true;


        }

        private void llPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PersonDetail Frm = new PersonDetail(clsApplication.Find(clsLDLApp.FindByID(this.LDLAppID).ApplicationID).AppPersonID);
            Frm.ShowDialog();
        }
    }
}
