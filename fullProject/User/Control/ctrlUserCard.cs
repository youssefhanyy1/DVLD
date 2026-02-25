using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.User.Control
{
    public partial class ctrlUserCard : UserControl
    {
         private int _UserID=-1;
        public clsUser _User;
        public int UserID
        {
            get { return _UserID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }
        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindByUserID(UserID);
            if (_User == null)
            {
                _ResetPersonInfo();
                MessageBox.Show($"User not found with ID:{UserID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillUserInfo();
        }
        private void _FillUserInfo()
        {
            ctrPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();
            if (_User.IsActive)
            {
                lblIsActive.Text = "Active";
                lblIsActive.ForeColor = Color.Green;
            }
            else
            {
                lblIsActive.Text = "Inactive";
                lblIsActive.ForeColor = Color.Red;
            }
        }
        private void _ResetPersonInfo()
        {
            ctrPersonCard1.ResetPersonInfo();
            lblUserID.Text = string.Empty;
            lblUserName.Text = string.Empty;
            lblIsActive.Text = string.Empty;
        }

        private void ctrPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void lblIsActive_Click(object sender, EventArgs e)
        {

        }
    }
  
}
