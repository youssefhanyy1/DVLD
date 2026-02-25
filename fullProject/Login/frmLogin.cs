using DVLD.Classes;
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

namespace fullProject.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User= clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
            if (User!=null)
            {
                if (chkRememberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                }
                else
                {
                    clsGlobal.RememberUsernameAndPassword("", "");
                }
                if (!User.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("User is InActive. Please contact system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsGlobal.CurrentUser = User;
                this.Hide();
                frmMain frm =new frmMain(this);
                frm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid UserName or Password","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";
            if (clsGlobal.GetStoredCredential(ref UserName,ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
            {
                chkRememberMe.Checked = false;
            }
        }
    }
}
