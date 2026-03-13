using DVLD.Classes;
using DVLD_Business;
using fullProject.Global_Classes;
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
            string username = txtUserName.Text.Trim();

            string hashedPassword = clsUtil.ComputeHash(txtPassword.Text.Trim());

            clsUser user = clsUser.FindByUsernameAndPassword(username, hashedPassword);

            if (user != null)
            {
                if (!user.IsActive)
                {
                    MessageBox.Show("Your account is InActive, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = user;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.Show();
            }
            else
            {
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (clsStoreInRegistry.GetStoredCredential(out UserName, out Password))
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