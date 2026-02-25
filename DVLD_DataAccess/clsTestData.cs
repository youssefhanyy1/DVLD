using System;
using System.Data;
using System.Data.SqlClient;

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
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select * from tests where TestID=@TestID";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
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
                reader.Close();
            }
            catch { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, int TestTypeID, ref stTestInfo testInfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

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

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
                reader.Close();
            }
            catch { isFound = false; }
            finally { connection.Close(); }

            return isFound;
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM Tests order by TestID";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) dt.Load(reader);
                reader.Close();
            }
            catch { }
            finally { connection.Close(); }
            return dt;
        }

        public static int AddNewTest(stTestInfo testInfo)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Insert Into Tests (TestAppointmentID,TestResult, Notes, CreatedByUserID)
                            Values (@TestAppointmentID,@TestResult, @Notes, @CreatedByUserID);
                            
                            UPDATE TestAppointments 
                            SET IsLocked=1 where TestAppointmentID = @TestAppointmentID;

                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", testInfo.TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", testInfo.TestResult);

            if (string.IsNullOrEmpty(testInfo.Notes))
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", testInfo.Notes);

            command.Parameters.AddWithValue("@CreatedByUserID", testInfo.CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch { } 
            finally { connection.Close(); }

            return TestID;
        }

        public static bool UpdateTest(stTestInfo testInfo)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Update Tests  
                            set TestAppointmentID = @TestAppointmentID,
                                TestResult=@TestResult,
                                Notes = @Notes,
                                CreatedByUserID=@CreatedByUserID
                                where TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testInfo.TestID);
            command.Parameters.AddWithValue("@TestAppointmentID", testInfo.TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", testInfo.TestResult);

            if (string.IsNullOrEmpty(testInfo.Notes))
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", testInfo.Notes);

            command.Parameters.AddWithValue("@CreatedByUserID", testInfo.CreatedByUserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch { return false; }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"SELECT PassedTestCount = count(TestTypeID)
                         FROM Tests INNER JOIN
                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                         where LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID and TestResult=1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
                }
            }
            catch { }
            finally { connection.Close(); }

            return PassedTestCount;
        }
    }
}