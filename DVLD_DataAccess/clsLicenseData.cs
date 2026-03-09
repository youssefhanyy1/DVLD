using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils; // ضفنا الـ Utils عشان الـ Log

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {
        public struct stLicenseInfo
        {
            public int LicenseID { get; set; }
            public int ApplicationID { get; set; }
            public int DriverID { get; set; }
            public int LicenseClass { get; set; }
            public DateTime IssueDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public string Notes { get; set; }
            public float PaidFees { get; set; }
            public bool IsActive { get; set; }
            public byte IssueReason { get; set; }
            public int CreatedByUserID { get; set; }
        }

        public static bool GetLicenseInfoByID(int LicenseID, ref stLicenseInfo licenseInfo)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                licenseInfo.LicenseID = (int)reader["LicenseID"];
                                licenseInfo.ApplicationID = (int)reader["ApplicationID"];
                                licenseInfo.DriverID = (int)reader["DriverID"];
                                licenseInfo.LicenseClass = (int)reader["LicenseClass"];
                                licenseInfo.IssueDate = (DateTime)reader["IssueDate"];
                                licenseInfo.ExpirationDate = (DateTime)reader["ExpirationDate"];

                                if (reader["Notes"] == DBNull.Value)
                                    licenseInfo.Notes = "";
                                else
                                    licenseInfo.Notes = (string)reader["Notes"];

                                licenseInfo.PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                licenseInfo.IsActive = (bool)reader["IsActive"];
                                licenseInfo.IssueReason = (byte)reader["IssueReason"];
                                licenseInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetLicenseInfoByID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Licenses";
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
                Log.LogException(ex, "GetAllLicenses", "Data access");
            }
            return dt;
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT     
                                   Licenses.LicenseID,
                                   ApplicationID,
                                   LicenseClasses.ClassName, Licenses.IssueDate, 
                                   Licenses.ExpirationDate, Licenses.IsActive
                                   FROM Licenses INNER JOIN
                                        LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                                   where DriverID=@DriverID
                                   Order By IsActive Desc, ExpirationDate Desc";

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
                Log.LogException(ex, "GetDriverLicenses", "Data access");
            }
            return dt;
        }

        public static int AddNewLicense(stLicenseInfo licenseInfo)
        {
            int LicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"
                                  INSERT INTO Licenses
                                   (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive,IssueReason, CreatedByUserID)
                                  VALUES
                                   (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive,@IssueReason, @CreatedByUserID);
                                  SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", licenseInfo.ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", licenseInfo.DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", licenseInfo.LicenseClass);
                        command.Parameters.AddWithValue("@IssueDate", licenseInfo.IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", licenseInfo.ExpirationDate);

                        if (string.IsNullOrEmpty(licenseInfo.Notes))
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", licenseInfo.Notes);

                        command.Parameters.AddWithValue("@PaidFees", licenseInfo.PaidFees);
                        command.Parameters.AddWithValue("@IsActive", licenseInfo.IsActive);
                        command.Parameters.AddWithValue("@IssueReason", licenseInfo.IssueReason);
                        command.Parameters.AddWithValue("@CreatedByUserID", licenseInfo.CreatedByUserID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewLicense", "Data access");
            }
            return LicenseID;
        }

        public static bool UpdateLicense(stLicenseInfo licenseInfo)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"UPDATE Licenses
                                   SET ApplicationID=@ApplicationID, DriverID = @DriverID,
                                      LicenseClass = @LicenseClass,
                                      IssueDate = @IssueDate,
                                      ExpirationDate = @ExpirationDate,
                                      Notes = @Notes,
                                      PaidFees = @PaidFees,
                                      IsActive = @IsActive,IssueReason=@IssueReason,
                                      CreatedByUserID = @CreatedByUserID
                                 WHERE LicenseID=@LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", licenseInfo.LicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", licenseInfo.ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", licenseInfo.DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", licenseInfo.LicenseClass);
                        command.Parameters.AddWithValue("@IssueDate", licenseInfo.IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", licenseInfo.ExpirationDate);

                        if (string.IsNullOrEmpty(licenseInfo.Notes))
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", licenseInfo.Notes);

                        command.Parameters.AddWithValue("@PaidFees", licenseInfo.PaidFees);
                        command.Parameters.AddWithValue("@IsActive", licenseInfo.IsActive);
                        command.Parameters.AddWithValue("@IssueReason", licenseInfo.IssueReason);
                        command.Parameters.AddWithValue("@CreatedByUserID", licenseInfo.CreatedByUserID);

                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateLicense", "Data access");
                return false;
            }
            return (rowAffected > 0);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT        Licenses.LicenseID
                                    FROM Licenses INNER JOIN
                                                     Drivers ON Licenses.DriverID = Drivers.DriverID
                                    WHERE  
                                     Licenses.LicenseClass = @LicenseClass 
                                      AND Drivers.PersonID = @PersonID
                                      And IsActive=1;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetActiveLicenseIDByPersonID", "Data access");
            }
            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"UPDATE Licenses
                                   SET 
                                      IsActive = 0
                                 WHERE LicenseID=@LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);

                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "DeactivateLicense", "Data access");
                return false;
            }
            return (rowAffected > 0);
        }
    }
}