using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business 
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };

        public enMode Mode = enMode.AddNew;
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };

        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }

        public clsPerson PersonInfo {  set; get; }
        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public clsApplicationType ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; }
        public string StatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo;

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsApplication(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, int ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.PersonInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            clsApplicationData.stApplicationInfo ApplicationInfo = new clsApplicationData.stApplicationInfo();

            ApplicationInfo.ApplicantPersonID = this.ApplicantPersonID;
            ApplicationInfo.ApplicationDate = this.ApplicationDate;

            ApplicationInfo.ApplicationTypeID = this.ApplicationTypeID;
            ApplicationInfo.ApplicationStatus = (byte)this.ApplicationStatus;
            ApplicationInfo.LastStatusDate = this.LastStatusDate;
            ApplicationInfo.PaidFees = this.PaidFees;
            ApplicationInfo.CreatedByUserID = this.CreatedByUserID;

            this.ApplicationID = clsApplicationData.AddNewApplication(ApplicationInfo);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            clsApplicationData.stApplicationInfo ApplicationInfo = new clsApplicationData.stApplicationInfo();

            ApplicationInfo.ApplicationID = this.ApplicationID;
            ApplicationInfo.ApplicantPersonID = this.ApplicantPersonID;
            ApplicationInfo.ApplicationDate = this.ApplicationDate;
            ApplicationInfo.ApplicationTypeID = this.ApplicationTypeID;
            ApplicationInfo.ApplicationStatus = (byte)this.ApplicationStatus;
            ApplicationInfo.LastStatusDate = this.LastStatusDate;
            ApplicationInfo.PaidFees = this.PaidFees;
            ApplicationInfo.CreatedByUserID = this.CreatedByUserID;

            return clsApplicationData.UpdateApplication(this.ApplicationID, ApplicationInfo);
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            clsApplicationData.stApplicationInfo ApplicationInfo = new clsApplicationData.stApplicationInfo();

            bool IsFound = clsApplicationData.GetApplicationInfoByID(ApplicationID, ref ApplicationInfo);

            if (IsFound)
            {
                return new clsApplication(
                    ApplicationInfo.ApplicationID,
                    ApplicationInfo.ApplicantPersonID,
                    ApplicationInfo.ApplicationDate,
                    ApplicationInfo.ApplicationTypeID,
                    (enApplicationStatus)ApplicationInfo.ApplicationStatus,
                    ApplicationInfo.LastStatusDate,
                    ApplicationInfo.PaidFees,
                    ApplicationInfo.CreatedByUserID
                );
            }
            else
            {
                return null;
            }
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 2);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 3);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public bool Delete()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }

        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public int GetActiveApplicationID(clsApplication.enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }

    }
}