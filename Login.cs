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
    public partial class Login : Form
    {

        string Username = "";
        string Password = "";
        private void _RememberMe()
        {
            if(chxRememberMe.Checked)
            {
                Username = txtUsername.Text;
                Password = txtPassword.Text;
            }
            else
            {
                Username = "";
                Password = "";
            }
        }
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (clsUser.isUserExist(txtUsername.Text, txtPassword.Text))
            {
                clsUser User1 = clsUser.Find(txtUsername.Text);

                if (User1.isActive)
                {
                    _RememberMe();
                    Main main = new Main(User1.ID);
                    this.Visible = false;
                    main.ShowDialog();
                    if (!main.IsLogOut)
                        Application.Exit();
                    txtUsername.Text = Username;
                    txtPassword.Text = Password;
                    this.Visible = true;
                }
                else
                    MessageBox.Show("Your account is deactivated, Please contact your admin", "Unactive Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                

            }
            else
                MessageBox.Show("Invalid Usernme/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsername.Text = Username;
            txtPassword.Text = Password;
        }
    }
}
