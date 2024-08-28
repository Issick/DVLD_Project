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
    public partial class ManageUsers : Form
    {
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;
        
        public ManageUsers()
        {
            InitializeComponent();
        }

        private DataView _RefreshUserListDataView()
        {

            DataTable dt = clsUser.GetAllUsers();
            DataColumn newCol = new DataColumn("FullName", typeof(System.String));
            dt.Columns.Add(newCol);
            foreach(DataRow row in dt.Rows)
            {
                row["FullName"] = clsPerson.Find((int)row["PersonID"]).FullLongName();
            }

            DataView dv = dt.DefaultView;
            dv.Table.Columns["Password"].ColumnMapping = MappingType.Hidden;

            return dv;

            
        }

        private void _RefreshDataGridView(DataView dv)
        {
            dgvUserList.DataSource = dv;
            dgvUserList.Columns["FullName"].DisplayIndex = 2;
            lblRecordsNum.Text = (dgvUserList.RowCount - 1).ToString();
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            _RefreshDataGridView(_RefreshUserListDataView());
            cbFilter.SelectedIndex = 0;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //AddEditPerson NewPerson = new AddEditPerson(-1);
            //NewPerson.ShowDialog();
            //_RefreshUserListDataView();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "None")
            {
                txtFilter.Visible = false;
                cbActiveUsers.Visible = false;
            }
            else if (cbFilter.Text == "IsActive")
            {
                txtFilter.Visible = false;
                cbActiveUsers.Visible = true;
            }
            else
            {
                txtFilter.Text = "";
                txtFilter.Visible = true;
                cbActiveUsers.Visible = false;
            }
                
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "PersonID" || cbFilter.Text=="UserID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click_1(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AddEditPerson Person1 = new AddEditPerson((int)dgvUserList.CurrentRow.Cells[0].Value);
            //Person1.ShowDialog();
            //_RefreshUserListDataView();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Are you sure to Delete this Person!", "Delete a Person", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            //{
            //    if (clsUser.DeletePerson((int)dgvUserList.CurrentRow.Cells[0].Value))
            //    {
            //        MessageBox.Show("Person has deleted successfully", "Delete Process");
            //        _RefreshUserListDataView();
            //    }
            //    else
            //        MessageBox.Show("Can Not Delete this Person", "Delete Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PersonDetail Person1 = new PersonDetail((int)dgvUserList.CurrentRow.Cells[0].Value);
            //Person1.ShowDialog();
            //_RefreshUserListDataView();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dv = _RefreshUserListDataView();
            string filter = "";

            if (cbFilter.Text == "PersonID" || cbFilter.Text == "UserID")
            {
                if (!string.IsNullOrEmpty(txtFilter.Text))
                {
                    filter = cbFilter.Text + "='" + txtFilter.Text + "'";
                    dv.RowFilter = filter;
                }
                //_RefreshDataGridView(dv);

            }
            else
            {
                dv.RowFilter = string.Format("{0} like '{1}%'", cbFilter.Text, txtFilter.Text);
            }
            _RefreshDataGridView(dv);
            //_RefreshDataGridView(_RefreshUserListDataView());

        }

        private void cbActiveUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dv = _RefreshUserListDataView();

            if (cbActiveUsers.Text == "Yes")
            {
                string filter = cbFilter.Text + "= 1";
                dv.RowFilter = filter;
            }
            else if (cbActiveUsers.Text == "No")
            {
                string filter = cbFilter.Text + "= 0";
                dv.RowFilter = filter;
            }
            //_RefreshDataGridView(dv);
            else
            {

            }
            _RefreshDataGridView(dv);

        }

        private void ManageUsers_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEditUser User1 = new AddEditUser(-1);
            User1.ShowDialog();
            _RefreshDataGridView(_RefreshUserListDataView());
        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to Delete this User!", "Delete a User", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (clsUser.DeleteUser((int)dgvUserList.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User has deleted successfully", "Delete Process");
                    _RefreshDataGridView(_RefreshUserListDataView());
                }
                else
                    MessageBox.Show("Can Not Delete this User", "Delete Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ChangePassword change = new ChangePassword((int)dgvUserList.CurrentRow.Cells[0].Value);
            change.ShowDialog();
        }

        private void editToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AddEditUser User1 = new AddEditUser((int)dgvUserList.CurrentRow.Cells[0].Value);
            User1.ShowDialog();
            _RefreshDataGridView(_RefreshUserListDataView());
        }

        private void showDetailsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            UserInfo Frm = new UserInfo((int)dgvUserList.CurrentRow.Cells[0].Value);
            Frm.ShowDialog();
            _RefreshDataGridView(_RefreshUserListDataView());
        }
    }
}
