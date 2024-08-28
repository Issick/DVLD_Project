using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussniessLayer;
using System.IO;

namespace DVLDProject
{
    public partial class PersonCard : UserControl
    {
        int ID = -1;
        public PersonCard()
        {
            InitializeComponent();
        }

        private void _LoadDefaultData()
        {
            lblPersonID.Text = "[????]";
            lblName.Text = "[????]";
            lblNantionalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblAdress.Text = "[????]";
            lblEmail.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            llEdit.Enabled = false;
            pbPersonalImage.Image = Properties.Resources.person_boy;
            this.ID = -1;
        }
        private void _RefreshInfo()
        {
            
            clsPerson Person1 = clsPerson.Find(this.ID);

            llEdit.Enabled = true;
            lblPersonID.Text = Person1.ID.ToString();
            lblName.Text = Person1.FullName();
            lblNantionalNo.Text = Person1.NationalNo;
            if (Person1.Gendor)
                lblGendor.Text = "Female";
            else
                lblGendor.Text = "Male";
            lblEmail.Text = Person1.Email;
            lblAdress.Text = Person1.Address;
            lblDateOfBirth.Text = Person1.DateOfBirth.ToShortDateString();
            lblPhone.Text = Person1.Phone;
            lblCountry.Text = clsCountry.Find(Person1.NationaltyCountryID).CountryName;
            if (Person1.ImagePath != "")
            {
                using (FileStream stream = new FileStream(Person1.ImagePath, FileMode.Open, FileAccess.Read))
                {
                    pbPersonalImage.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
            }
            else if (Person1.Gendor)
                pbPersonalImage.Image = Properties.Resources.patient_female;
            else
                pbPersonalImage.Image = Properties.Resources.person_boy;
        }
        public void LoadPersonData(int ID)
        {
            clsPerson person1 = clsPerson.Find(ID);
            if(person1!=null)
            {
                this.ID = person1.ID;
                _RefreshInfo();
            }
            else
            {
                _LoadDefaultData();
            }
            
        }
        public int GetID()
        {
            return this.ID;
        }
        public void LoadPersonData(string NationalNo)
        {
            clsPerson person1= clsPerson.Find(NationalNo);

            if(person1!=null)
            {
                this.ID = person1.ID;
                _RefreshInfo();
            }
            else
            {
                _LoadDefaultData();
            }
            
        }
        private void llEdit_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddEditPerson Person1 = new AddEditPerson(this.ID);
            Person1.ShowDialog();
            _RefreshInfo();
        }
    }
}
