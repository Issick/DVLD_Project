using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject
{
    public partial class Main : Form
    {
        enum enMode
        {
            ManagePeople, ManageUsers, CurrentUserInfo, ChangePass,
            ApplicationType, TestType, ManageLDLApp, NewLDLApp, Empty
        };

        public int CurrentUserID = -1;

        enMode Mode = enMode.Empty;

        public bool IsLogOut = false;
        public Main(int UserID)
        {
            InitializeComponent();
            CurrentUserID = UserID;

        }

        private void ModeClear(object Sender)
        {
            Mode = enMode.Empty;
        }

        private void _CloseOtherForms()
        {
            if (this.ActiveMdiChild != null)
                this.ActiveMdiChild.Close();
        }

        private void _CreateNewForm(enMode Mode)
        {
            
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Mode!=enMode.ManagePeople)
            {
                _CloseOtherForms();
                Mode = enMode.ManagePeople;

                ManagePeople Frm = new ManagePeople();
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();
            }
            
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mode != enMode.ManageUsers)
            {
                _CloseOtherForms();
                Mode = enMode.ManageUsers;

                ManageUsers Frm = new ManageUsers();
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();
            }
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsLogOut = true;
            this.Close();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Mode!=enMode.CurrentUserInfo)
            {
                _CloseOtherForms();

                Mode = enMode.CurrentUserInfo;
                UserInfo Frm = new UserInfo(CurrentUserID);
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();

            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mode != enMode.ChangePass)
            {
                _CloseOtherForms();

                Mode = enMode.ChangePass;
                ChangePassword Frm = new ChangePassword(CurrentUserID);
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();

            }
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Mode != enMode.ApplicationType)
            {
                _CloseOtherForms();

                Mode = enMode.ApplicationType;
                ManageApplicationTypes Frm = new ManageApplicationTypes();
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();
            }
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mode != enMode.TestType)
            {
                _CloseOtherForms();

                Mode = enMode.TestType;
                ListTestType Frm = new ListTestType();
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();
            }
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mode != enMode.NewLDLApp)
            {
                _CloseOtherForms();

                Mode = enMode.NewLDLApp;
                NewLocalDLApp Frm = new NewLocalDLApp(CurrentUserID);
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();
            }
        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Mode != enMode.ManageLDLApp)
            {
                _CloseOtherForms();

                Mode = enMode.ManageLDLApp;
                ManageLocalDrivingLicenseApp Frm = new ManageLocalDrivingLicenseApp(CurrentUserID);
                Frm.DataBack += ModeClear;
                Frm.MdiParent = this;
                Frm.Show();
            }
        }
    }
}
