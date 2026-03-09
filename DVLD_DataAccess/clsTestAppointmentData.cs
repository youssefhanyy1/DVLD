using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils; // ضفنا الـ Utils عشان الـ Log

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
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetTestAppointmentInfoByID", "Data access");
                isfound = false;
            }
            return isfound;
        }

        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID, ref stTestAppointment testAppointment)
        {
            bool isfound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT top 1 *
                                     FROM TestAppointments
                                     WHERE (TestTypeID = @TestTypeID) 
                                     AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                                     order by TestAppointmentID Desc";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetLastTestAppointment", "Data access");
                isfound = false;
            }
            return isfound;
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"select * from TestAppointments_View order by AppointmentDate Desc";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetAllTestAppointments", "Data access");
            }
            return dt;
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked
                                     FROM TestAppointments
                                     WHERE (TestTypeID = @TestTypeID) 
                                     AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                                     order by TestAppointmentID desc;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetApplicationTestAppointmentsPerTestType", "Data access");
            }
            return dt;
        }

        public static int AddNewTestAppointment(stTestAppointment testAppointment)
        {
            int TestAppointmentID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Insert Into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
                                     Values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,0,@RetakeTestApplicationID);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", testAppointment.TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testAppointment.LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate", testAppointment.AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", testAppointment.PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", testAppointment.CreatedByUserID);

                        if (testAppointment.RetakeTestApplicationID == -1)
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", testAppointment.RetakeTestApplicationID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestAppointmentID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewTestAppointment", "Data access");
            }
            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(stTestAppointment testAppointment)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update TestAppointments  
                                     set TestTypeID = @TestTypeID,
                                         LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                         AppointmentDate = @AppointmentDate,
                                         PaidFees = @PaidFees,
                                         CreatedByUserID = @CreatedByUserID,
                                         IsLocked=@IsLocked,
                                         RetakeTestApplicationID=@RetakeTestApplicationID
                                     where TestAppointmentID = @TestAppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateTestAppointment", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
                Log.LogException(ex, "GetTestID", "Data access");
            }
            return TestID;
        }
    }
}