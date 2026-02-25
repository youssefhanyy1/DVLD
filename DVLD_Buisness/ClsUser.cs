using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business 
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsUser()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = Username;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            ClsUserData.stUserData userData = new ClsUserData.stUserData();

            userData.PersonID = this.PersonID;
            userData.UserName = this.UserName;
            userData.Password = this.Password;
            userData.IsActice = this.IsActive;

            this.UserID = ClsUserData.AddNewUser(userData);

            return (this.UserID != -1);
        }
        private bool _UpdateUser()
        {
            ClsUserData.stUserData userData = new ClsUserData.stUserData();
            userData.UserID = this.UserID;
            userData.PersonID = this.PersonID;
            userData.UserName = this.UserName;
            userData.Password = this.Password;
            userData.IsActice = this.IsActive;
            return ClsUserData.UpdateUserInfo(this.UserID, userData);
        }
        public static clsUser FindByUserID(int UserID)
        {
            ClsUserData.stUserData UserStruct = new ClsUserData.stUserData();

            bool IsFound = ClsUserData.GetUserInfoByUserID(UserID, ref UserStruct);

            if (IsFound)
            {
                return new clsUser(UserID,
                                   UserStruct.PersonID,
                                   UserStruct.UserName,
                                   UserStruct.Password,
                                   UserStruct.IsActice); 
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            ClsUserData.stUserData UserStruct = new ClsUserData.stUserData();

            bool IsFound = ClsUserData.GetUserInfoByPersonID(PersonID, ref UserStruct);

            if (IsFound)
            {
                return new clsUser(UserStruct.UserID,  
                                   PersonID,           
                                   UserStruct.UserName,
                                   UserStruct.Password,
                                   UserStruct.IsActice);
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindByUsernameAndPassword(string UserName, string Password)
        {
            ClsUserData.stUserData UserStruct = new ClsUserData.stUserData();

            bool IsFound = ClsUserData.GetUserInfoByUsernameAndPassword(UserName, Password, ref UserStruct);

            if (IsFound)
            {
                return new clsUser(
                    UserStruct.UserID,
                    UserStruct.PersonID,
                    UserStruct.UserName,
                    UserStruct.Password,
                    UserStruct.IsActice
                );
            }
            else
            {
                return null;
            }
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }
        public static DataTable GetAllUsers()
        {
            return ClsUserData.GetAllUser();
        }

        public static bool DeleteUser(int UserID)
        {
            return ClsUserData.DeleteUser(UserID);
        }

        public static bool isUserExist(int UserID)
        {
            return ClsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return ClsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID)
        {
            return ClsUserData.IsUserExistForPersonID(PersonID);
        }

    }
}