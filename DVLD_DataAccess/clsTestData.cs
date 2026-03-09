using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils; // ضفنا دي عشان الـ Log

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public struct stTestInfo
        {
            public int TestID { get; set; }
            public int TestAppointmentID { get; set; }
            public bool TestResult { get; set; }
            public string Notes { get; set; }
            public int CreatedByUserID { get; set; }
        }

        public static bool GetTestInfoByID(int TestID, ref stTestInfo TestInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from tests where TestID=@TestID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TestID", TestID);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                TestInfo.TestID = (int)reader["TestID"];
                                TestInfo.TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestInfo.TestResult = (bool)reader["TestResult"];

                                if (reader["Notes"] == DBNull.Value)
                                    TestInfo.Notes = "";
                                else
                                    TestInfo.Notes = (string)reader["Notes"];

                                TestInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetTestInfoByID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, int TestTypeID, ref stTestInfo testInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT top 1 Tests.TestID, 
                        Tests.TestAppointmentID, Tests.TestResult, 
                        Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                        FROM LocalDrivingLicenseApplications 
                        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                        INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                        WHERE (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND (TestAppointments.TestTypeID = @TestTypeID)
                        ORDER BY Tests.TestAppointmentID DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                testInfo.TestID = (int)reader["TestID"];
                                testInfo.TestAppointmentID = (int)reader["TestAppointmentID"];
                                testInfo.TestResult = (bool)reader["TestResult"];

                                if (reader["Notes"] == DBNull.Value)
                                    testInfo.Notes = "";
                                else
                                    testInfo.Notes = (string)reader["Notes"];

                                testInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetLastTestByPersonAndTestTypeAndLicenseClass", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Tests order by TestID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetAllTests", "Data access");
            }
            return dt;
        }

        public static int AddNewTest(stTestInfo testInfo)
        {
            int TestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Insert Into Tests (TestAppointmentID,TestResult, Notes, CreatedByUserID)
                                    Values (@TestAppointmentID,@TestResult, @Notes, @CreatedByUserID);
                                    
                                    UPDATE TestAppointments 
                                    SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;

                                    SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", testInfo.TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", testInfo.TestResult);

                        if (string.IsNullOrEmpty(testInfo.Notes))
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", testInfo.Notes);

                        command.Parameters.AddWithValue("@CreatedByUserID", testInfo.CreatedByUserID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewTest", "Data access");
            }
            return TestID;
        }

        public static bool UpdateTest(stTestInfo testInfo)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update Tests  
                                    set TestAppointmentID = @TestAppointmentID,
                                        TestResult=@TestResult,
                                        Notes = @Notes,
                                        CreatedByUserID=@CreatedByUserID
                                        where TestID = @TestID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestID", testInfo.TestID);
                        command.Parameters.AddWithValue("@TestAppointmentID", testInfo.TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", testInfo.TestResult);

                        if (string.IsNullOrEmpty(testInfo.Notes))
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", testInfo.Notes);

                        command.Parameters.AddWithValue("@CreatedByUserID", testInfo.CreatedByUserID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateTest", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT PassedTestCount = count(TestTypeID)
                                 FROM Tests INNER JOIN
                                 TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                                 where LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestResult=1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                        {
                            PassedTestCount = ptCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetPassedTestCount", "Data access");
            }
            return PassedTestCount;
        }
    }
}