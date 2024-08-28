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
    public partial class TestAppointmentsForm : Form
    {
        int CurrentUserID = -1;
        int LDLAppID = -1;
        public TestAppointmentsForm(int LDLAppID, int CurrentUserID)
        {
            InitializeComponent();
            this.LDLAppID = LDLAppID;
            this.CurrentUserID = CurrentUserID;
        }

        private void VisionTestAppointments_Load(object sender, EventArgs e)
        {
            testAppointments1.LoadInfo(this.LDLAppID, this.CurrentUserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
