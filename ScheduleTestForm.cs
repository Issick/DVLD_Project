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
    public partial class ScheduleTestForm : Form
    {
        int UserID = -1;
        int LDLAppID = -1;
        int dgvRecords = 0;
        int AppointmentID = -1;
        public ScheduleTestForm(int LDLAppID, int CurrentUserID, int dgvRecords, int AppointmentID=-1)
        {
            InitializeComponent();

            this.LDLAppID = LDLAppID;
            this.UserID = CurrentUserID;
            this.dgvRecords = dgvRecords;
            this.AppointmentID = AppointmentID;
        }

        private void ScheduleTestForm_Load(object sender, EventArgs e)
        {
            scheduleTest1.LoadInfo(this.LDLAppID, this.dgvRecords, this.UserID, this.AppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
