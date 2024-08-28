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
    public partial class PersonDetail : Form
    {
        int ID = -1;
        public PersonDetail(int ID)
        {
            InitializeComponent();
            this.ID = ID;
        }

        private void PersonDetail_Load(object sender, EventArgs e)
        {
            PersonCard1.LoadPersonData(this.ID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
