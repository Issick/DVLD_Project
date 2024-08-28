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
    public partial class UpdateTestType : Form
    {
        int TestTypeID = -1;
        public UpdateTestType(int TestTypeID)
        {
            InitializeComponent();
            this.TestTypeID = TestTypeID;
        }

        private void UpdateTestType_Load(object sender, EventArgs e)
        {
            clsTestType Test1 = clsTestType.Find(this.TestTypeID);

            lblID.Text = Test1.ID.ToString();
            txtTestTitle.Text = Test1.TestTitle;
            txtTestDescription.Text = Test1.TestDescription;
            txtTestFees.Text = Test1.TestFee.ToString();
        }

        private void txtTestFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsTestType Test1 = clsTestType.Find(this.TestTypeID);

            Test1.TestTitle = txtTestTitle.Text;
            Test1.TestDescription = txtTestDescription.Text;
            Test1.TestFee = Convert.ToDouble(txtTestFees.Text);

            if(Test1.Save())
                MessageBox.Show("Test Type updated successfully", "Test Type Update");
            else
                MessageBox.Show("Test Type did not update!", "Test Type Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
