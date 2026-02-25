using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business     
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsDriver DriverInfo;
        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }

        public clsInternationalLicense()
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;

            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;

            Mode = enMode.AddNew;
        }

        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID,
             int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive)
        {
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);

            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            clsInternationalLicenseData.stInternationalLicenseInfo LicenseInfo = new clsInternationalLicenseData.stInternationalLicenseInfo();

            LicenseInfo.ApplicationID = this.ApplicationID;
            LicenseInfo.DriverID = this.DriverID;
            LicenseInfo.IssuedUsingLocalLicenseID = this.IssuedUsingLocalLicenseID;
            LicenseInfo.IssueDate = this.IssueDate;
            LicenseInfo.ExpirationDate = this.ExpirationDate;
            LicenseInfo.IsActive = this.IsActive;
            LicenseInfo.CreatedByUserID = this.CreatedByUserID;

            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(LicenseInfo);

            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            clsInternationalLicenseData.stInternationalLicenseInfo LicenseInfo = new clsInternationalLicenseData.stInternationalLicenseInfo();

            LicenseInfo.InternationalLicenseID = this.InternationalLicenseID;
            LicenseInfo.ApplicationID = this.ApplicationID;
            LicenseInfo.DriverID = this.DriverID;
            LicenseInfo.IssuedUsingLocalLicenseID = this.IssuedUsingLocalLicenseID;
            LicenseInfo.IssueDate = this.IssueDate;
            LicenseInfo.ExpirationDate = this.ExpirationDate;
            LicenseInfo.IsActive = this.IsActive;
            LicenseInfo.CreatedByUserID = this.CreatedByUserID;

            return clsInternationalLicenseData.UpdateInternationalLicense(LicenseInfo);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            clsInternationalLicenseData.stInternationalLicenseInfo LicenseInfo = new clsInternationalLicenseData.stInternationalLicenseInfo();

            if (clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref LicenseInfo))
            {
                clsApplication Application = clsApplication.FindBaseApplication(LicenseInfo.ApplicationID);

                if (Application == null)
                    return null;

                return new clsInternationalLicense(
                    Application.ApplicationID,
                    Application.ApplicantPersonID,
                    Application.ApplicationDate,
                    (enApplicationStatus)Application.ApplicationStatus,
                    Application.LastStatusDate,
                    Application.PaidFees,
                    Application.CreatedByUserID,
                    LicenseInfo.InternationalLicenseID,
                    LicenseInfo.DriverID,
                    LicenseInfo.IssuedUsingLocalLicenseID,
                    LicenseInfo.IssueDate,
                    LicenseInfo.ExpirationDate,
                    LicenseInfo.IsActive);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public bool Save()
        {
      
            base.Mode = (clsApplication.enMode)Mode;

            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateInternationalLicense();
            }

            return false;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
    }
}