using System;
using System.Data;
using System.Data.SqlClient;
using Utils; // تم الإضافة عشان الـ Logging

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {
        public struct stApplicationInfo
        {
            public int ApplicationID { get; set; }
            public int ApplicantPersonID { get; set; }
            public DateTime ApplicationDate { get; set; }
            public int ApplicationTypeID { get; set; }
            public byte ApplicationStatus { get; set; }
            public DateTime LastStatusDate { get; set; }
            public float PaidFees { get; set; }
            public int CreatedByUserID { get; set; }
        }

        public static bool GetApplicationInfoByID(int ApplicationID, ref stApplicationInfo applicationInfo)
        {
            bool Isfound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Isfound = true;
                                applicationInfo.ApplicationID = (int)reader["ApplicationID"];
                                applicationInfo.ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                applicationInfo.ApplicationDate = (DateTime)reader["ApplicationDate"];
                                applicationInfo.ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                applicationInfo.ApplicationStatus = (byte)reader["ApplicationStatus"];
                                applicationInfo.LastStatusDate = (DateTime)reader["LastStatusDate"];
                                applicationInfo.PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                applicationInfo.CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetApplicationInfoByID", "Data access");
                Isfound = false;
            }
            return Isfound;
        }

        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from ApplicationsList_View order by ApplicationDate desc";
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
                Log.LogException(ex, "GetAllApplications", "Data access");
            }
            return dt;
        }

        public static int AddNewApplication(stApplicationInfo applicationInfo)
        {
            int ApplicationID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"INSERT INTO Applications (ApplicantPersonID,ApplicationDate,ApplicationTypeID,
                                    ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
                                    VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,
                                              @ApplicationStatus,@LastStatusDate,@PaidFees,@CreatedByUserID);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", applicationInfo.ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", applicationInfo.ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", applicationInfo.ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", applicationInfo.ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", applicationInfo.LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", applicationInfo.PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", applicationInfo.CreatedByUserID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ApplicationID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewApplication", "Data access");
            }
            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, stApplicationInfo applicationInfo)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update Applications set 
                                    ApplicantPersonID = @ApplicantPersonID,
                                    ApplicationDate = @ApplicationDate,
                                    ApplicationTypeID = @ApplicationTypeID,
                                    ApplicationStatus = @ApplicationStatus, 
                                    LastStatusDate = @LastStatusDate,
                                    PaidFees = @PaidFees,
                                    CreatedByUserID=@CreatedByUserID
                                    where ApplicationID=@ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@ApplicantPersonID", applicationInfo.ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", applicationInfo.ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", applicationInfo.ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", applicationInfo.ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", applicationInfo.LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", applicationInfo.PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", applicationInfo.CreatedByUserID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateApplication", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "DELETE FROM Applications WHERE ApplicationID=@ApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "DeleteApplication", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @ApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "IsApplicationExist", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT ActiveApplicationID=ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID and ApplicationTypeID=@ApplicationTypeID and ApplicationStatus=1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                        {
                            ActiveApplicationID = AppID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetActiveApplicationID", "Data access");
            }
            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                                    From Applications INNER JOIN
                                    LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                                    WHERE ApplicantPersonID = @ApplicantPersonID 
                                    and ApplicationTypeID=@ApplicationTypeID 
                                    and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                                    and ApplicationStatus=1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                        {
                            ActiveApplicationID = AppID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetActiveApplicationIDForLicenseClass", "Data access");
            }
            return ActiveApplicationID;
        }

        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update Applications set ApplicationStatus = @NewStatus, LastStatusDate = @LastStatusDate where ApplicationID=@ApplicationID;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateStatus", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }
    }
}