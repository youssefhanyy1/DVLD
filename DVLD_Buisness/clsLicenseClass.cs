using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsLicenseClass
    {
        public enum enMode {AddNew=0,Update=1 };
        public enMode Mode= enMode.AddNew;

        public int LicenseClassID { get; set;}
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float ClassFees { set; get; }

        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
            Mode= enMode.AddNew;
        }
        public clsLicenseClass(int LicenseClassID, string ClassName,
          string ClassDescription,
          byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)

        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
            Mode = enMode.Update;
        }
        private bool _AddNewLicenseClass()
        {
            clsLicenseClassData.stLicenseClassInfo licenseClassInfo = new clsLicenseClassData.stLicenseClassInfo();
            
            licenseClassInfo.LicenseClassID = this.LicenseClassID;
            licenseClassInfo.ClassName = this.ClassName;
            licenseClassInfo.DefaultValidityLength= this.DefaultValidityLength;
            licenseClassInfo.ClassDescription = this.ClassDescription;
            licenseClassInfo.MinimumAllowedAge= this.MinimumAllowedAge;
            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(licenseClassInfo);
            return (this.LicenseClassID != -1);
        }  
        private bool _UpdataLicenseClass()
        {
            clsLicenseClassData.stLicenseClassInfo LicenseClassInfo = new clsLicenseClassData.stLicenseClassInfo();

            LicenseClassInfo.LicenseClassID = this.LicenseClassID;
            LicenseClassInfo.ClassName = this.ClassName;
            LicenseClassInfo.ClassDescription = this.ClassDescription;
            LicenseClassInfo.MinimumAllowedAge = this.MinimumAllowedAge;
            LicenseClassInfo.DefaultValidityLength = this.DefaultValidityLength;
            LicenseClassInfo.ClassFees = this.ClassFees;

            return clsLicenseClassData.UpdateLicenseClass(LicenseClassInfo);
        }
        public static clsLicenseClass Find(int LicenseClassID)
        {
            clsLicenseClassData.stLicenseClassInfo LicenseClassInfo = new clsLicenseClassData.stLicenseClassInfo();

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref LicenseClassInfo))
            {
                return new clsLicenseClass(LicenseClassInfo.LicenseClassID,
                                           LicenseClassInfo.ClassName,
                                           LicenseClassInfo.ClassDescription,
                                           LicenseClassInfo.MinimumAllowedAge,
                                           LicenseClassInfo.DefaultValidityLength,
                                           LicenseClassInfo.ClassFees);
            }
            else
            {
                return null;
            }
        }

        public static clsLicenseClass Find(string ClassName)
        {
            clsLicenseClassData.stLicenseClassInfo LicenseClassInfo = new clsLicenseClassData.stLicenseClassInfo();

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassInfo))
            {
                return new clsLicenseClass(LicenseClassInfo.LicenseClassID,
                                           LicenseClassInfo.ClassName,
                                           LicenseClassInfo.ClassDescription,
                                           LicenseClassInfo.MinimumAllowedAge,
                                           LicenseClassInfo.DefaultValidityLength,
                                           LicenseClassInfo.ClassFees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseClass())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdataLicenseClass();
            }

            return false;
        }

    }
}
