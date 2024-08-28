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
    public partial class ChangePassword : Form
    {
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;

        clsUser User = new clsUser();
        public ChangePassword(int ID)
        {
            InitializeComponent();
            User = clsUser.Find(ID);
        }

        private void _EmptyValidation(TextBox Sender, CancelEventArgs e, string Message)
        {
            if (string.IsNullOrWhiteSpace(Sender.Text))
            {
                e.Cancel = true;
                Sender.Focus();
                errorProvider1.SetError(Sender, Message);
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Sender, "");
            }
        }

        private void txtCurrentPass_Validating(object sender, CancelEventArgs e)
        {
            _EmptyValidation((TextBox)sender, e, "Has to have a value");

            if(txtCurrentPass.Text!=User.Password)
            {
                e.Cancel = true;
                txtCurrentPass.Focus();
                errorProvider1.SetError(txtCurrentPass, "Dose Not match the Password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPass, "");
            }
        }

        private void txtNewPass_Validating(object sender, CancelEventArgs e)
        {
            _EmptyValidation((TextBox)sender, e, "Has to have a value");
        }

        private void txtPassConfirm_Validating(object sender, CancelEventArgs e)
        {
            if(txtNewPass.Text!=txtPassConfirm.Text)
            {
                e.Cancel = true;
                txtPassConfirm.Focus();
                errorProvider1.SetError(txtPassConfirm, "Dose Not match the Password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassConfirm, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassConfirm.Text) && txtNewPass.Text==txtPassConfirm.Text)
            {
                User.Password = txtNewPass.Text;
                if (User.Save())
                    MessageBox.Show("Password changed successfully", "Password changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("System Error!", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Change Password steps are uncomplete!", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            userCard1.LoadUserData(User.ID);
        }

        private void ChangePassword_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
