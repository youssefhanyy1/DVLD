using DVLD.Classes;
using DVLD_Business;
using fullProject.Applications.Applications_Type;
using fullProject.Applications.International_License;
using fullProject.Applications.Local_Driving_License;
using fullProject.Applications.Renew_Local_License;
using fullProject.Applications.ReplaceLostOrDamagedLicense;
using fullProject.Applications.Rlease_Detained_License;
using fullProject.Drivers;
using fullProject.Licenses.Detain_License;
using fullProject.Login;
using fullProject.people;
using fullProject.tests.test_type;
using fullProject.User;
using System;
using System.Windows.Forms;

namespace fullProject
{
    public partial class frmMain : Form
    {
        frmLogin loginForm;
        public frmMain(frmLogin frm)
        {
            InitializeComponent();
            loginForm = frm;
        }

        private void frmMain_Load(object sender, System.EventArgs e)
        {
           

           
        }

        private void mangeApplicationsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {

        }

        private void toolStripMenuItem5_Click(object sender, System.EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, System.EventArgs e)
        {

        }

        private void peopleToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Form frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListUsers();
            frm.ShowDialog();
        }

        private void sighOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            loginForm.Show();
            this.Close();
        }

        private void accountSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mangeApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm= new frmListTestTypes();
            frm.ShowDialog();   
        }

        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdataLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmRenewLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReplaceLostOrDamagedLicenseApplication();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDrivers();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainLicenseApplication();
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDetainedLicenses();
            frm.ShowDialog();   
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListInternationalLicesnseApplications();
            frm.ShowDialog();
        }
    }
}
