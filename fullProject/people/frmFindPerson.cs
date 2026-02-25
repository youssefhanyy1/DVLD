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
    public partial class frmFindPerson : Form
    {
        public delegate void DataBackEventHandler(object sender,int person);
        public event DataBackEventHandler DataBack;
        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void frmFindPerson_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DataBack?.Invoke(this, ctrPersonCardWithFilter1.PersonID);
        }
    }
}
