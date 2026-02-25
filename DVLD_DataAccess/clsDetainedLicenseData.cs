using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
                reader.Close();
            }
            catch
            {
                return isFound;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref stDetainedLicenseInfo DetainedLicenseInfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID order by DetainID desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
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
                reader.Close();
            }
            catch
            {
                return isFound;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static DataTable GetAllDetainedLicense()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";
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

        public static int AddNewDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            int DetainID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
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
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", detainedLicenseInfo.LicenseID);
            command.Parameters.AddWithValue("@DetainDate", detainedLicenseInfo.DetainDate);
            command.Parameters.AddWithValue("@FineFees", detainedLicenseInfo.FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", detainedLicenseInfo.CreatedByUserID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                }
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return DetainID;
        }

        public static bool UpdateDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", detainedLicenseInfo.LicenseID);
            command.Parameters.AddWithValue("@DetainDate", detainedLicenseInfo.DetainDate);
            command.Parameters.AddWithValue("@FineFees", detainedLicenseInfo.FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", detainedLicenseInfo.CreatedByUserID);
            command.Parameters.AddWithValue("@DetainID", detainedLicenseInfo.DetainID);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }

        public static bool ReleaseDetainedLicense(stDetainedLicenseInfo detainedLicenseInfo)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID = @ReleaseApplicationID,
                              ReleasedByUserID = @ReleasedByUserID
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", detainedLicenseInfo.DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", detainedLicenseInfo.ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", detainedLicenseInfo.ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"select IsDetained=1 
                            from detainedLicenses 
                            where 
                            LicenseID=@LicenseID 
                            and IsReleased=0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsDetained = Convert.ToBoolean(result);
                }
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return IsDetained;
        }
    }
}