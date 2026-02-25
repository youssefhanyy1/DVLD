using DVLD_Business;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.people.Controls
{
    public partial class ctrPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int personId)
        {
            Action<int> Handler = OnPersonSelected;
            if (Handle!=null)
            {
                Handler(personId); 
            }
        }

        private bool _showAddPerson = true;
        public bool ShowAddPerson
        {
            get { return _showAddPerson; } 
            set {
                _showAddPerson = value; 
            btnAddNewPerson.Visible = _showAddPerson;
            }
        }
        private bool _FilterEnapled = true;

        public bool FilterEnapled
        {
            get { return _FilterEnapled; }
            set
            {
                _FilterEnapled = value;
                gbFilters.Visible = _FilterEnapled;
            }
        }
        private int _PersonID = -1;
        public int PersonID{
            get { return ctrPersonCard1.PersonID; } 
        }
        public clsPerson SelectedPersonInfo { get { return ctrPersonCard1.SelectedPersonInfo; } }
        public ctrPersonCardWithFilter()
        {
            InitializeComponent();
        }
        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text=PersonID.ToString();
            FindNow();
        }
        private void FindNow()
        {
            switch (cbFilterBy.Text) {
                case "Person ID" :
                    ctrPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    break;

                case "National No.":
                    ctrPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;
                    default:
                    break;
            }
            if (OnPersonSelected!=null && FilterEnapled)
            {
                OnPersonSelected(ctrPersonCard1.PersonID);
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //if (this.ValidateChildren()) {
            //MessageBox.Show("Please fix all validation errors before proceeding.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // return;
            //}
            FindNow();
        }

        private void ctrPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void ctrPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "This field is required.");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue,null);
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += DataBackEvent;
            frm.ShowDialog();
        }
        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received

            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrPersonCard1.LoadPersonInfo(PersonID);
        }
        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }

            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void gbFilters_Enter(object sender, EventArgs e)
        {

        }
    }
}
