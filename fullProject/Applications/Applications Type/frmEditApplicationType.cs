using DVLD.Classes;
using DVLD_Buisness; 
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace fullProject.Applications.Applications_Type
{
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationID = -1;
        private clsApplicationType _ApplicationType;

        public frmEditApplicationType(int ApplicationID)
        {
            InitializeComponent();
            _ApplicationID = ApplicationID;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblApplicationTypeID.Text = _ApplicationID.ToString();
            _ApplicationType = clsApplicationType.Find(_ApplicationID);

            if (_ApplicationType != null)
            {
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren()) 
            {
                MessageBox.Show("Some fields are not valid! Put the mouse over the red icon(s)", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ApplicationType.Title = txtTitle.Text.Trim();

            if (float.TryParse(txtFees.Text.Trim(), out float fees))
            {
                _ApplicationType.Fees = fees;
            }
            else
            {
                return;
            }

            if (_ApplicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title is required");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees is required");
            }
            else if (!clsValidatoin.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees must be a number");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }
    }
}