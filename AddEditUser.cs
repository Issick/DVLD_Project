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
    public partial class AddEditUser : Form
    {
        enum enMode { AddNew, Update};

        enMode Mode = enMode.AddNew;

        int UserID = -1;

        bool AllowNextTab = false;
        
        public AddEditUser(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
        }

        private void _CheckMode()
        {
            if (this.UserID == -1)
                Mode = enMode.AddNew;
            else
                Mode = enMode.Update;
        }

        private int _Save()
        {
            clsUser User1 = new clsUser();

            if (this.Mode == enMode.Update)
                User1 = clsUser.Find(this.UserID);

            User1.PersonID = PersonCard1.GetID();
            User1.UserName = txtUsername.Text;
            User1.Password = txtPassword.Text;
            User1.isActive = checkBox1.Checked;

            if(User1.Save())
            {
                lblTitle.Text = "Update User";
                lblUserID.Text = User1.ID.ToString();

                if(this.Mode==enMode.AddNew)
                {
                    MessageBox.Show("User has been added successfully");
                    this.Mode = enMode.Update;
                }
                else
                {
                    MessageBox.Show("User has been updated successfully");  
                }
                return User1.ID;
            }
            else
            {
                if(this.Mode==enMode.AddNew)
                {
                    MessageBox.Show("Sorry, User did not Add!");
                    return -1;
                }
                else
                {
                    MessageBox.Show("Sorry, User did not Update!");
                    return User1.ID;
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 1)
            {
                PersonCard1.LoadPersonData(Convert.ToInt32(txtFilter.Text));
            }
            else if (cbFilter.SelectedIndex == 2)
            {
                PersonCard1.LoadPersonData(txtFilter.Text);
            }
            else
                PersonCard1.LoadPersonData(-1);
        }

        private void _AllowRejectCreatUser()
        {
            if (PersonCard1.GetID() == -1)
                MessageBox.Show("Person did not definded!", "Person Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (clsUser.Find(PersonCard1.GetID()) != null && Mode==enMode.AddNew)
            {
                MessageBox.Show("User is already Exist!", "User Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                AllowNextTab = true;
                tabControl1.SelectedIndex = 1;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _AllowRejectCreatUser();
        }

        private void AddEditUser_Load(object sender, EventArgs e)
        {
            _CheckMode();

            if (Mode == enMode.AddNew)

                lblTitle.Text = "Add New User";
            else
            {
                lblTitle.Text = "Update User";
                AllowNextTab = true;
                gbxFilter.Enabled = false;
                PersonCard1.LoadPersonData(clsUser.Find(this.UserID).PersonID);
            }
                


        }

        private void _AddEditPersonDataBack(object Sender,int ID)
        {
            PersonCard1.LoadPersonData(ID);
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            AddEditPerson Person1 = new AddEditPerson(-1);
            Person1.DataBack += _AddEditPersonDataBack;
            Person1.ShowDialog();
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

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            _EmptyValidation((TextBox)sender, e, "Username has to have a value");

            if (clsUser.isUserExist(txtUsername.Text) && this.Mode == enMode.AddNew)
            {
                e.Cancel = true;
                txtUsername.Focus();
                errorProvider1.SetError(txtUsername, "This Username is used!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUsername, "");
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!AllowNextTab)
                tabControl1.SelectedIndex = 0;
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            _EmptyValidation((TextBox)sender, e, "Password has to have a value");
        }

        private void txtPassConfirm_Validating(object sender, CancelEventArgs e)
        {
            if(txtPassword.Text!=txtPassConfirm.Text)
            {
                e.Cancel = true;
                txtPassConfirm.Focus();
                errorProvider1.SetError(txtPassConfirm, "Dose not match the Password you entered!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassConfirm, "");
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassConfirm.Text))
                this.UserID = _Save();
        }
    }
}
