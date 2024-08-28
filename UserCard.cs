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

namespace DVLDProject
{
    public partial class UserCard : UserControl
    {
        int UserID = -1;
        int PersonID = -1;
        public UserCard()
        {
            InitializeComponent();
        }

        private void _LoadDefaultData()
        {
            personCard1.LoadPersonData(PersonID);
            lblUserID.Text = "[????]";
            lblUsername.Text = "[????]";
            lblisActive.Text = "[????]";
        }

        private void _RefreshInfo(clsUser User)
        {
            personCard1.LoadPersonData(PersonID);
            lblUserID.Text = User.ID.ToString();
            lblUsername.Text = User.UserName;
            lblisActive.Text = (User.isActive) ? "Yes" : "No";
        }

        public int GetUserID()
        {
            return UserID;
        }


        public void LoadUserData(int ID)
        {
            clsUser User1 = clsUser.Find(ID);

            if (User1 != null)
            {
                this.UserID = User1.ID;
                this.PersonID = User1.PersonID;
                _RefreshInfo(User1);
            }
            else
            {
                _LoadDefaultData();
            }

        }

    }
}
