using DVLD.Classes;
using DVLD_Buisness;
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

namespace fullProject.Applications.Local_Driving_License
{
    public partial class frmAddUpdataLocalDrivingLicense : Form
    {
        public enum enMode { AddNew = 0, Updata = 1 };
        private enMode _Mode;
        private int _SelectPersonID = -1;
        private int _LocalDrivingLicenseApplicationID = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmAddUpdataLocalDrivingLicense()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdataLocalDrivingLicense(int LocalDrivingLicenseApplication)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplication;
            _Mode = enMode.Updata;
        }
        private void _FillLicenseClassesInComoboBox()
        {
          DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();
            foreach (DataRow row in dtLicenseClasses.Rows)
           {
              cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComoboBox();
            if (_Mode == enMode.AddNew)
            {

                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                //cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;

            }
        }
        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnapled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error loading Local Driving License Application data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;
        }
        private void frmAddUpdataLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode == enMode.Updata)
            {
                _LoadData();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Updata)
            {
                btnSave.Enabled = true;
                tcApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }
            else
            {
                MessageBox.Show("Please select applicant person.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please wait saving data...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
              int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;
             int ActiveApplicantPersonID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);
           if (ActiveApplicantPersonID != -1)
           {
              MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicantPersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               cbLicenseClass.Focus();
             return;
           }
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID; ;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;


            if (_LocalDrivingLicenseApplication.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Updata;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int PersonID)
        {
            _SelectPersonID = PersonID;
        }
        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void cbLicenseClass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
