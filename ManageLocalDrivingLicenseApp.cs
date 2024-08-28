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
    public partial class ManageLocalDrivingLicenseApp : Form
    {
        
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;

        int CurrentUserID = -1;

        string FilterColumn = "";
        public ManageLocalDrivingLicenseApp(int CurrentUserID)
        {
            InitializeComponent();
            this.CurrentUserID = CurrentUserID;
        }

        private void _RefreshAppsList()
        {
            dgvAppsList.DataSource = clsLDVLAView.GetAllAplications();
            dgvAppsList.ReadOnly = true;
            lblRecordsNum.Text = dgvAppsList.RowCount.ToString();
        }

        private void _SetFilter()
        {
            switch(cbFilter.SelectedIndex)
            {
                case 0:
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    cbFilter.Tag = "Equal";
                    break;
                case 1:
                    FilterColumn = "ClassName";
                    cbFilter.Tag = "Like";
                    break;
                case 2:
                    FilterColumn = "NationalNo";
                    cbFilter.Tag = "Equal";
                    break;
                case 3:
                    FilterColumn = "FullName";
                    cbFilter.Tag = "Like";
                    break;
                case 4:
                    FilterColumn = "ApplicationDate";
                    cbFilter.Tag = "Equal";
                    break;
                case 5:
                    FilterColumn = "PassedTestCount";
                    cbFilter.Tag = "Equal";
                    break;

            }
        }

        private int _AppStatus()
        {
            int LocalApp = clsLDLApp.FindByID((int)dgvAppsList.CurrentRow.Cells[0].Value).ApplicationID;
            clsApplication App = clsApplication.Find(LocalApp);
            return App.AppStatus;
        }

        private void _NewOptions()
        {
            switch((int)dgvAppsList.CurrentRow.Cells[5].Value)      //Passed Test Count
            {
                case 0:
                    contextMenuStrip1.Items[7].Enabled = true;
                    sechduleVisionTestToolStripMenuItem.Enabled = true;
                    sechduleWritenTestToolStripMenuItem.Enabled = false;
                    sechduleStreetTestToolStripMenuItem.Enabled = false;
                    contextMenuStrip1.Items[9].Enabled = false;
                    contextMenuStrip1.Items[11].Enabled = false;
                    break;
                case 1:
                    contextMenuStrip1.Items[7].Enabled = true;
                    sechduleVisionTestToolStripMenuItem.Enabled = false;
                    sechduleWritenTestToolStripMenuItem.Enabled = true;
                    sechduleStreetTestToolStripMenuItem.Enabled = false;
                    contextMenuStrip1.Items[9].Enabled = false;
                    contextMenuStrip1.Items[11].Enabled = false;
                    break;
                case 2:
                    contextMenuStrip1.Items[7].Enabled = true;
                    sechduleVisionTestToolStripMenuItem.Enabled = false;
                    sechduleWritenTestToolStripMenuItem.Enabled = false;
                    sechduleStreetTestToolStripMenuItem.Enabled = true;
                    contextMenuStrip1.Items[9].Enabled = false;
                    contextMenuStrip1.Items[11].Enabled = false;
                    break;
                case 3:
                    contextMenuStrip1.Items[7].Enabled = false;
                    contextMenuStrip1.Items[9].Enabled = true;
                    contextMenuStrip1.Items[11].Enabled = false;
                    break;
            }
        }

        private void _CanceledOptions()
        {

        }

        private void _CompletedOptions()
        {
            contextMenuStrip1.Items[0].Enabled = true;

            contextMenuStrip1.Items[2].Enabled = false;
            contextMenuStrip1.Items[3].Enabled = false;

            contextMenuStrip1.Items[5].Enabled = false;

            contextMenuStrip1.Items[7].Enabled = false;

            contextMenuStrip1.Items[9].Enabled = false;

            contextMenuStrip1.Items[11].Enabled = true;

            contextMenuStrip1.Items[13].Enabled = true;
        }

        private void ManageLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {
            _RefreshAppsList();
            cbStatus.Visible = false;
            txtFilter.Visible = false;
        }

        private void btnAddNewApp_Click(object sender, EventArgs e)
        {
            NewLocalDLApp Frm = new NewLocalDLApp(this.CurrentUserID);
            Frm.ShowDialog();
            _RefreshAppsList();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SetFilter();
            if (cbFilter.Text == null)
            {
                txtFilter.Visible = false;
                cbStatus.Visible = false;
            }
            else if(cbFilter.Text=="Status")
            {
                txtFilter.Visible = false;
                cbStatus.Visible = true;
            }
            else
            {
                txtFilter.Visible = true;
                cbStatus.Visible = false;
            }
                
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "L.D.L Application ID" || cbFilter.Text== "Application Date")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dataView = clsLDVLAView.GetAllAplications().DefaultView;

            if (!string.IsNullOrEmpty(txtFilter.Text))
            {
                string filter = "";

                if (cbFilter.Tag.ToString()=="Like")
                    filter = FilterColumn + " like '" + txtFilter.Text + "%'";
                else
                    filter = FilterColumn + "='" + txtFilter.Text + "'";

                dataView.RowFilter = filter;

                dgvAppsList.DataSource = dataView;
                lblRecordsNum.Text = (dataView.Count).ToString();
            }
            else
                _RefreshAppsList();
        }

        private void ManageLocalDrivingLicenseApp_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to Cancel this Application!", "Cancel an Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //in this case 2 is for Cancel Status
                
                int LocalApp = clsLDLApp.FindByID((int)dgvAppsList.CurrentRow.Cells[0].Value).ApplicationID;
                clsApplication App = clsApplication.Find(LocalApp);
                App.AppStatus = 2;

                if (App.UpdateApp())
                    MessageBox.Show("Application has canceled successfully", "Cancel Process", MessageBoxButtons.OK,MessageBoxIcon.Information);
                else
                    MessageBox.Show("Application has Not canceled !", "Cancel Process",MessageBoxButtons.OK,MessageBoxIcon.Error);

                _RefreshAppsList();
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            switch(_AppStatus())
            {
                case 1:         // New
                    _NewOptions();
                    break;
                case 2:         // Cancelled
                    _CanceledOptions();
                    break;
                case 3:         // Completed
                    _CompletedOptions();
                    break;
            }
        }

        private void sechduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointmentsForm Frm = new TestAppointmentsForm((int)dgvAppsList.CurrentRow.Cells[0].Value, this.CurrentUserID);
            Frm.ShowDialog();
            _RefreshAppsList();
        }

        private void sechduleWritenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointmentsForm Frm = new TestAppointmentsForm((int)dgvAppsList.CurrentRow.Cells[0].Value, this.CurrentUserID);
            Frm.ShowDialog();
            _RefreshAppsList();
        }

        private void sechduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAppointmentsForm Frm = new TestAppointmentsForm((int)dgvAppsList.CurrentRow.Cells[0].Value, this.CurrentUserID);
            Frm.ShowDialog();
            _RefreshAppsList();
        }

        private void issueDrivingLicensFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IssueDriverForFirstTime Frm = new IssueDriverForFirstTime((int)dgvAppsList.CurrentRow.Cells[0].Value, this.CurrentUserID);
            Frm.ShowDialog();
            _RefreshAppsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseInfo Frm = new LicenseInfo(clsLDLApp.FindByID((int)dgvAppsList.CurrentRow.Cells[0].Value).ApplicationID);
            Frm.ShowDialog();

        }
    }
}
