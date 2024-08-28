﻿using System;
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
    public partial class UserInfo : Form
    {
        public delegate void DataBackEventHandler(object Sender);

        public DataBackEventHandler DataBack;

        int UserID = -1;
        public UserInfo(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserInfo_Load(object sender, EventArgs e)
        {
            userCard1.LoadUserData(this.UserID);
        }

        private void UserInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBack?.Invoke(this);
        }
    }
}
