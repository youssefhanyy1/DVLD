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
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetInternationalLicenseInfoByID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"
                                    SELECT    InternationalLicenseID, ApplicationID,DriverID,
                                                IssuedUsingLocalLicenseID , IssueDate, 
                                                ExpirationDate, IsActive
                                    from InternationalLicenses 
                                        order by IsActive, ExpirationDate desc";

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
                Log.LogException(ex, "GetAllInternationalLicenses", "Data access");
            }
            return dt;
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"
                                    SELECT    InternationalLicenseID, ApplicationID,
                                                IssuedUsingLocalLicenseID , IssueDate, 
                                                ExpirationDate, IsActive
                                    from InternationalLicenses where DriverID=@DriverID
                                        order by ExpirationDate desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
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
                Log.LogException(ex, "GetDriverInternationalLicenses", "Data access");
            }
            return dt;
        }

        public static int AddNewInternationalLicense(stInternationalLicenseInfo internationalLicenseInfo)
        {
            int InternationalLicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
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

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", internationalLicenseInfo.ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", internationalLicenseInfo.DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", internationalLicenseInfo.IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", internationalLicenseInfo.IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", internationalLicenseInfo.ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", internationalLicenseInfo.IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", internationalLicenseInfo.CreatedByUserID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewInternationalLicense", "Data access");
            }
            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(stInternationalLicenseInfo internationalLicenseInfo)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
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

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseInfo.InternationalLicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", internationalLicenseInfo.ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", internationalLicenseInfo.DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", internationalLicenseInfo.IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", internationalLicenseInfo.IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", internationalLicenseInfo.ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", internationalLicenseInfo.IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", internationalLicenseInfo.CreatedByUserID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateInternationalLicense", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"  
                                    SELECT Top 1 InternationalLicenseID
                                    FROM InternationalLicenses 
                                    where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                                    order by ExpirationDate Desc;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetActiveInternationalLicenseIDByDriverID", "Data access");
            }
            return InternationalLicenseID;
        }
    }
}