using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
        public struct stLicenseClassInfo
        {
            public int LicenseClassID { get; set; }
            public string ClassName { get; set; }
            public string ClassDescription { get; set; }
            public byte MinimumAllowedAge { get; set; }
            public byte DefaultValidityLength { get; set; }
            public float ClassFees { get; set; }
        }

        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref stLicenseClassInfo licenseClassInfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    licenseClassInfo.LicenseClassID = (int)reader["LicenseClassID"];
                    licenseClassInfo.ClassName = (string)reader["ClassName"];
                    licenseClassInfo.ClassDescription = (string)reader["ClassDescription"];
                    licenseClassInfo.MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    licenseClassInfo.DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    licenseClassInfo.ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }
                reader.Close();
            }
            catch { isFound = false; }
            finally { connection.Close(); }
            return isFound;
        }

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref stLicenseClassInfo licenseClassInfo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClassName", ClassName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    licenseClassInfo.LicenseClassID = (int)reader["LicenseClassID"];
                    licenseClassInfo.ClassName = (string)reader["ClassName"];
                    licenseClassInfo.ClassDescription = (string)reader["ClassDescription"];
                    licenseClassInfo.MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    licenseClassInfo.DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    licenseClassInfo.ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }
                reader.Close();
            }
            catch { isFound = false; }
            finally { connection.Close(); }
            return isFound;
        }

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM LicenseClasses order by ClassName";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) dt.Load(reader);
                reader.Close();
            }
            catch { }
            finally { connection.Close(); }
            return dt;
        }

        public static int AddNewLicenseClass(stLicenseClassInfo licenseClassInfo)
        {
            int LicenseClassID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"Insert Into LicenseClasses 
                           (ClassName,ClassDescription,MinimumAllowedAge, 
                            DefaultValidityLength,ClassFees)
                            Values ( 
                            @ClassName,@ClassDescription,@MinimumAllowedAge, 
                            @DefaultValidityLength,@ClassFees);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClassName", licenseClassInfo.ClassName);
            command.Parameters.AddWithValue("@ClassDescription", licenseClassInfo.ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", licenseClassInfo.MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", licenseClassInfo.DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", licenseClassInfo.ClassFees);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseClassID = insertedID;
                }
            }
            catch { } 
            finally { connection.Close(); }
            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(stLicenseClassInfo licenseClassInfo) 
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"Update LicenseClasses  
                             set ClassName = @ClassName,
                                 ClassDescription = @ClassDescription,
                                 MinimumAllowedAge = @MinimumAllowedAge,
                                 DefaultValidityLength = @DefaultValidityLength,
                                 ClassFees = @ClassFees
                                 where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassInfo.LicenseClassID);
            command.Parameters.AddWithValue("@ClassName", licenseClassInfo.ClassName);
            command.Parameters.AddWithValue("@ClassDescription", licenseClassInfo.ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", licenseClassInfo.MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", licenseClassInfo.DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", licenseClassInfo.ClassFees);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch { return false; }
            finally { connection.Close(); }
            return (rowsAffected > 0);
        }
    }
}