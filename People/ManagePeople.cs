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
    public partial class ManagePeople : Form
    {

        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;   
        public ManagePeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeopleList()
        {
            dgvPeopleList.DataSource = clsPerson.GetAllPersons();
            dgvPeopleList.ReadOnly = true;
            lblRecordsNum.Text = (dgvPeopleList.RowCount-1).ToString();
        }

        private void ManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            cbFilter.SelectedIndex = 0;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddEditPerson NewPerson = new AddEditPerson(-1);
            NewPerson.ShowDialog();
            _RefreshPeopleList();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "None")
                txtFilter.Visible = false;
            else
                txtFilter.Visible = true;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilter.Text=="Person ID")
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click_1(sender,e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditPerson Person1 = new AddEditPerson((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            Person1.ShowDialog();
            _RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to Delete this Person!","Delete a Person",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
            {
                if (clsPerson.DeletePerson((int)dgvPeopleList.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person has deleted successfully", "Delete Process");
                    _RefreshPeopleList();
                }
                else
                    MessageBox.Show("Can Not Delete this Person", "Delete Process",MessageBoxButtons.OK ,MessageBoxIcon.Error);
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilter.Text))
            {
                DataView dataView = clsPerson.GetAllPersons().DefaultView;
                try
                {
                    string filter = cbFilter.Text + "='" + txtFilter.Text+"'";
                    dataView.RowFilter = filter;
                }
                catch
                {
                    //dataView.RowFilter = "PersonID=-1";
                }
                dgvPeopleList.DataSource = dataView;
                lblRecordsNum.Text = (dataView.Count).ToString();
            }
            else
                _RefreshPeopleList();

            
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonDetail Person1 = new PersonDetail((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            Person1.ShowDialog();
            _RefreshPeopleList();
        }

        private void ManagePeople_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
