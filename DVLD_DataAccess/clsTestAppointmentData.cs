using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public struct stTestAppointment
        {
            public int TestAppointmentID { get; set; }
            public int TestTypeID { get; set; }
            public int LocalDrivingLicenseApplicationID { get; set; }
            public DateTime AppointmentDate { get; set; }
            public float PaidFees { get; set; }
            public int CreatedByUserID { get; set; }
            public bool IsLocked { get; set; }
            public int RetakeTestApplicationID { get; set; }
        }

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref stTestAppointment testAppointment)
        {
            bool isfound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isfound = true;
                    testAppointment.TestAppointmentID = (int)reader["TestAppointmentID"];
                    testAppointment.TestTypeID = (int)reader["TestTypeID"];
                    testAppointment.LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    testAppointment.AppointmentDate = (DateTime)reader["AppointmentDate"];
                    testAppointment.PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    testAppointment.CreatedByUserID = (int)reader["CreatedByUserID"];
                    testAppointment.IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        testAppointment.RetakeTestApplicationID = -1;
                    else
                        testAppointment.RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                }
                else
                {
                    isfound = false;
                }
                reader.Close();
            }
            catch
            {
                isfound = false;
            }
            finally
            {
                connection.Close();
            }
            return isfound;
        }

        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID, ref stTestAppointment testAppointment)
        {
            bool isfound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"SELECT top 1 *
                             FROM TestAppointments
                             WHERE (TestTypeID = @TestTypeID) 
                             AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             order by TestAppointmentID Desc";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isfound = true;
                    testAppointment.TestAppointmentID = (int)reader["TestAppointmentID"];
                    testAppointment.TestTypeID = (int)reader["TestTypeID"]; 
                    testAppointment.LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"]; 
                    testAppointment.AppointmentDate = (DateTime)reader["AppointmentDate"];
                    testAppointment.PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    testAppointment.CreatedByUserID = (int)reader["CreatedByUserID"];
                    testAppointment.IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        testAppointment.RetakeTestApplicationID = -1;
                    else
                        testAppointment.RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                }
                else
                {
                    isfound = false;
                }
                reader.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return isfound;
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"select * from TestAppointments_View order by AppointmentDate Desc";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked
                             FROM TestAppointments
                             WHERE (TestTypeID = @TestTypeID) 
                             AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                             order by TestAppointmentID desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static int AddNewTestAppointment(stTestAppointment testAppointment)
        {
            int TestAppointmentID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Insert Into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
                             Values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,0,@RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", testAppointment.TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testAppointment.LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", testAppointment.AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", testAppointment.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", testAppointment.CreatedByUserID);

            if (testAppointment.RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", testAppointment.RetakeTestApplicationID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(stTestAppointment testAppointment) 
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"Update TestAppointments  
                             set TestTypeID = @TestTypeID,
                                 LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                 AppointmentDate = @AppointmentDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID,
                                 IsLocked=@IsLocked,
                                 RetakeTestApplicationID=@RetakeTestApplicationID
                             where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testAppointment.TestAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", testAppointment.TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testAppointment.LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", testAppointment.AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", testAppointment.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", testAppointment.CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", testAppointment.IsLocked);

            if (testAppointment.RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", testAppointment.RetakeTestApplicationID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch { return false; }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
            finally
            {
                connection.Close();
            }
            return TestID;
        }
    }
}