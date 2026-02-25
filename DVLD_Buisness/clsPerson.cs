using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }

        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
        }

        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public short Gender { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }

        public clsCountry CountryInfo;


        private string _ImagePath;
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        public clsPerson()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.NationalNo = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";
            this.Gender = 0;

            Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, short Gender,
            string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            this.FirstName = FirstName ?? string.Empty;
            this.SecondName = SecondName ?? string.Empty;
            this.ThirdName = ThirdName ?? string.Empty;
            this.LastName = LastName ?? string.Empty;
            this.NationalNo = NationalNo ?? string.Empty;
            this.Email = Email ?? string.Empty;
            this.Address = Address ?? string.Empty;
            this.Phone = Phone ?? string.Empty;
            this.ImagePath = ImagePath ?? string.Empty;

            this.PersonID = PersonID;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.NationalityCountryID = NationalityCountryID;

            this.CountryInfo = clsCountry.Find(NationalityCountryID);

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            ClsPersonData.stpersondata PersonStruct = new ClsPersonData.stpersondata();

            PersonStruct.Firstname = this.FirstName;
            PersonStruct.SecondName = this.SecondName;
            PersonStruct.ThirdName = this.ThirdName;
            PersonStruct.Lastname = this.LastName;
            PersonStruct.NationalNo = this.NationalNo;
            PersonStruct.DataOfBirth = this.DateOfBirth;
            PersonStruct.Gender = this.Gender;
            PersonStruct.Address = this.Address;
            PersonStruct.Phone = this.Phone;
            PersonStruct.Email = this.Email;
            PersonStruct.NationalityCountryID = this.NationalityCountryID;
            PersonStruct.ImagePath = this.ImagePath;

            this.PersonID = ClsPersonData.AddNewPerson(PersonStruct);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            ClsPersonData.stpersondata PersonStruct = new ClsPersonData.stpersondata();

            PersonStruct.PersonID = this.PersonID;
            PersonStruct.Firstname = this.FirstName;
            PersonStruct.SecondName = this.SecondName;
            PersonStruct.ThirdName = this.ThirdName;
            PersonStruct.Lastname = this.LastName;
            PersonStruct.NationalNo = this.NationalNo;
            PersonStruct.DataOfBirth = this.DateOfBirth;
            PersonStruct.Gender = this.Gender;
            PersonStruct.Address = this.Address;
            PersonStruct.Phone = this.Phone;
            PersonStruct.Email = this.Email;
            PersonStruct.NationalityCountryID = this.NationalityCountryID;
            PersonStruct.ImagePath = this.ImagePath;

            return ClsPersonData.UpdatePersonInfo(this.PersonID, PersonStruct);
        }

        public static clsPerson Find(int PersonID)
        {
            ClsPersonData.stpersondata PersonStruct = new ClsPersonData.stpersondata();

            if (ClsPersonData.GetPersonInfoByID(PersonID,ref PersonStruct))
            {
                return new clsPerson(PersonID, PersonStruct.Firstname, PersonStruct.SecondName,
                    PersonStruct.ThirdName, PersonStruct.Lastname, PersonStruct.NationalNo,
                    PersonStruct.DataOfBirth, PersonStruct.Gender, PersonStruct.Address,
                    PersonStruct.Phone, PersonStruct.Email, PersonStruct.NationalityCountryID,
                    PersonStruct.ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNo)
        {
            ClsPersonData.stpersondata PersonStruct = new ClsPersonData.stpersondata();

            if (ClsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonStruct))
            {
                return new clsPerson(PersonStruct.PersonID, PersonStruct.Firstname, PersonStruct.SecondName,
                    PersonStruct.ThirdName, PersonStruct.Lastname, PersonStruct.NationalNo,
                    PersonStruct.DataOfBirth, PersonStruct.Gender, PersonStruct.Address,
                    PersonStruct.Phone, PersonStruct.Email, PersonStruct.NationalityCountryID,
                    PersonStruct.ImagePath);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }

        public static DataTable GetAllPeople()
        {
            return ClsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int ID)
        {
            return ClsPersonData.DeletePerson(ID);
        }

        public static bool isPersonExist(int ID)
        {
            return ClsPersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationlNo)
        {
            return ClsPersonData.IsPersonExist(NationlNo);
        }

    }
}
