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
    public partial class ScheduleTest : UserControl
    {

        int LocalLDAppID = -1;
        int PreTestsCount = 0;
        int UserID = -1;
        int AppointmentID = -1;
        enum enMode { Vision=1, Written=2, Street=3}

        enMode TestType = enMode.Vision;
        public ScheduleTest()
        {
            InitializeComponent();
        }

        private void _LoadTestType()
        {
            switch(clsLDVLAView.Find(this.LocalLDAppID).PassedTestCount)
            {
                case 0:
                    this.TestType = enMode.Vision;
                    gbTestType.Text = "Vision Test";
                    pbTest.Image = Properties.Resources.address;
                    lblFees.Text = clsTestType.Find(1).TestFee.ToString();
                    break;
                case 1:
                    this.TestType = enMode.Written;
                    gbTestType.Text = "Written Test";
                    pbTest.Image = Properties.Resources.clipboard;
                    lblFees.Text = clsTestType.Find(2).TestFee.ToString();
                    break;
                case 2:
                    this.TestType = enMode.Street;
                    gbTestType.Text = "Street Test";
                    pbTest.Image = Properties.Resources.home;
                    lblFees.Text = clsTestType.Find(3).TestFee.ToString();
                    break;
                default:
                    break;

            }
        }

        private bool _IsRetakeTest()
        {
            if (this.AppointmentID != -1)
                return (this.PreTestsCount > 0) && (!clsTestAppointment.FindByID(this.AppointmentID).IsLocked);
            else
                return (this.PreTestsCount > 0);
        }

        private bool _IsEndedAppointment()
        {
            if (this.AppointmentID != -1)
                return (this.PreTestsCount > 0) && (clsTestAppointment.FindByID(this.AppointmentID).IsLocked);
            else
                return false;

        }

        private bool _IsTestAppointmentLocked()
        {
            if (this.AppointmentID != -1)
                return clsTestAppointment.FindByID(this.AppointmentID).IsLocked;
            else
                return false;
        }

        public void LoadInfo(int LDLAppID, int PerTestCount, int UserID, int AppointmentID=-1)
        {
            this.LocalLDAppID = LDLAppID;
            this.PreTestsCount = PerTestCount;
            this.UserID = UserID;
            this.AppointmentID = AppointmentID;

            _LoadTestType();

            if(_IsRetakeTest())
            {
                lblTitle.Text = "Schedule Retake Test";
                gbRetakeTest.Enabled = true;
                lblRAppFees.Text = "5";
                if (this.AppointmentID != -1)
                    lblRTestAppID.Text = this.AppointmentID.ToString();
            }

            if(_IsEndedAppointment())
            {
                lblAppointmentLocked.Visible = true;
                gbRetakeTest.Enabled = true;
            }
            else
                lblAppointmentLocked.Visible = false;

            lblDLAppID.Text = this.LocalLDAppID.ToString();
            lblClassName.Text = clsLicenseClass.Find(clsLDLApp.FindByID(this.LocalLDAppID).LicenseClassID).ClassName;
            lblPersonName.Text = clsPerson.Find(clsLDVLAView.Find(this.LocalLDAppID).NationalNo).FullLongName();
            lblTrial.Text = this.PreTestsCount.ToString();
            if (this.AppointmentID != -1)
                dtpDate.Value = clsTestAppointment.FindByID(this.AppointmentID).AppointmentDate;
            else
                dtpDate.Value = DateTime.Today;
            lblTotalFees.Text = (Convert.ToDouble(lblFees.Text) + Convert.ToDouble(lblRAppFees.Text)).ToString();

            if (_IsTestAppointmentLocked())
            {
                dtpDate.Enabled = false;
                btnSave.Enabled = false;
            }
                
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
                clsTestAppointment Appointment = new clsTestAppointment();
            if (this.AppointmentID != -1)
                Appointment = clsTestAppointment.FindByID(this.AppointmentID);

            Appointment.TestTypeID = (int)this.TestType;
            Appointment.LDLAppID = this.LocalLDAppID;
            Appointment.AppointmentDate = dtpDate.Value;
            Appointment.PaidFees = Convert.ToDouble(lblTotalFees.Text);
            Appointment.CreatedByUserID = this.UserID;
            Appointment.IsLocked = false;

            if (Appointment.Save())
            {
                MessageBox.Show("Data saved successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (_IsRetakeTest())
                    lblRTestAppID.Text = Appointment.AppointmentID.ToString();
            }
            else
                MessageBox.Show("Failed to save data", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
