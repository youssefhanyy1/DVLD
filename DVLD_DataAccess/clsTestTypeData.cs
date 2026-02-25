using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {
        public static bool GetTestTypeByID(int testTypeID, ref string TestTypeTitle, ref string testDescription, ref float TestFees)
        {
            bool Isfound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = "select * from TestTypes WHERE TestTypeID = @TestTypeID"; 

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Isfound = true;

                    TestTypeTitle = reader["TestTypeTitle"].ToString();

                    testDescription = reader["TestTypeDescription"].ToString();
                    TestFees = float.Parse(reader["TestTypeFees"].ToString());
                }
                else
                {
                    Isfound = false;
                }
                reader.Close();
            }
            catch (Exception)
            {
                Isfound = false;
            }
            finally
            {
                connection.Close();
            }
            return Isfound;
        }

        public static DataTable GetAllTestType()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select * from TestTypes order by TestTypeID"; 
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
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
                             VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", Title);
            command.Parameters.AddWithValue("@TestTypeDescription", Description);
            command.Parameters.AddWithValue("@TestTypeFees", Fees);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestTypeID = insertedID;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                connection.Close();
            }
            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"UPDATE TestTypes
                             SET TestTypeTitle = @TestTypeTitle,
                                 TestTypeDescription = @TestTypeDescription,
                                 TestTypeFees = @TestTypeFees
                             WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", Title);
            command.Parameters.AddWithValue("@TestTypeDescription", Description);
            command.Parameters.AddWithValue("@TestTypeFees", Fees);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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
    }
}