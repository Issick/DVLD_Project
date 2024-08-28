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
    public partial class LicenseInfo : Form
    {
        int AppID = -1;
        public LicenseInfo(int AppID)
        {
            InitializeComponent();
            this.AppID = AppID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LicenseInfo_Load(object sender, EventArgs e)
        {
            driverInfo1.LoadLicenseData(this.AppID);
        }
    }
}
