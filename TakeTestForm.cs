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
    public partial class TakeTestForm : Form
    {

        int LDAppID = -1;
        int PreTestCount = 0;
        int UserID = -1;
        int AppointmentID = -1;
        public TakeTestForm(int LDAppID, int PreTestCount, int UserID, int AppointmentID)
        {
            InitializeComponent();
            this.LDAppID = LDAppID;
            this.PreTestCount = PreTestCount;
            this.UserID = UserID;
            this.AppointmentID = AppointmentID;
        }

        private void TakeTestForm_Load(object sender, EventArgs e)
        {
            takeTestControl1.LoadInfo(this.LDAppID, this.PreTestCount, this.UserID, this.AppointmentID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(takeTestControl1.Save())
            {
                
                this.Close();
            }
            else
            {

            }
            
        }

    }
}
