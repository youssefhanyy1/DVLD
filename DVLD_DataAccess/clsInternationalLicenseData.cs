using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {
        public struct stInternationalLicenseInfo
        {
            public int InternationalLicenseID { get; set; }
            public int ApplicationID { get; set; }
            public int DriverID { get; set; }
            public int IssuedUsingLocalLicenseID { get; set; }
            public DateTime IssueDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public bool IsActive { get; set; }
            public int CreatedByUserID { get; set; }
        }

        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, ref stInternationalLicenseInfo internationalLicenseInfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    internationalLicenseInfo.InternationalLicenseID = (int)reader["InternationalLicenseID"];
                    internationalLicenseInfo.ApplicationID = (int)reader["ApplicationID"];
                    internationalLicenseInfo.DriverID = (int)reader["DriverID"];
                    internationalLicenseInfo.IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    internationalLicenseInfo.IssueDate = (DateTime)reader["IssueDate"];
                    internationalLicenseInfo.ExpirationDate = (DateTime)reader["ExpirationDate"];
                    internationalLicenseInfo.IsActive = (bool)reader["IsActive"];
                    internationalLicenseInfo.CreatedByUserID = (int)reader["CreatedByUserID"]; 
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"
            SELECT    InternationalLicenseID, ApplicationID,DriverID,
                        IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
            from InternationalLicenses 
                order by IsActive, ExpirationDate desc";

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
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"
            SELECT    InternationalLicenseID, ApplicationID,
                        IssuedUsingLocalLicenseID , IssueDate, 
                        ExpirationDate, IsActive
            from InternationalLicenses where DriverID=@DriverID
                order by ExpirationDate desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

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
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewInternationalLicense(stInternationalLicenseInfo internationalLicenseInfo)
        {
            int InternationalLicenseID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"
                               Update InternationalLicenses 
                               set IsActive=0
                               where DriverID=@DriverID;

                             INSERT INTO InternationalLicenses
                               (
                                ApplicationID,
                                DriverID,
                                IssuedUsingLocalLicenseID,
                                IssueDate,
                                ExpirationDate,
                                IsActive,
                                CreatedByUserID)
                         VALUES
                               (@ApplicationID,
                                @DriverID,
                                @IssuedUsingLocalLicenseID,
                                @IssueDate,
                                @ExpirationDate,
                                @IsActive,
                                @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", internationalLicenseInfo.ApplicationID);
            command.Parameters.AddWithValue("@DriverID", internationalLicenseInfo.DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", internationalLicenseInfo.IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", internationalLicenseInfo.IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", internationalLicenseInfo.ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", internationalLicenseInfo.IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", internationalLicenseInfo.CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(stInternationalLicenseInfo internationalLicenseInfo)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"UPDATE InternationalLicenses
                           SET 
                              ApplicationID=@ApplicationID,
                              DriverID = @DriverID,
                              IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                              IssueDate = @IssueDate,
                              ExpirationDate = @ExpirationDate,
                              IsActive = @IsActive,
                              CreatedByUserID = @CreatedByUserID
                         WHERE InternationalLicenseID=@InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseInfo.InternationalLicenseID);
            command.Parameters.AddWithValue("@ApplicationID", internationalLicenseInfo.ApplicationID);
            command.Parameters.AddWithValue("@DriverID", internationalLicenseInfo.DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", internationalLicenseInfo.IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", internationalLicenseInfo.IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", internationalLicenseInfo.ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", internationalLicenseInfo.IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", internationalLicenseInfo.CreatedByUserID);

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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                            order by ExpirationDate Desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }
    }
}