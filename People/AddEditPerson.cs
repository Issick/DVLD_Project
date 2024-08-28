using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using BussniessLayer;
using System.IO;

namespace DVLDProject
{
    public partial class AddEditPerson : Form
    {

        public delegate void DataBackEventHandler(object Sender, int ID);

        public DataBackEventHandler DataBack;
        enum enMode { AddNew=0, Update=1};

        struct stClassInfo
        {
            public int ID ;
            public enMode Mode ;
        };

        stClassInfo AddUpdateFormInfo = new stClassInfo();
        
        string ImagesPath = @"C:\Users\ASUS\source\repos\DVLDProject\DVLD People Images\";
        private void _ChooseMode(int ID)
        {
            if (ID == -1)
                this.AddUpdateFormInfo.Mode = enMode.AddNew;
            else
                this.AddUpdateFormInfo.Mode = enMode.Update;
        }

        private void _SetDefaultImage()
        {
            if (rbnMale.Checked)
                pbImage.Image = Properties.Resources.person_boy;
            else
                pbImage.Image = Properties.Resources.patient_female;
            pbImage.Tag = null;
        }

        public AddEditPerson(int ID)
        {
            InitializeComponent();
            this.AddUpdateFormInfo.ID = ID;
            _ChooseMode(ID);
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();
            foreach(DataRow Row in dtCountries.Rows)
            {
                cbCountries.Items.Add(Row["CountryName"]);
            }
        }

        private void _UploadAddNewInfo()
        {
            lblTitle.Text = "Add New Person";
            rbnMale.Checked = true;
            llRemove.Visible = false;
            cbCountries.SelectedIndex = cbCountries.FindString("Syria");
            pbImage.Image = Properties.Resources.person_boy;
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
        }

        private void _UploadPersonInfo(int ID)
        {
            clsPerson Person1 = clsPerson.Find(ID);
            lblTitle.Text = "Edit Person";
            lblPersonID.Text = Person1.ID.ToString();
            txtFirstName.Text = Person1.FirstName;
            txtSecondName.Text = Person1.SecondName;
            txtThirdName.Text = Person1.ThirdName;
            txtLastName.Text = Person1.LastName;
            txtNationalNo.Text = Person1.NationalNo;
            dtpDateOfBirth.Value = Person1.DateOfBirth;
            if (!Person1.Gendor)
                rbnMale.Checked = true;
            else
                rbnFemale.Checked = true;
            txtPhone.Text = Person1.Phone;
            txtEmail.Text = Person1.Email;
            cbCountries.SelectedIndex = cbCountries.FindString(clsCountry.Find(Person1.NationaltyCountryID).CountryName);
            txtAddress.Text = Person1.Address;
            if (Person1.ImagePath == "")
            {
                llRemove.Visible = false;
                if (!Person1.Gendor)
                    pbImage.Image = Properties.Resources.person_boy;
                else
                    pbImage.Image = Properties.Resources.patient_female;
            }
            else
            {
                using (FileStream stream = new FileStream(Person1.ImagePath, FileMode.Open, FileAccess.Read))
                {
                    pbImage.Image = Image.FromStream(stream);
                    stream.Dispose();
                }
                pbImage.Tag = false;
                //pbImage.Load(Person1.ImagePath);
            }
        }

        private void _LoadData()
        {

            _FillCountriesInComboBox();

            if (this.AddUpdateFormInfo.Mode == enMode.AddNew)
                _UploadAddNewInfo();
            else
                _UploadPersonInfo(this.AddUpdateFormInfo.ID);
        }

