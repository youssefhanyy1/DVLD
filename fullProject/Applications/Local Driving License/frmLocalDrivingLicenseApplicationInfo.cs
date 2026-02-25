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
    public partial class frmLocalDrivingLicenseApplicationInfo : Form
    {
        private int _ApplicationID = -1;
        public frmLocalDrivingLicenseApplicationInfo(int ApplicactionID)
        {
            InitializeComponent();
            _ApplicationID = ApplicactionID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_ApplicationID);

        }

        private void ctrlDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
