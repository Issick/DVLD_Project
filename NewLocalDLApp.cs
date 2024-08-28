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
    public partial class NewLocalDLApp : Form
    {
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;

        enum enMode { AddNew, Update };

        enMode Mode = enMode.AddNew;

        int CurrentUserID = -1;

        bool AllowNextTab = false;

        int ExistAppID = -1;
        public NewLocalDLApp(int CurrentUserID)
        {
            InitializeComponent();
            this.CurrentUserID = CurrentUserID;
        }

        private void _CheckMode()
        {

        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtL_Classes = clsLicenseClass.GetAllLicenseClasses();
            foreach (DataRow Row in dtL_Classes.Rows)
            {
                comboBox1.Items.Add(Row["ClassName"]);
            }
        }

        private void _LoadDefaultInfo()
        {
            lblAppDate.Text = DateTime.Today.ToShortDateString();
            _FillLicenseClassesInComboBox();
            lblAppFee.Text = "15";
            lblUsername.Text = clsUser.Find(CurrentUserID).UserName.ToString();

        }

        private void _AllowRejectCreatApplication()
        {
            if (personCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Person did not definded!", "Person Needed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
            else
            {
                AllowNextTab = true;
                tabControl1.SelectedIndex = 1;
                _LoadDefaultInfo();
            }
        }

        private bool _IsAppExsitAndNew()
        {
            clsLicenseClass LicClass1 = clsLicenseClass.Find(comboBox1.SelectedItem.ToString());
            DataTable AppIDs = clsLDLApp.GetAllAppIDsForLicenseClass(LicClass1.ID);

            foreach(DataRow Row in AppIDs.Rows)
            {
                //in this case we prupose that AppStatus 'New' equal 1
                if (clsLDLApp.IsAppExistAndNotNew((int)Row[0], LicClass1.ID, 1))
                {
                    this.ExistAppID = (int)Row[0];
                    return true;
                }
            }
            return false;
        }

        private bool _IsAppExsitAndComplete()
        {
            clsLicenseClass LicClass1 = clsLicenseClass.Find(comboBox1.SelectedItem.ToString());
            DataTable AppIDs = clsLDLApp.GetAllAppIDsForLicenseClass(LicClass1.ID);

            foreach (DataRow Row in AppIDs.Rows)
            {
                //in this case we prupose that AppStatus 'Complete' equal 3
                if (clsLDLApp.IsAppExistAndNotNew((int)Row[0], LicClass1.ID, 3))
                {
                    this.ExistAppID = (int)Row[0];
                    return true;
                }
            }
            return false;
        }

        private void _AddNewApplication()
        {
            clsApplication App1 = new clsApplication();

            App1.AppPersonID = personCardWithFilter1.PersonID;
            App1.AppDateStart = DateTime.Today;
            App1.AppTypeID = clsApplicationType.Find("New Local Driving License Service").ID;
            // 1 As New Status
            App1.AppStatus = 1;
            App1.LastStatusDate = DateTime.Today;
            App1.PaidFees = clsApplicationType.Find(App1.AppTypeID).ApplicationFee;
            App1.CreatedUserID = this.CurrentUserID;

            if (App1.AddNewApp())
            {
                MessageBox.Show("New Application Added successfully", "Application Adding", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                clsLDLApp LDLApp1 = new clsLDLApp();
                LDLApp1.ApplicationID = App1.ID;
                LDLApp1.LicenseClassID = clsLicenseClass.Find(comboBox1.SelectedItem.ToString()).ID;
                LDLApp1.Save();
                lblAppID.Text = App1.ID.ToString();
                Mode = enMode.Update;
            }
            else
            {
                MessageBox.Show("New Application had faild to Add!", "Application Adding", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

                

        }

        private void _ActiveApplicationIDMessage()
        {
            string Message = string.Format("Could Not Add an Application, this Application is already Exist with ID {0}", this.ExistAppID);
            MessageBox.Show(Message, "Exist Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _CompleteApplicationIDMessage()
        {
            
            MessageBox.Show("Person already has a License with the same applied driving class," +
                " Choose different driving class", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _AllowRejectCreatApplication();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!AllowNextTab)
                tabControl1.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem==null)
                MessageBox.Show("Please choose a License Class", "License Class is missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (_IsAppExsitAndNew())
                    _ActiveApplicationIDMessage();
                else if (_IsAppExsitAndComplete())
                    _CompleteApplicationIDMessage();
                else
                    _AddNewApplication();
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewLocalDLApp_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
