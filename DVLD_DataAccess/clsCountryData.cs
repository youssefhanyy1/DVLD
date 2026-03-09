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
    public class clsCountryData
    {
        public enum enGender { Male = 0, Female = 1 };

        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from Countries where CountryID=@CountryID;";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CountryID", CountryID);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                CountryName = reader["CountryName"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetCountryInfoByID", "Data access");
                IsFound = false;
            }
            return IsFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int CounreyID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from Countries where CountryName=@CountryName;";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CountryName", CountryName);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                CounreyID = (int)reader["CountryID"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetCountryInfoByName", "Data access");
                IsFound = false;
            }
            return IsFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable Dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT * FROM Countries order by CountryName";
                    using (SqlCommand Command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = Command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetAllCountries", "Data access");
            }
            return Dt;
        }
    }
}