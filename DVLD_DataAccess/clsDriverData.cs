using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    DriverInfo.DriverID = (int)reader["DriverID"];
                    DriverInfo.PersonID = (int)reader["PersonID"];
                    DriverInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                    DriverInfo.CreatedDate = (DateTime)reader["CreatedDate"];
                }
                reader.Close();
            }
            catch { isFound = false; }
            finally { connection.Close(); }
            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref stDriverInfo DriverInfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    DriverInfo.DriverID = (int)reader["DriverID"];
                    DriverInfo.PersonID = (int)reader["PersonID"];
                    DriverInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                    DriverInfo.CreatedDate = (DateTime)reader["CreatedDate"];
                }
                reader.Close();
            }
            catch { isFound = false; }
            finally { connection.Close(); }
            return isFound;
        }

        public static DataTable GetAllDrivers() 
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM Drivers_View order by FullName";
            SqlCommand commend = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = commend.ExecuteReader();
                if (reader.HasRows) dt.Load(reader);
                reader.Close();
            }
            catch { }
            finally { connection.Close(); }
            return dt;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                             Values (@PersonID,@CreatedByUserID,@CreatedDate);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    DriverID = InsertedID;
                }
            }
            catch { } 
            finally { connection.Close(); }
            return DriverID;
        }

        public static bool UpdateDriver(stDriverInfo driverInfo)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Update Drivers  
                             set PersonID = @PersonID,
                                 CreatedByUserID = @CreatedByUserID
                                 where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", driverInfo.DriverID);
            command.Parameters.AddWithValue("@PersonID", driverInfo.PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", driverInfo.CreatedByUserID);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch { return false; }
            finally { connection.Close(); }
            return (rowAffected > 0);
        }
    }
}