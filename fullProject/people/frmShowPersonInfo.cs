using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.people
{
    public partial class frmShowPersonInfo : Form
    {
        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();
            ctrPersonCard1.LoadPersonInfo(personID);
        }
        public frmShowPersonInfo(string NationalNo)
        {
            InitializeComponent();
            ctrPersonCard1.LoadPersonInfo(NationalNo);
        }

        private void frmShowPersonInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
