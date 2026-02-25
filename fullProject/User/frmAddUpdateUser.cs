using DVLD_Business;
using fullProject.people.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.User
{
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode ;
        private int _UserID = -1;
        clsUser _User;

        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
        }
        private void _ResetDefaultVaules()
        {
            if (_Mode==enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
                tpLoginInfo.Enabled = false;
                ctrPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled= true;
                btnClose.Enabled = true;

            }
        }
        private void _LoadData()
        {
            _User=clsUser.FindByUserID(_UserID);
            ctrPersonCardWithFilter1.FilterEnapled = false;
            if (_User==null)
            {
                MessageBox.Show($"User not found with ID: {_User}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            ctrPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultVaules();
            if (_Mode==enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }
            if (ctrPersonCardWithFilter1.PersonID!=-1)
            {
                if (clsUser.isUserExistForPersonID(ctrPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("A user is already associated with the selected person. Please select a different person.", "Duplicate User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ctrPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }

            }
            else
            { 
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrPersonCardWithFilter1.FilterFocus();

            }
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrPersonCardWithFilter1.FilterFocus();

        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "User Name is required.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }
            if (_Mode==enMode.AddNew)
            {
                if (clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "User Name already exists. Please choose a different User Name.");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);

                }
            }
            else
            {
                if (_User.UserName!=txtUserName.Text.Trim() && clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "User Name already exists. Please choose a different User Name.");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtPassword,"Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Passwords do not match.");
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
                MessageBox.Show("Please correct the validation errors before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.PersonID = ctrPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("User information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
