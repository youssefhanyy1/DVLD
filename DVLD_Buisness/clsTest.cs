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
    public class clsTest
    {
        public enum enMode {AddNew=0, Update=1 };
        public enMode Mode = enMode.AddNew;
        public int TestID {  get; set; }
        public int TestAppointmentID { get; set; }

        public clsTestAppointment TestAppointmentInfo {  get; set; }

        public bool TestResult {  get; set; }

        public string Notes { get; set; }

        public int CreatedByUserID {  get; set; }

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        public clsTest(int TestID, int TestAppointmentID,
            bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }
        private bool _AddNewTest()
        {
            clsTestData.stTestInfo testInfo = new clsTestData.stTestInfo();

            testInfo.TestID = this.TestID;
            testInfo.TestAppointmentID=this.TestAppointmentID;
            testInfo.TestResult = this.TestResult;
            testInfo.Notes = this.Notes;
            testInfo.CreatedByUserID=this.CreatedByUserID;

            this.TestID = clsTestData.AddNewTest(testInfo);
            return (this.TestID != -1);
        }
        private bool _UpdateTest()
        {
            clsTestData.stTestInfo TestInfo = new clsTestData.stTestInfo();

            TestInfo.TestID = this.TestID;
            TestInfo.TestAppointmentID = this.TestAppointmentID;
            TestInfo.TestResult = this.TestResult;
            TestInfo.Notes = this.Notes;
            TestInfo.CreatedByUserID = this.CreatedByUserID;

            return clsTestData.UpdateTest(TestInfo);
        }
        public static clsTest Find(int TestID)
        {
            clsTestData.stTestInfo TestInfo = new clsTestData.stTestInfo();

            if (clsTestData.GetTestInfoByID(TestID, ref TestInfo))
            {
                return new clsTest(TestInfo.TestID,
                        TestInfo.TestAppointmentID,
                        TestInfo.TestResult,
                        TestInfo.Notes,
                        TestInfo.CreatedByUserID);
            }
            else
            {
                return null;
            }
        }
        public static clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            clsTestData.stTestInfo TestInfo = new clsTestData.stTestInfo();

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestTypeID, ref TestInfo))
            {
                return new clsTest(TestInfo.TestID,
                        TestInfo.TestAppointmentID,
                        TestInfo.TestResult,
                        TestInfo.Notes,
                        TestInfo.CreatedByUserID);
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }
        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTest();
            }

            return false;
        }
    }
}
