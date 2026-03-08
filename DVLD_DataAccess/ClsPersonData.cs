using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace DVLD_DataAccess
{
    public class ClsPersonData
    {
       public  struct stpersondata
        {
            public int PersonID { get; set; }  
            public string Firstname {  get; set; }
            public string SecondName { get; set; }
            public string ThirdName {  get; set; }
            public string Lastname { get; set; }

            public string NationalNo { get; set; }
            public DateTime DataOfBirth {  get; set; }
            public short Gender {  get; set; }
            public string Address {  get; set; }
            public string Phone { get; set;  }
            public string Email {  get; set; }
            public int NationalityCountryID {  get; set; }

            public string ImagePath {  get; set; }

        }

        public static bool GetPersonInfoByID(int PersonID,ref stpersondata personData)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    personData.Firstname = (string)reader["FirstName"];
                    personData.SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        personData.ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        personData.ThirdName = "";
                    }

                    personData.Lastname = (string)reader["LastName"];
                    personData.NationalNo = (string)reader["NationalNo"];
                    personData.DataOfBirth = (DateTime)reader["DateOfBirth"];
                    personData.Gender = (byte)reader["Gendor"];
                    personData.Address = (string)reader["Address"];
                    personData.Phone = (string)reader["Phone"];


                    if (reader["Email"] != DBNull.Value)
                    {
                        personData.Email = (string)reader["Email"];
                    }
                    else
                    {
                        personData.Email = "";
                    }

                    personData.NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        personData.ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                       personData.ImagePath = "";
                    }

                }
                else
                {
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref stpersondata persondata)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string qurey = "select * from People where NationalNo=@NationalNo";
            SqlCommand command = new SqlCommand(qurey, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    IsFound = true;
                    persondata.PersonID = (int)reader["PersonID"];
                    persondata.Firstname = (string)reader["FirstName"];
                    persondata.SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                        persondata.ThirdName = (string)reader["ThirdName"];
                    else
                        persondata.ThirdName = "";

                    persondata.Lastname = (string)reader["LastName"];
                    persondata.NationalNo = (string)reader["NationalNo"];
                    persondata.DataOfBirth = (DateTime)reader["DateOfBirth"];

                    
                    persondata.Gender = Convert.ToByte(reader["Gendor"]);

                    persondata.Address = (string)reader["Address"];
                    persondata.Phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                        persondata.Email = (string)reader["Email"];
                    else
                        persondata.Email = "";

                    persondata.NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                        persondata.ImagePath = (string)reader["ImagePath"];
                    else
                        persondata.ImagePath = "";
                }
                else
                {
                    IsFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static int AddNewPerson(stpersondata persondata)
        {
            int NewPersonID = -1;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string qurey = @"INSERT INTO People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                                   DateOfBirth,Gendor,Address,Phone, Email, NationalityCountryID,ImagePath)
                             VALUES (@FirstName, @SecondName,@ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth,@Gendor,@Address,@Phone, @Email,@NationalityCountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(qurey, connection);

            command.Parameters.AddWithValue("@FirstName", persondata.Firstname);
            command.Parameters.AddWithValue("@SecondName", persondata.SecondName);

            if (!string.IsNullOrEmpty(persondata.ThirdName))
                command.Parameters.AddWithValue("@ThirdName", persondata.ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", persondata.Lastname);
            command.Parameters.AddWithValue("@NationalNo", persondata.NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", persondata.DataOfBirth);
            command.Parameters.AddWithValue("@Gendor", persondata.Gender);
            command.Parameters.AddWithValue("@Address", persondata.Address);
            command.Parameters.AddWithValue("@Phone", persondata.Phone);

            if (!string.IsNullOrEmpty(persondata.Email))
                command.Parameters.AddWithValue("@Email", persondata.Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", persondata.NationalityCountryID);

            if (!string.IsNullOrEmpty(persondata.ImagePath))
                command.Parameters.AddWithValue("@ImagePath", persondata.ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();
                object res = command.ExecuteScalar();
                if (res!=null&&int.TryParse(res.ToString(),out int insertID))
                {
                    NewPersonID = insertID;

                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return NewPersonID;
        }
        public static bool UpdatePersonInfo(int PersonID, stpersondata persondata)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor=@Gendor,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FirstName", persondata.Firstname);
            command.Parameters.AddWithValue("@SecondName",persondata.SecondName);
            if (!string.IsNullOrEmpty(persondata.ThirdName))
                command.Parameters.AddWithValue("@ThirdName", persondata.ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", persondata.Lastname);
            command.Parameters.AddWithValue("@NationalNo", persondata.NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", persondata.DataOfBirth);
            command.Parameters.AddWithValue("@Gendor", persondata.Gender);
            command.Parameters.AddWithValue("@Address", persondata.Address);
            command.Parameters.AddWithValue("@Phone", persondata.Phone);

            if (!string.IsNullOrEmpty(persondata.Email))
                command.Parameters.AddWithValue("@Email", persondata.Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", persondata.NationalityCountryID);

            if (!string.IsNullOrEmpty(persondata.ImagePath))
                command.Parameters.AddWithValue("@ImagePath", persondata.ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            
            try
            {
                connection.Open();
                rowAffected= command.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }finally { 
            connection.Close();
            }

            return (rowAffected > 0);
        }

        public static DataTable GetAllPeople()
        {
            DataTable result = new DataTable();
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query =
             @"SELECT People.PersonID, People.NationalNo,
                   People.FirstName, People.SecondName, People.ThirdName, People.LastName,
		     	  People.DateOfBirth, People.Gendor,  
				  CASE
                       WHEN People.Gendor = 0 THEN 'Male'

                  ELSE 'Female'

                  END as GendorCaption ,
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.FirstName";
            SqlCommand command = new SqlCommand(query,connection);
            try
            {
                connection.Open ();
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
        public static bool DeletePerson(int PersonID)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = @"Delete People 
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool IsPersonExist(int PersonID) {
            bool isFound=false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select Found=1 from People where PersonID=@PersonID";
            SqlCommand command= new SqlCommand(query, connection);
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
            catch (Exception ex) { 
            isFound=false;
            }
            finally
            {
                connection.Close();
               
            }
            return isFound;
  
        
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(ClsDataAccsess.connations);
            string query = "select Found=1 from People where NationalNo=@NationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
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
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;


        }
    }
}
