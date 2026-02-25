using DVLD.Classes;
using DVLD_Buisness;
using DVLD_Business;
using fullProject.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fullProject.people.frmAddUpdatePerson;

namespace fullProject.people
{
    public partial class frmAddUpdatePerson : Form
    {

        public delegate void DataBackEventHandler(object sender,int personID);

        public event DataBackEventHandler DataBack;
        public enum enMode {addNew=0, Update = 1}
        public enum enGender {Male=0,Female=1 }

        private enMode _Mode;

        private int _personID = -1;
        clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.addNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Mode=enMode.Update;
            _personID = PersonID;
        }
        private void _FillCountriesInComoboBox()
        {
            DataTable DtCountries = clsCountry.GetAllCountries();
            foreach (DataRow row in DtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }

        }

        private void _ResetDefualtValues()
        {
            _FillCountriesInComoboBox();
            if (_Mode == enMode.addNew)
            {
                lbAddOrEdit.Text = "Add new Person";
                _Person = new clsPerson();
            }
            else
            {
                lbAddOrEdit.Text = "Update Person";
            }
            if (rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;

            }
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            cbCountry.SelectedIndex = cbCountry.FindString("egypt");
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            TxtNational.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            TxtAddress.Text = "";

        }
        private void _LoadData()
        {
            _Person=clsPerson.Find(_personID);
            if (_Person==null)
            {
                MessageBox.Show($"Person not found this person with ID: {_Person}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lbpersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            TxtNational.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            if (_Person.Gender==0)
            {
                rbMale.Checked = true;
            }
            else
            {
                rbFemale.Checked = true;
            }
            TxtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);
            if (_Person.ImagePath!="")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
            }
            llRemoveImage.Visible = (_Person.ImagePath!="");
        }

   
        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            if (_Mode == enMode.Update){
                _LoadData();
            }
        }

        private bool _HandlePerosonImage()
        {
            if (_Person.ImagePath!=pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to delete old image file. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (pbPersonImage.ImageLocation!=null)
            {
                string SourcePath = pbPersonImage.ImageLocation.ToString();
                if (clsUtil.CopyImageToProjectImagesFolder(ref SourcePath))
                {
                    pbPersonImage.ImageLocation = SourcePath;
                    return true;
                }
                else{
                    MessageBox.Show("Failed to copy image to project images folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please fix the errors and try again.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_HandlePerosonImage())
            {
                return;
            }
            int NationalCountryID=clsCountry.Find(cbCountry.Text).CountryID;


            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = TxtNational.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = TxtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
                _Person.Gender = (short)enGender.Male;
            else
                _Person.Gender = (short)enGender.Female;

            _Person.NationalityCountryID = NationalCountryID;
            if (pbPersonImage.ImageLocation != null)
            {
                _Person.ImagePath = pbPersonImage.ImageLocation;
            }
            else
            {
                _Person.ImagePath = "";
            }
            if (_Person.Save())
            {
                lbpersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.Update;
                lbAddOrEdit.Text = "Update Person";
                MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Failed to save data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim()=="")
            {
                return;
            }
            if (!clsValidatoin.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid email format!");

            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }

        private void TxtNational_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtNational.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(TxtNational, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(TxtNational, null);
            }
            if (TxtNational.Text.Trim()==_Person.NationalNo && clsPerson.isPersonExist(TxtNational.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(TxtNational, "This National No is Used for anthoer person!");
            }
            else{
                errorProvider1.SetError(TxtNational, null);
            }
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation==null)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation =null;
            if (rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }
            llRemoveImage.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
