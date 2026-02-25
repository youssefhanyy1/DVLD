using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business 
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public clsTestType.enTestType TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public clsApplication RetakeTestAppInfo { get; set; }

        public int TestID
        {
            get { return _GetTestID(); }
        }

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;
            Mode = enMode.AddNew;
        }

        public clsTestAppointment(int TestAppointmentID, clsTestType.enTestType TestTypeID,
        int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
        int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;

            if (RetakeTestApplicationID != -1)
                this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);
            else
                this.RetakeTestAppInfo = null;

            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        {
            clsTestAppointmentData.stTestAppointment AppointmentInfo = new clsTestAppointmentData.stTestAppointment();

            AppointmentInfo.TestTypeID = (int)this.TestTypeID;
            AppointmentInfo.LocalDrivingLicenseApplicationID = this.LocalDrivingLicenseApplicationID;
            AppointmentInfo.AppointmentDate = this.AppointmentDate;
            AppointmentInfo.PaidFees = this.PaidFees;
            AppointmentInfo.CreatedByUserID = this.CreatedByUserID;
            AppointmentInfo.RetakeTestApplicationID = this.RetakeTestApplicationID;

            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment(AppointmentInfo);

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            clsTestAppointmentData.stTestAppointment AppointmentInfo = new clsTestAppointmentData.stTestAppointment();

            AppointmentInfo.TestAppointmentID = this.TestAppointmentID;
            AppointmentInfo.TestTypeID = (int)this.TestTypeID;
            AppointmentInfo.LocalDrivingLicenseApplicationID = this.LocalDrivingLicenseApplicationID;
            AppointmentInfo.AppointmentDate = this.AppointmentDate;
            AppointmentInfo.PaidFees = this.PaidFees;
            AppointmentInfo.CreatedByUserID = this.CreatedByUserID;
            AppointmentInfo.IsLocked = this.IsLocked;
            AppointmentInfo.RetakeTestApplicationID = this.RetakeTestApplicationID;

            return clsTestAppointmentData.UpdateTestAppointment(AppointmentInfo);
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            clsTestAppointmentData.stTestAppointment AppointmentInfo = new clsTestAppointmentData.stTestAppointment();

            if (clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref AppointmentInfo))
            {
                return new clsTestAppointment(
                    AppointmentInfo.TestAppointmentID,
                    (clsTestType.enTestType)AppointmentInfo.TestTypeID,
                    AppointmentInfo.LocalDrivingLicenseApplicationID,
                    AppointmentInfo.AppointmentDate,
                    AppointmentInfo.PaidFees,
                    AppointmentInfo.CreatedByUserID,
                    AppointmentInfo.IsLocked,
                    AppointmentInfo.RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            clsTestAppointmentData.stTestAppointment AppointmentInfo = new clsTestAppointmentData.stTestAppointment();

            if (clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID, ref AppointmentInfo))
            {
                return new clsTestAppointment(
                    AppointmentInfo.TestAppointmentID,
                    (clsTestType.enTestType)AppointmentInfo.TestTypeID,
                    AppointmentInfo.LocalDrivingLicenseApplicationID,
                    AppointmentInfo.AppointmentDate,
                    AppointmentInfo.PaidFees,
                    AppointmentInfo.CreatedByUserID,
                    AppointmentInfo.IsLocked,
                    AppointmentInfo.RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTestAppointment();
            }

            return false;
        }

        private int _GetTestID()
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }
    }
}