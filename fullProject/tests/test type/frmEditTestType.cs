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

namespace fullProject.tests.test_type
{
    public partial class frmEditTestType : Form
    {
        private clsTestType.enTestType _TestTypeID=clsTestType.enTestType.VisionTest;
        private clsTestType _TestType; 
        public frmEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);
            if (_TestType!=null)
            {
                lblTestTypeID.Text = ((int)_TestTypeID).ToString();
                txtTitle.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = _TestType.Fees.ToString("0.00");
            }
            else
            {
                MessageBox.Show("Error: Test Type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
                errorProvider1.SetError(txtTitle, "Title is required.");
            }else
            {
                errorProvider1.SetError(txtTitle,null);
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;    
                errorProvider1.SetError(txtDescription, "Description is required.");
            }
            else
            {
                errorProvider1.SetError(txtDescription, null);
            }
            
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtFees, "Fees is required.");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
            if (!clsValidatoin.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees must be a valid number.");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please correct the errors before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _TestType.Title = txtTitle.Text.Trim();
            _TestType.Description = txtDescription.Text.Trim();
            _TestType.Fees = Convert.ToSingle(txtFees.Text.Trim());

            if (_TestType.Save())
            {
                MessageBox.Show("Test Type updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Error: Unable to update Test Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
