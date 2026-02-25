using DVLD_Business;
using fullProject.Applications.Applications_Type;
using fullProject.Applications.Local_Driving_License;
using fullProject.Login;
using fullProject.people;
using fullProject.tests;
using fullProject.tests.test_type;
using fullProject.User;
using System;
using System.Windows.Forms;

namespace fullProject
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

          Application.Run(new frmLogin());

            //Application.Run(new frmMain());
            //Application.Run(new frmAddUpdatePerson());
            //Application.Run(new frmListPeople());
            //Application.Run(new frmChangePassword(1034));
            //Application.Run(new frmListUsers());

            //Application.Run(new frmManageApplicationTypes());
            // Application.Run(new frmListTestTypes());   
            // Application.Run(new frmAddUpdataLocalDrivingLicense());
            //Application.Run(new frmTakeTest(108,clsTestType.enTestType.VisionTest ));

        }
    }
}
