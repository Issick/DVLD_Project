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
    public partial class TestAppointments : UserControl
    {
        int CurrentUserID = -1;
        int LDLAppID = -1;
        bool IsThereActiveAppointment = false;
        enum enMode { Vision = 1, Written = 2, Street = 3 }

        enMode TestType = enMode.Vision;

        public TestAppointments()
        {
            InitializeComponent();
        }

        private void _LoadTestType()
        {
            switch (clsLDVLAView.Find(this.LDLAppID).PassedTestCount)
            {
                case 0:
                    this.TestType = enMode.Vision;
                    lblTitle.Text = "Vision Test Appointments";
                    pbTest.Image = Properties.Resources.address;
                    break;
                case 1:
                    this.TestType = enMode.Written;
                    lblTitle.Text = "Written Test Appointments";
                    pbTest.Image = Properties.Resources.clipboard;
                    break;
                case 2:
                    this.TestType = enMode.Street;
                    lblTitle.Text = "Street Test Appointments";
                    pbTest.Image = Properties.Resources.home;
                    break;
                default:
                    break;

            }
        }

        private void _RefreshList()
        {
            DataView dv = clsTestAppointment.GetAllAppointments().DefaultView;

            string Filter = "TestTypeID = "+ ((int)this.TestType).ToString() +"AND LocalDrivingLicenseApplicationID = " + this.LDLAppID.ToString();
            dv.RowFilter = Filter;

            dgvAppointmentsList.DataSource = dv;

            dgvAppointmentsList.Columns["TestTypeID"].Visible = false;
            dgvAppointmentsList.Columns["LocalDrivingLicenseApplicationID"].Visible = false;
            dgvAppointmentsList.Columns["CreatedByUserID"].Visible = false;

            lblRecordsNum.Text = dgvAppointmentsList.RowCount.ToString();

            for (int i = 0; i < dv.Count; i++)
            {
                if ((bool)dgvAppointmentsList.Rows[i].Cells[6].Value == true)
                {
                    if (clsTest.FindByAppointmentID((int)dgvAppointmentsList.Rows[i].Cells[0].Value).TestResult)
                        btnAddNew.Enabled = false;
                }
            }

            dv.RowFilter = "IsLocked = 'false'";
            this.IsThereActiveAppointment = (dv.Count > 0);
            dv.RowFilter = Filter;

        }

        private bool _HasAnActiveAppointment()
        {
            bool IsActive = false;

            if (dgvAppointmentsList.RowCount != 0)
            {
                for (int i = 0; i < dgvAppointmentsList.RowCount; i++)
                {
                    if ((bool)dgvAppointmentsList.Rows[i].Cells[6].Value == false)
                    {
                        IsActive = true;
                        break;
                    }

                }
            }

            return IsActive;
        }

        private int _RetakeTestsCount()
        {
            if (IsThereActiveAppointment)
                return dgvAppointmentsList.RowCount - 1;
            else
                return dgvAppointmentsList.RowCount;

        }

        public void LoadInfo(int LDLAppID, int CurrentUserID)
        {
            this.LDLAppID = LDLAppID;
            this.CurrentUserID = CurrentUserID;

            _LoadTestType();

            dlaInfoWithAppBasicInfo1.LoadInfo(this.LDLAppID);

            _RefreshList();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if (_HasAnActiveAppointment())
                MessageBox.Show("Person has an active appointment for this test," +
                    " you cannot add new one", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
            {
                ScheduleTestForm Frm = new ScheduleTestForm(this.LDLAppID, this.CurrentUserID, _RetakeTestsCount());
                Frm.ShowDialog();
                _RefreshList();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleTestForm Frm = new ScheduleTestForm(this.LDLAppID, this.CurrentUserID, _RetakeTestsCount(), (int)dgvAppointmentsList.CurrentRow.Cells[0].Value);
            Frm.ShowDialog();
            _RefreshList();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TakeTestForm Frm = new TakeTestForm(this.LDLAppID, _RetakeTestsCount(), this.CurrentUserID, (int)dgvAppointmentsList.CurrentRow.Cells[0].Value);
            Frm.ShowDialog();
            _RefreshList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if ((bool)dgvAppointmentsList.CurrentRow.Cells[6].Value)
                takeTestToolStripMenuItem.Enabled = false;
            else
                takeTestToolStripMenuItem.Enabled = true;
        }
    }
}
