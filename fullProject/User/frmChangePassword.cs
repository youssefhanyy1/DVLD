using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.User
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _resetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _resetDefaultValues();
            _User = clsUser.FindByUserID(_UserID);
            if (_User==null)
            {
                MessageBox.Show($"User not found with ID {_UserID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Please enter current password.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            }
            if (_User.Password==txtCurrentPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is incorrect.");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Please enter new password.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim()!=txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirm password does not match new password.");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please correct the errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _User.Password = txtNewPassword.Text.Trim();
            if (_User.Save())
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _resetDefaultValues();
            }
            else
            {
                MessageBox.Show("Failed to change password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCurrentPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
