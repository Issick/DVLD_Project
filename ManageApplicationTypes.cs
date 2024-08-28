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
    public partial class ManageApplicationTypes : Form
    {
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;

        public ManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void _RefreshApplicationList()
        {
            dgvPeopleList.DataSource = clsApplicationType.GetAllApplications();
            dgvPeopleList.ReadOnly = true;
            lblRecordsNum.Text = (dgvPeopleList.RowCount - 1).ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateAppType Frm = new UpdateAppType((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            Frm.ShowDialog();
            _RefreshApplicationList();
        }

        private void ManageApplicationTypes_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
