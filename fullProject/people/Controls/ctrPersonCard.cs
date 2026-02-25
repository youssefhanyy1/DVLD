using DVLD_Buisness;
using DVLD_Business;
using fullProject.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fullProject.people.Controls
{
    public partial class ctrPersonCard : UserControl
    {
        private int _PersonID;
        private clsPerson _Person;

        public int PersonID
        {
            get { return _PersonID; }
        }
        public clsPerson Person
        {
            get
            {
                return _Person;
            }
        }
        public void LoadPersonInfo(int Person)
        {
            _Person = clsPerson.Find(Person);
            if (_Person == null)
            {
                MessageBox.Show($"No person with PersonID :{Person}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNO)
        {
            _Person = clsPerson.Find(NationalNO);
            if (_Person == null)
            {
                MessageBox.Show($"No person with NationalNo :{NationalNO}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }
        public ctrPersonCard()
        {
            InitializeComponent();
        }
        private void _LoadPersonImage()
        {
            if (_Person.Gender == 0)
            {
                pbPersonImage.Image = Resources.Male_512;

            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;

            }
            string ImagePath = _Person.ImagePath;
            if (ImagePath != null)
            {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.ImageLocation = ImagePath;

                }
                else
                {
                    //MessageBox.Show($"Could not find this Image: = {ImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Visible = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo ?? string.Empty;
            lblFullName.Text = _Person.FullName ?? string.Empty;
            lblGendor.Text = _Person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email ?? string.Empty;
            lblPhone.Text = _Person.Phone ?? string.Empty;
            lblDateOfBirth.Text = _Person.DateOfBirth == DateTime.MinValue ? "" : _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID)?.CountryName ?? string.Empty;
            lblAddress.Text = _Person.Address ?? string.Empty;
            _LoadPersonImage();
        }
        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";
            pictureBox4.Image = Resources.Man_32;
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;

        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
