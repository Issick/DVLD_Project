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
    public partial class ListTestType : Form
    {
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;

        public ListTestType()
        {
            InitializeComponent();
        }

        private void _RefreshTestTypesList()
        {
            dgvTestTypesList.DataSource = clsTestType.GetAllTestTypes();
            dgvTestTypesList.ReadOnly = true;
            lblRecordsNum.Text = (dgvTestTypesList.RowCount - 1).ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListTestType_Load(object sender, EventArgs e)
        {
            _RefreshTestTypesList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTestType Frm = new UpdateTestType((int)dgvTestTypesList.CurrentRow.Cells[0].Value);
            Frm.ShowDialog();
            _RefreshTestTypesList();
        }

        private void ListTestType_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
