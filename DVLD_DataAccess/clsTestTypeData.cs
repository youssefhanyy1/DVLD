using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils; // ضفنا دي عشان الـ Log 

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {
        public static bool GetTestTypeByID(int testTypeID, ref string TestTypeTitle, ref string testDescription, ref float TestFees)
        {
            bool Isfound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from TestTypes WHERE TestTypeID = @TestTypeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", testTypeID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Isfound = true;
                                TestTypeTitle = reader["TestTypeTitle"].ToString();
                                testDescription = reader["TestTypeDescription"].ToString();
                                TestFees = float.Parse(reader["TestTypeFees"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetTestTypeByID", "Data access");
                Isfound = false;
            }
            return Isfound;
        }

        public static DataTable GetAllTestType()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from TestTypes order by TestTypeID";
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
                Log.LogException(ex, "GetAllTestType", "Data access");
            }
            return dt;
        }

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
                                     VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@TestTypeFees", Fees);

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestTypeID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewTestType", "Data access");
            }
            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"UPDATE TestTypes
                                     SET TestTypeTitle = @TestTypeTitle,
                                         TestTypeDescription = @TestTypeDescription,
                                         TestTypeFees = @TestTypeFees
                                     WHERE TestTypeID = @TestTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@TestTypeFees", Fees);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateTestType", "Data access");
                return false;
            }
            return (rowsAffected > 0);
        }
    }
}