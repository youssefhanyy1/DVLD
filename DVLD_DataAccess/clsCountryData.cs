using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        public enum enGender {Male=0,Female=1};

        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
          bool IsFound= false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select * from Countries\r\nwhere CountryID=@CountryID;";
            SqlCommand cmd = new SqlCommand(query,connection);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) { 
                IsFound = true;
                    CountryName = (reader["CountryName"]).ToString();
                }
                reader.Close();

            }
            catch (Exception) {
                IsFound = false;
            }
            finally
            {
                connection.Close();
                
            }
            return IsFound;
        }

        public static bool GetCountryInfoByName(string CountryName,ref int CounreyID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select * from Countries\r\nwhere CountryName=@CountryName;";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    CounreyID = (int)reader["CountryID"];
                }
                reader.Close();

            }
            catch (Exception)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();

            }
            return IsFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable Dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "SELECT * FROM Countries order by CountryName";
            SqlCommand Command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.HasRows)
                {
                    Dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception)
            {

            }
            finally { 
            connection.Close();
            }
            return Dt;
        }
    }
}