        private int _AddEditPersonInfo()
        {
            
            clsPerson Person1 = new clsPerson();

            if (this.AddUpdateFormInfo.Mode == enMode.Update)
            {
                Person1 = clsPerson.Find(this.AddUpdateFormInfo.ID);
            }


            Person1.FirstName = txtFirstName.Text;
            Person1.SecondName = txtSecondName.Text;
            Person1.ThirdName = txtThirdName.Text;
            Person1.LastName = txtLastName.Text;
            Person1.NationalNo = txtNationalNo.Text;
            Person1.DateOfBirth = dtpDateOfBirth.Value;
            if (rbnMale.Checked)
                Person1.Gendor = false;
            else
                Person1.Gendor = true;
            Person1.Phone = txtPhone.Text;
            Person1.Email = txtEmail.Text;
            Person1.NationaltyCountryID = clsCountry.Find(cbCountries.SelectedItem.ToString()).ID;
            Person1.Address = txtAddress.Text;
            if (!llRemove.Visible)
            {
                if(Person1.ImagePath!="")
                {
                    
                    File.Delete(Person1.ImagePath);
                }
                Person1.ImagePath = "";

            }
            else if((bool)pbImage.Tag)
            {
                try
                {
                    Guid PicName = Guid.NewGuid();
                    string NewImage = ImagesPath + PicName + ".png";
                    File.Copy(openFileDialog1.FileName, NewImage);


                    if (Person1.ImagePath != "")
                    {
                        //string Temp = Person1.ImagePath;
                        File.Delete(Person1.ImagePath);
                        Person1.ImagePath = NewImage;

                        using (FileStream stream = new FileStream(Person1.ImagePath, FileMode.Open, FileAccess.Read))
                        {
                            pbImage.Image = Image.FromStream(stream);
                            stream.Dispose();
                        }
                        //pbImage.Load(Person1.ImagePath);

                       
                        
                    }
                    else
                    {
                        Person1.ImagePath = NewImage;
                        pbImage.Tag = true;
                    }


                }
                catch
                {
                    MessageBox.Show("Old Image has not deleted", "Delete Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
 
            }
            if (Person1.Save())
            {
                if(this.AddUpdateFormInfo.Mode==enMode.AddNew)
                {
                    MessageBox.Show("Person has added successfully");
                    this.AddUpdateFormInfo.Mode = enMode.Update;
                }
                else
                {
                    MessageBox.Show("Person has updated successfully");
                }
                return Person1.ID;
            }
            else
            {
                if(this.AddUpdateFormInfo.Mode==enMode.AddNew)
                {
                    MessageBox.Show("Sorry, Person did not Add!");
                    return -1;
                }
                else
                {
                    MessageBox.Show("Sorry, Person did not update!");
                    return Person1.ID;
                }
                
            }

        }

        private void AddEditPerson_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void llSetPic_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                pbImage.Load(openFileDialog1.FileName);
                pbImage.Tag = true;
                llRemove.Visible = true;
                
            }
                
        }

        private void rbnFemale_CheckedChanged(object sender, EventArgs e)
        {
            if(rbnFemale.Checked && !llRemove.Visible)
            {
                _SetDefaultImage();
            }
        }

        private void rbnMale_CheckedChanged(object sender, EventArgs e)
        {
            if(rbnMale.Checked && !llRemove.Visible)
            {
                _SetDefaultImage();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.AddUpdateFormInfo.ID = _AddEditPersonInfo();
            _LoadData();
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _SetDefaultImage();
            llRemove.Visible = false;
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

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            _EmptyValidation((TextBox)sender,e, "First Name has to have a value");

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            _EmptyValidation((TextBox)sender, e, "National No has to have a value");

            if(clsPerson.isPersonExist(txtNationalNo.Text) && this.AddUpdateFormInfo.Mode==enMode.AddNew )
            {
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "This No is used!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(txtEmail.Text!="")
            {
                if (!txtEmail.Text.Contains("@"))
                {
                    e.Cancel = true;
                    txtEmail.Focus();
                    errorProvider1.SetError(txtEmail, "this invaled Email");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtEmail, "");
                }
            }
                
        }

        private void AddEditPerson_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this, this.AddUpdateFormInfo.ID);
        }

    }
}
