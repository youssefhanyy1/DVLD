using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.ClsPersonData;

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
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string qurey = "select * from Users where UserID=@UserID";
            SqlCommand command = new SqlCommand(qurey, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    userData.UserID = (int)reader["UserID"];
                    userData.PersonID = (int)reader["PersonID"];
                    userData.UserName = (string)reader["UserName"];
                    userData.Password = (string)reader["Password"];
                    userData.IsActice = (bool)reader["IsActive"];
                }
                else
                {
                    IsFound = false;
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
        public static bool GetUserInfoByPersonID(int personID, ref stUserData userData)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string qurey = "select * from Users where PersonID=@PersonID";
            SqlCommand command = new SqlCommand(qurey, connection);
            command.Parameters.AddWithValue("@PersonID", personID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    userData.UserID = (int)reader["UserID"];
                    userData.PersonID = (int)reader["PersonID"];
                    userData.UserName = (string)reader["UserName"];
                    userData.Password = (string)reader["Password"];
                    userData.IsActice = (bool)reader["IsActive"];
                }
                else
                {
                    IsFound = false;
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
        public static bool GetUserInfoByUsernameAndPassword(string UserName,string Password, ref stUserData userData)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string qurey = "select * from Users where UserName=@UserName and Password=@Password ";
            SqlCommand command = new SqlCommand(qurey, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    userData.UserID = (int)reader["UserID"];
                    userData.PersonID = (int)reader["PersonID"];
                    userData.UserName = (string)reader["UserName"];
                    userData.Password = (string)reader["Password"];
                    userData.IsActice = (bool)reader["IsActive"];
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception )
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }
        public static int AddNewUser(stUserData userData)
        {
            int NewUserID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
                             VALUES (@PersonID, @UserName,@Password,@IsActive);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", userData.PersonID);
            command.Parameters.AddWithValue("@UserName", userData.UserName);
            command.Parameters.AddWithValue("@Password", userData.Password);
            command.Parameters.AddWithValue("@IsActive", userData.IsActice);

            try
            {
                connection.Open();
                object res = command.ExecuteScalar();
                if (res != null && int.TryParse(res.ToString(), out int insertID))
                {
                    NewUserID = insertID;

                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return NewUserID;
        }
        public static bool UpdateUserInfo(int UserID, stUserData userData)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"Update  Users  
                            set PersonID = @PersonID,
                                UserName = @UserName,
                                Password = @Password,
                                IsActive = @IsActive
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", userData.PersonID);
            command.Parameters.AddWithValue("@UserName", userData.UserName);
            command.Parameters.AddWithValue("@Password", userData.Password);
            command.Parameters.AddWithValue("@IsActive", userData.IsActice);
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowAffected > 0);
        }
        public static DataTable GetAllUser()
        {
            DataTable result = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"SELECT  Users.UserID, Users.PersonID,
                            FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                             Users.UserName, Users.IsActive
                             FROM  Users INNER JOIN
                                    People ON Users.PersonID = People.PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    result.Load(reader);
                }
            }
            catch
            {
                return null;

            }
            finally
            {
                connection.Close();
            }


            return result;

        }
        public static bool DeleteUser(int UserID)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"Delete Users 
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowAffected > 0);
        }
        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select Found=1 from Users where UserID=@UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception )
            {
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;


        }
        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select Found=1 from Users where UserName=@UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception )
            {
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;

        }
        public static bool IsUserExistForPersonID(int PersonID)
        {

            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select Found=1 from Users where PersonID=@PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    isFound = true;
                }
                reader.Close();
            }
            catch (Exception )
            {
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }
        public static bool DoesPersonHaveUser44(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool ChangePassword(int UserID, stUserData userData)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = @"Update  Users  
                            set Password = @Password
                            where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
          
            command.Parameters.AddWithValue("@Password", userData.Password); 
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowAffected > 0);
        }

    }
}
