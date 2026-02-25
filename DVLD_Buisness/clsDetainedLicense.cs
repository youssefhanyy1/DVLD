using System;
using System.ComponentModel;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business 
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }
        public int ReleaseApplicationID { set; get; }

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1; 
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        public clsDetainedLicense(int DetainID,
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID,
            bool IsReleased, DateTime ReleaseDate,
            int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;

            if (this.ReleasedByUserID != -1)
                this.ReleasedByUserInfo = clsUser.FindByUserID(this.ReleasedByUserID);
            else
                this.ReleasedByUserInfo = null;

            Mode = enMode.Update;
        }

        private bool _AddNewDetainedLicense()
        {
            clsDetainedLicenseData.stDetainedLicenseInfo DetainedLicenseInfo = new clsDetainedLicenseData.stDetainedLicenseInfo();

            DetainedLicenseInfo.LicenseID = this.LicenseID;
            DetainedLicenseInfo.DetainDate = this.DetainDate;
            DetainedLicenseInfo.FineFees = this.FineFees;
            DetainedLicenseInfo.CreatedByUserID = this.CreatedByUserID;

            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(DetainedLicenseInfo);

            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            clsDetainedLicenseData.stDetainedLicenseInfo DetainedLicenseInfo = new clsDetainedLicenseData.stDetainedLicenseInfo();

            DetainedLicenseInfo.DetainID = this.DetainID;
            DetainedLicenseInfo.LicenseID = this.LicenseID;
            DetainedLicenseInfo.DetainDate = this.DetainDate;
            DetainedLicenseInfo.FineFees = this.FineFees;
            DetainedLicenseInfo.CreatedByUserID = this.CreatedByUserID;

            return clsDetainedLicenseData.UpdateDetainedLicense(DetainedLicenseInfo);
        }

        public static clsDetainedLicense Find(int DetainID)
        {
            clsDetainedLicenseData.stDetainedLicenseInfo DetainedLicenseInfo = new clsDetainedLicenseData.stDetainedLicenseInfo();

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID, ref DetainedLicenseInfo))
            {
                return new clsDetainedLicense(
                    DetainedLicenseInfo.DetainID,
                    DetainedLicenseInfo.LicenseID,
                    DetainedLicenseInfo.DetainDate,
                    DetainedLicenseInfo.FineFees,
                    DetainedLicenseInfo.CreatedByUserID,
                    DetainedLicenseInfo.IsReleased,
                    DetainedLicenseInfo.ReleaseDate,
                    DetainedLicenseInfo.ReleasedByUserID,
                    DetainedLicenseInfo.ReleaseApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            clsDetainedLicenseData.stDetainedLicenseInfo DetainedLicenseInfo = new clsDetainedLicenseData.stDetainedLicenseInfo();

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID, ref DetainedLicenseInfo))
            {
                return new clsDetainedLicense(
                    DetainedLicenseInfo.DetainID,
                    DetainedLicenseInfo.LicenseID,
                    DetainedLicenseInfo.DetainDate,
                    DetainedLicenseInfo.FineFees,
                    DetainedLicenseInfo.CreatedByUserID,
                    DetainedLicenseInfo.IsReleased,
                    DetainedLicenseInfo.ReleaseDate,
                    DetainedLicenseInfo.ReleasedByUserID,
                    DetainedLicenseInfo.ReleaseApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicense();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateDetainedLicense();
            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            clsDetainedLicenseData.stDetainedLicenseInfo DetainedLicenseInfo = new clsDetainedLicenseData.stDetainedLicenseInfo();

            DetainedLicenseInfo.DetainID = this.DetainID;
            DetainedLicenseInfo.ReleasedByUserID = ReleasedByUserID;
            DetainedLicenseInfo.ReleaseApplicationID = ReleaseApplicationID;

            return clsDetainedLicenseData.ReleaseDetainedLicense(DetainedLicenseInfo);
        }
    }
}