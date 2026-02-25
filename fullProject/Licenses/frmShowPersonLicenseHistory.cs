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

namespace fullProject.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _PersonID = -1;
        public frmShowPersonLicenseHistory(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {

            if (_PersonID != -1)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnapled = false;
                ctrlDriverLicenses1.LoadInfoPersonID(_PersonID);
            }
            else
            {
                ctrlPersonCardWithFilter1.Enabled = true;
                ctrlPersonCardWithFilter1.FilterFocus();
            }

        }

        private void ctrPersonCardWithFilter1_OnPersonSelected(int PersonID)
        {
            _PersonID= PersonID;
            if (_PersonID == -1)
            {
                ctrlDriverLicenses1.Focus();
            }
            else
            {
                ctrlDriverLicenses1.LoadInfoPersonID(PersonID);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
