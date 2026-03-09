using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils; // تم الإضافة عشان الـ Logging

namespace DVLD_DataAccess
{
    public class clsDetainedLicenseData
    {
        public struct stDetainedLicenseInfo
        {
            public int DetainID { get; set; }
            public int LicenseID { get; set; }
            public DateTime DetainDate { get; set; }
            public float FineFees { get; set; }
            public int CreatedByUserID { get; set; }
            public bool IsReleased { get; set; }
            public DateTime ReleaseDate { get; set; }
            public int ReleasedByUserID { get; set; }
            public int ReleaseApplicationID { get; set; }
        }

        public static bool GetDetainedLicenseInfoByID(int DetainID, ref stDetainedLicenseInfo DetainedLicenseInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                DetainedLicenseInfo.DetainID = Convert.ToInt32(reader["DetainID"]);
                                DetainedLicenseInfo.LicenseID = Convert.ToInt32(reader["LicenseID"]);
                                DetainedLicenseInfo.DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                                DetainedLicenseInfo.FineFees = Convert.ToSingle(reader["FineFees"]);
                                DetainedLicenseInfo.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                                DetainedLicenseInfo.IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)
                                    DetainedLicenseInfo.ReleaseDate = DateTime.MaxValue;
                                else
                                    DetainedLicenseInfo.ReleaseDate = (DateTime)reader["ReleaseDate"];

                                if (reader["ReleasedByUserID"] == DBNull.Value)
                                    DetainedLicenseInfo.ReleasedByUserID = -1;
                                else
                                    DetainedLicenseInfo.ReleasedByUserID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)
                                    DetainedLicenseInfo.ReleaseApplicationID = -1;
                                else
                                    DetainedLicenseInfo.ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetDetainedLicenseInfoByID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref stDetainedLicenseInfo DetainedLicenseInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID order by DetainID desc";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                DetainedLicenseInfo.DetainID = Convert.ToInt32(reader["DetainID"]);
                                DetainedLicenseInfo.LicenseID = Convert.ToInt32(reader["LicenseID"]);
                                DetainedLicenseInfo.DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                                DetainedLicenseInfo.FineFees = Convert.ToSingle(reader["FineFees"]);
                                DetainedLicenseInfo.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                                DetainedLicenseInfo.IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)
                                    DetainedLicenseInfo.ReleaseDate = DateTime.MaxValue;
                                else
                                    DetainedLicenseInfo.ReleaseDate = (DateTime)reader["ReleaseDate"];

                                if (reader["ReleasedByUserID"] == DBNull.Value)
                                    DetainedLicenseInfo.ReleasedByUserID = -1;
                                else
                                    DetainedLicenseInfo.ReleasedByUserID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)
                                    DetainedLicenseInfo.ReleaseApplicationID = -1;
                                else
                                    DetainedLicenseInfo.ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetDetainedLicenseInfoByLicenseID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllDetainedLicense()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";
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
                Log.LogException(ex, "GetAllDetainedLicense", "Data access");
            }
            return dt;
        }

        public static int AddNewDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            int DetainID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"INSERT INTO dbo.DetainedLicenses
                                       (LicenseID,
                                       DetainDate,
                                       FineFees,
                                       CreatedByUserID,
                                       IsReleased)
                                    VALUES
                                       (@LicenseID,
                                       @DetainDate, 
                                       @FineFees, 
                                       @CreatedByUserID,
                                       0);
                                    
                                    SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", detainedLicenseInfo.LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", detainedLicenseInfo.DetainDate);
                        command.Parameters.AddWithValue("@FineFees", detainedLicenseInfo.FineFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", detainedLicenseInfo.CreatedByUserID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            DetainID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewDetainedLicense", "Data access");
            }
            return DetainID;
        }

        public static bool UpdateDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"UPDATE dbo.DetainedLicenses
                                      SET LicenseID = @LicenseID, 
                                      DetainDate = @DetainDate, 
                                      FineFees = @FineFees,
                                      CreatedByUserID = @CreatedByUserID   
                                      WHERE DetainID=@DetainID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", detainedLicenseInfo.LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", detainedLicenseInfo.DetainDate);
                        command.Parameters.AddWithValue("@FineFees", detainedLicenseInfo.FineFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", detainedLicenseInfo.CreatedByUserID);
                        command.Parameters.AddWithValue("@DetainID", detainedLicenseInfo.DetainID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateDetainedLicense", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static bool ReleaseDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"UPDATE dbo.DetainedLicenses
                                      SET IsReleased = 1, 
                                      ReleaseDate = @ReleaseDate, 
                                      ReleaseApplicationID = @ReleaseApplicationID,
                                      ReleasedByUserID = @ReleasedByUserID
                                      WHERE DetainID=@DetainID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID", detainedLicenseInfo.DetainID);
                        command.Parameters.AddWithValue("@ReleasedByUserID", detainedLicenseInfo.ReleasedByUserID);
                        command.Parameters.AddWithValue("@ReleaseApplicationID", detainedLicenseInfo.ReleaseApplicationID);
                        command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "ReleaseDetainedLicense", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"select IsDetained=1 
                                    from detainedLicenses 
                                    where 
                                    LicenseID=@LicenseID 
                                    and IsReleased=0;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            IsDetained = Convert.ToBoolean(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "IsLicenseDetained", "Data access");
                IsDetained = false;
            }
            return IsDetained;
        }
    }
}