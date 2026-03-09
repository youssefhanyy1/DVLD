using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DVLD_DataAccess
{
    public class ClsUserData
    {
        public struct stUserData
        {
            public int UserID { get; set; }
            public int PersonID { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool IsActice { get; set; }
        }

        public static bool GetUserInfoByUserID(int UserID, ref stUserData userData)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from Users where UserID=@UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                userData.UserID = (int)reader["UserID"];
                                userData.PersonID = (int)reader["PersonID"];
                                userData.UserName = (string)reader["UserName"];
                                userData.Password = (string)reader["Password"];
                                userData.IsActice = (bool)reader["IsActive"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetUserInfoByUserID", "Data access");
                IsFound = false;
            }
            return IsFound;
        }

        public static bool GetUserInfoByPersonID(int personID, ref stUserData userData)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from Users where PersonID=@PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", personID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                userData.UserID = (int)reader["UserID"];
                                userData.PersonID = (int)reader["PersonID"];
                                userData.UserName = (string)reader["UserName"];
                                userData.Password = (string)reader["Password"];
                                userData.IsActice = (bool)reader["IsActive"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetUserInfoByPersonID", "Data access");
                IsFound = false;
            }
            return IsFound;
        }

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password, ref stUserData userData)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select * from Users where UserName=@UserName and Password=@Password ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                userData.UserID = (int)reader["UserID"];
                                userData.PersonID = (int)reader["PersonID"];
                                userData.UserName = (string)reader["UserName"];
                                userData.Password = (string)reader["Password"];
                                userData.IsActice = (bool)reader["IsActive"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetUserInfoByUsernameAndPassword", "Data access");
                IsFound = false;
            }
            return IsFound;
        }

        public static int AddNewUser(stUserData userData)
        {
            int NewUserID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
                                     VALUES (@PersonID, @UserName,@Password,@IsActive);
                                     SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", userData.PersonID);
                        command.Parameters.AddWithValue("@UserName", userData.UserName);
                        command.Parameters.AddWithValue("@Password", userData.Password);
                        command.Parameters.AddWithValue("@IsActive", userData.IsActice);

                        connection.Open();
                        object res = command.ExecuteScalar();
                        if (res != null && int.TryParse(res.ToString(), out int insertID))
                        {
                            NewUserID = insertID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "AddNewUser", "Data access");
                NewUserID = -1;
            }
            return NewUserID;
        }

        public static bool UpdateUserInfo(int UserID, stUserData userData)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update  Users  
                                     set PersonID = @PersonID,
                                         UserName = @UserName,
                                         Password = @Password,
                                         IsActive = @IsActive
                                     where UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", userData.PersonID);
                        command.Parameters.AddWithValue("@UserName", userData.UserName);
                        command.Parameters.AddWithValue("@Password", userData.Password);
                        command.Parameters.AddWithValue("@IsActive", userData.IsActice);
                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "UpdateUserInfo", "Data access");
                return false;
            }
            return (rowAffected > 0);
        }

        public static DataTable GetAllUser()
        {
            DataTable result = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"SELECT  Users.UserID, Users.PersonID,
                                    FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                                     Users.UserName, Users.IsActive
                                     FROM  Users INNER JOIN
                                            People ON Users.PersonID = People.PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                result.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "GetAllUser", "Data access");
                return null;
            }
            return result;
        }

        public static bool DeleteUser(int UserID)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Delete Users 
                                     where UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "DeleteUser", "Data access");
                return false;
            }
            return (rowAffected > 0);
        }

        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select Found=1 from Users where UserID=@UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
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
                Log.LogException(ex, "IsUserExist_UserID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select Found=1 from Users where UserName=@UserName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", UserName);
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
                Log.LogException(ex, "IsUserExist_UserName", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "select Found=1 from Users where PersonID=@PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
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
                Log.LogException(ex, "IsUserExistForPersonID", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool DoesPersonHaveUser44(int PersonID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
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
                Log.LogException(ex, "DoesPersonHaveUser44", "Data access");
                isFound = false;
            }
            return isFound;
        }

        public static bool ChangePassword(int UserID, stUserData userData)
        {
            int rowAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ClsDataAccsess.connations))
                {
                    string query = @"Update  Users  
                                     set Password = @Password
                                     where UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Password", userData.Password);

                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "ChangePassword", "Data access");
                return false;
            }
            return (rowAffected > 0);
        }
    }
}