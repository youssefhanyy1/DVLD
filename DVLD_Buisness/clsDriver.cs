using DVLD_Business;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsDriver
    {
        public enum enMode {AddNew=0,Update=1 };
        public enMode Mode = enMode.AddNew;
        public clsPerson PersonInfo;
        public int DriverID {  get; set; }
        public int PersonID {  get; set; }
        public int CreatedByUserID {  get; set; }
        public DateTime CreatedDate {  get; set; }


        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;
            Mode = enMode.AddNew;
        }

        public clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            this.PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriver(PersonID, CreatedByUserID);
            return (this.DriverID != -1);

        }
        private bool _UpdateDriver()
        {
            clsDriverData.stDriverInfo DriverInfo = new clsDriverData.stDriverInfo();

            DriverInfo.DriverID = this.DriverID;
            DriverInfo.PersonID = this.PersonID;
            DriverInfo.CreatedByUserID = this.CreatedByUserID;

            return clsDriverData.UpdateDriver(DriverInfo);
        }
        public static clsDriver FindByDriverID(int DriverID)
        {
            clsDriverData.stDriverInfo DriverInfo = new clsDriverData.stDriverInfo();

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref DriverInfo))
            {
                return new clsDriver(DriverInfo.DriverID,
                                     DriverInfo.PersonID,
                                     DriverInfo.CreatedByUserID,
                                     DriverInfo.CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            clsDriverData.stDriverInfo DriverInfo = new clsDriverData.stDriverInfo();

            if (clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverInfo))
            {
                return new clsDriver(DriverInfo.DriverID,
                                     DriverInfo.PersonID,
                                     DriverInfo.CreatedByUserID,
                                     DriverInfo.CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateDriver();
            }

            return false;
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        //public static DataTable GetInternationalLicenses(int DriverID)
        //{
        //    return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        //}
    }
}
