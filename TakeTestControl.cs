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
    public partial class TakeTestControl : UserControl
    {
        int LocalLDAppID = -1;
        int PreTestsCount = 0;
        int UserID = -1;
        int AppointmentID = -1;
        enum enMode { Vision = 1, Written = 2, Street = 3 }

        enMode TestType = enMode.Vision;

        public TakeTestControl()
        {
            InitializeComponent();
        }

        private void _LoadTestType()
        {
            switch (clsLDVLAView.Find(this.LocalLDAppID).PassedTestCount)
            {
                case 0:
                    this.TestType = enMode.Vision;
                    gbTestType.Text = "Vision Test";
                    pbTest.Image = Properties.Resources.address;
                    break;
                case 1:
                    this.TestType = enMode.Written;
                    gbTestType.Text = "Written Test";
                    pbTest.Image = Properties.Resources.clipboard;
                    break;
                case 2:
                    this.TestType = enMode.Street;
                    gbTestType.Text = "Street Test";
                    pbTest.Image = Properties.Resources.home;
                    break;
                default:
                    break;

            }
        }

        public bool Save()
        {
            if (rbPass.Checked == false && rbFail.Checked == false)
            {
                MessageBox.Show("Cannot Save without result, Please choose Pass or Fail", "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                clsTest test = new clsTest();
                test.TestAppointmentID = this.AppointmentID;
                if (rbPass.Checked)
                    test.TestResult = true;
                else
                    test.TestResult = false;
                test.Notes = textBox1.Text;
                test.CreatedByUserID = this.UserID;

                if (test.Save())
                {
                    MessageBox.Show("Test Results have saved successfully", "Result Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblTestID.Text = test.ID.ToString();
                    clsTestAppointment Appointment = clsTestAppointment.FindByID(this.AppointmentID);
                    Appointment.IsLocked = true;
                    if (Appointment.Save())
                        MessageBox.Show("Appointment has Locked", "Lock Appointment");
                    else
                        MessageBox.Show("Appointment has not Locked", "Lock Appointment");
                    return true;
                }
                else
                {
                    return false;
                }


            }
        }

        public void LoadInfo(int LDLAppID, int PerTestCount, int UserID, int AppointmentID)
        {
            this.LocalLDAppID = LDLAppID;
            this.PreTestsCount = PerTestCount;
            this.UserID = UserID;
            this.AppointmentID = AppointmentID;

            _LoadTestType();

            lblDLAppID.Text = this.LocalLDAppID.ToString();
            lblClassName.Text = clsLicenseClass.Find(clsLDLApp.FindByID(this.LocalLDAppID).LicenseClassID).ClassName;
            lblPersonName.Text = clsPerson.Find(clsLDVLAView.Find(this.LocalLDAppID).NationalNo).FullLongName();
            lblTrial.Text = this.PreTestsCount.ToString();
            lblDate.Text = clsTestAppointment.FindByID(this.AppointmentID).AppointmentDate.ToShortDateString();
            lblFees.Text = clsTestAppointment.FindByID(this.AppointmentID).PaidFees.ToString();

        }

    }
}
