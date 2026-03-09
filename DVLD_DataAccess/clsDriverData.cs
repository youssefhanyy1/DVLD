using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils; // تم الإضافة عشان الـ Logging

namespace DVLD_DataAccess
{
    public class clsDriverData
    {
        public struct stDriverInfo
        {
            public int DriverID { get; set; }
            public int PersonID { get; set; }
            public int CreatedByUserID { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public static bool GetDriverInfoByDriverID(int DriverID, ref stDriverInfo DriverInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                DriverInfo.DriverID = (int)reader["DriverID"];
                                DriverInfo.PersonID = (int)reader["PersonID"];
                                DriverInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                                DriverInfo.CreatedDate = (DateTime)reader["CreatedDate"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetDriverInfoByDriverID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref stDriverInfo DriverInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                DriverInfo.DriverID = (int)reader["DriverID"];
                                DriverInfo.PersonID = (int)reader["PersonID"];
                                DriverInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                                DriverInfo.CreatedDate = (DateTime)reader["CreatedDate"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetDriverInfoByPersonID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Drivers_View order by FullName";
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
                Log.LogException(ex, "GetAllDrivers", "Data access");
            }
            return dt;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                                     Values (@PersonID,@CreatedByUserID,@CreatedDate);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                        {
                            DriverID = InsertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewDriver", "Data access");
            }
            return DriverID;
        }

        public static bool UpdateDriver(stDriverInfo driverInfo)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update Drivers  
                                     set PersonID = @PersonID,
                                         CreatedByUserID = @CreatedByUserID
                                         where DriverID = @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", driverInfo.DriverID);
                        command.Parameters.AddWithValue("@PersonID", driverInfo.PersonID);
                        command.Parameters.AddWithValue("@CreatedByUserID", driverInfo.CreatedByUserID);

                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateDriver", "Data access");
                return false;
            }
            return (rowAffected > 0);
        }
    }
}