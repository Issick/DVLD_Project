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
    public partial class UpdateAppType : Form
    {
        int AppID = -1;
        
        public UpdateAppType(int AppID)
        {
            InitializeComponent();
            this.AppID = AppID;
        }

        private void UpdateAppType_Load(object sender, EventArgs e)
        {
            clsApplicationType App1 = clsApplicationType.Find(this.AppID);

            lblAppID.Text = App1.ID.ToString();
            txtAppTitle.Text = App1.ApplicationTitle;
            txtFees.Text = App1.ApplicationFee.ToString();

        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsApplicationType App1 = clsApplicationType.Find(this.AppID);

            App1.ApplicationTitle = txtAppTitle.Text;
            App1.ApplicationFee = Convert.ToDouble(txtFees.Text);

            if (App1.Save())
                MessageBox.Show("Application Type updated successfully", "Application Type Update");
            else
                MessageBox.Show("Application Type did not update!", "Application Type Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
