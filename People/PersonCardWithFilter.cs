using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject
{
    public partial class PersonCardWithFilter : UserControl
    {

        public int PersonID = -1;
        public PersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void _ReturnedPersonID(object Sender,int ID)
        {
            personCard1.LoadPersonData(ID);
            this.PersonID = ID;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbxFilter.SelectedIndex==0)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void PersonCardWithFilter_Load(object sender, EventArgs e)
        {
            personCard1.LoadPersonData(-1);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (cbxFilter.SelectedIndex == 0)
                personCard1.LoadPersonData(Convert.ToInt32(txtFilter.Text));
            else if(cbxFilter.SelectedIndex==1)
                personCard1.LoadPersonData(txtFilter.Text);
            else
            {
                personCard1.LoadPersonData(-1);
                return;
            }

            this.PersonID = personCard1.GetID();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            AddEditPerson Person1 = new AddEditPerson(-1);
            Person1.DataBack += _ReturnedPersonID;
            Person1.ShowDialog();
        }
    }
}
